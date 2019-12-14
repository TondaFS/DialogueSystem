using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    /// <summary>
    /// Class manages all conversation in the scene. Has reference to starting dialogue, all UI objects in the scene and trackable lists.
    /// </summary>
    public class Conversation : MonoBehaviour
    {
        public static Conversation Instance;

        public delegate void ConversationAction();
        /// <summary>
        /// Event raised when the converastion ends and no other buttons or actions are left/set
        /// </summary>
        public static event ConversationAction ConversationEnded;
        
        public Dialogue startingDialogue;
        /// <summary>
        /// Lists of all trackable items in the scene.
        /// </summary>
        public Trackables trackableThings;

        [Tooltip("If unchecked the text is aligned to the left.")]
        public bool alignTextToSpeakerSide;

        [Tooltip("If unchecked the text is gonna be white.")]
        public bool applySpeakerColorToDialogueLine;

        [Tooltip("If unchecked the text is aligned to the left.")]
        public bool alignNonSideSpeakerToCenter = true;

        [Tooltip("If unchecked only the current speake is shown.")]
        public bool showBothSpeakersAllTheTime = false;

        [Tooltip("If unchecked current trackables in 'inventory' won't be erased, allowing to set initial items or keep same items between scenes.")]
        public bool clearAllTrackableItemsAtStart = true;

        [Header("UI Elements")]
        [Space(5)]
        public Image leftSpeakerImage;
        public Text leftSpeakerName;
        
        [Space(5)]
        public Image rightSpeakerImage;
        public Text rightSpeakerName;

        [Space(15)]
        public Transform dialogueWindow;
        public Text dialogueText;
        /// <summary>
        /// Prefab, which will be instantiated as a dialogue option  
        /// </summary>
        public GameObject dialogueOptionPrefab;
        public GameObject nextButton;
        
        /// <summary>
        /// All dialogue lines in current dialogue
        /// </summary>
        private Queue<DialogueLine> lines;
        private Dialogue currentDialogue;

        private List<GameObject> optionButtons;
        private FadeToBlack _fadeToBlack;
        private AudioSource _dialogueAudioSource;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }

            //finds necessary references
            _fadeToBlack = FindObjectOfType<FadeToBlack>();
            _dialogueAudioSource = GetComponent<AudioSource>();
            if (_dialogueAudioSource == null)
                _dialogueAudioSource = this.gameObject.AddComponent<AudioSource>();
        }

        /// <summary>
        /// Prepares the scene
        /// </summary>
        private void Start()
        {
            currentDialogue = startingDialogue;
                        
            if (!showBothSpeakersAllTheTime)
            {
                leftSpeakerImage.gameObject.SetActive(false);
                rightSpeakerImage.gameObject.SetActive(false);
                leftSpeakerName.gameObject.SetActive(false);
                rightSpeakerName.gameObject.SetActive(false);
            }
            else
            {
                leftSpeakerImage.gameObject.SetActive(true);
                rightSpeakerImage.gameObject.SetActive(true);
                leftSpeakerName.gameObject.SetActive(true);
                rightSpeakerName.gameObject.SetActive(true);
            }

            nextButton.SetActive(false);
            dialogueText.text = "";
            optionButtons = new List<GameObject>();

            if (clearAllTrackableItemsAtStart)
                trackableThings.ClearAllTrackables();

            PrepareNextDialogues();
            ShowNextDialogueLine();

            _fadeToBlack.StartFadeOut();
        }

        //Register to button events
        private void OnEnable()
        {
            ContinueButton.ContinueButtonClicked += OnContinueButtonClick;
            DialogueOptionButton.DialogueOptionClicked += OnDialogueOptionClicked;
        }
        private void OnDisable()
        {
            ContinueButton.ContinueButtonClicked -= OnContinueButtonClick;
            DialogueOptionButton.DialogueOptionClicked -= OnDialogueOptionClicked;
        }

        

        private void OnContinueButtonClick()
        {
            ShowNextDialogueLine();
        }
        /// <summary>
        /// When user clicks option button, execute all choice actions and then prepares new dialogue if there is any
        /// </summary>
        /// <param name="id">position of the choice in the array</param>
        private void OnDialogueOptionClicked(int id)
        {
            for (int i = optionButtons.Count- 1; i >= 0; i--)
            {
                Destroy(optionButtons[i]);
            }

            optionButtons.Clear();
            DialogueOption option = currentDialogue.dialogueOptions[id];

            if (option.actions != null)
                ExecuteAllDialogueActions(option.actions);
                        
            if (option.subsequentDialogue != null)
            {
                currentDialogue = option.subsequentDialogue;
                PrepareNextDialogues();
                ShowNextDialogueLine();
            }
        }

        /// <summary>
        /// Add all dialogue lines to the queue and executes all starting dialogue actions.
        /// </summary>
        private void PrepareNextDialogues()
        {
            lines = new Queue<DialogueLine>();
            foreach (DialogueLine dl in currentDialogue.dialogue)
            {
                lines.Enqueue(dl);
            }

            if (currentDialogue.actions != null)
                ExecuteAllDialogueActions(currentDialogue.actions);
        }

        /// <summary>
        /// Shows the next dialogue line if there is any and option buttons if we hit the end of dialogue.
        /// </summary>
        private void ShowNextDialogueLine()
        {
            if (lines.Count == 0)
            {
                Debug.LogWarning("There is no dialogue line left!");
                return;
            }

            DialogueLine dl = lines.Dequeue();
            SetDialogueWindow(dl);

            if (dl.actions != null)
                ExecuteAllDialogueActions(dl.actions);

            if (lines.Count != 0)
            {
                nextButton.SetActive(true);
            }
            //we are at the end of the dialogue, shows options
            else
            {
                nextButton.SetActive(false);
                if (currentDialogue.dialogueOptions == null || currentDialogue.dialogueOptions.Length == 0)
                {
                    ConversationEnded?.Invoke();
                    return;
                }

                //show each option in dialogue
                for (int i = 0; i < currentDialogue.dialogueOptions.Length; i++)
                {        
                    DialogueOption opt = currentDialogue.dialogueOptions[i];

                    if (opt.prerequisities != null && !DoesMeetPrerequisities(opt.prerequisities))
                        continue;

                    GameObject go = Instantiate(dialogueOptionPrefab) as GameObject;
                    go.transform.SetParent(dialogueWindow, false);
                    DialogueOptionButton button = go.gameObject.GetComponent<DialogueOptionButton>();
                    button.dialogueOptionID = i;
                    button.optionText.text = opt.optionText;                    
                    optionButtons.Add(go);
                }                
            }
        }

        /// <summary>
        /// Checks if the the given list of items is in the players 'inventory'.
        /// </summary>
        /// <param name="items">List of trackable items needed to be in players inventory.</param>
        /// <returns></returns>
        private bool DoesMeetPrerequisities(List<TrackableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!trackableThings.DoesHaveTrackable(items[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Shows the dialogue text, aligns text according the speaker if needed and changes color.
        /// </summary>
        /// <param name="dl">Dialogue line to be shown.</param>
        private void SetDialogueWindow(DialogueLine dl)
        {
            dialogueText.text = dl.dialogueText;

            if (!alignTextToSpeakerSide)
                dialogueText.alignment = TextAnchor.UpperLeft;

            if (applySpeakerColorToDialogueLine)
                dialogueText.color = dl.speaker.dialogueColor;
            else
                dialogueText.color = Color.white;

            //we want to make sure the text size container will be calculated for our text
            LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);

            //shows or hides speakers in the dialogue, changes their color and text anchor
            if (currentDialogue.actorsOnTheLeft.Contains(dl.speaker))
            {
                if (alignTextToSpeakerSide)
                    dialogueText.alignment = TextAnchor.UpperLeft;

                leftSpeakerImage.sprite = dl.speaker.image;
                leftSpeakerName.text = dl.speaker.actorName;
                leftSpeakerName.color = dl.speaker.dialogueColor;

                if (!showBothSpeakersAllTheTime)
                {
                    leftSpeakerImage.gameObject.SetActive(true);
                    leftSpeakerName.gameObject.SetActive(true);

                    rightSpeakerImage.gameObject.SetActive(false);
                    rightSpeakerName.gameObject.SetActive(false);
                }
            }
            else if (currentDialogue.actorsOnTheRight.Contains(dl.speaker))
            {
                if (alignTextToSpeakerSide)
                    dialogueText.alignment = TextAnchor.UpperRight;

                rightSpeakerImage.sprite = dl.speaker.image;
                rightSpeakerName.text = dl.speaker.actorName;
                rightSpeakerName.color = dl.speaker.dialogueColor;

                if (!showBothSpeakersAllTheTime)
                {
                    leftSpeakerImage.gameObject.SetActive(false);
                    leftSpeakerName.gameObject.SetActive(false);
                    rightSpeakerImage.gameObject.SetActive(true);
                    rightSpeakerName.gameObject.SetActive(true);
                }
            }
            else
            {
                if (alignNonSideSpeakerToCenter)
                    dialogueText.alignment = TextAnchor.UpperCenter;

                if (!showBothSpeakersAllTheTime)
                {
                    leftSpeakerImage.gameObject.SetActive(false);
                    leftSpeakerName.gameObject.SetActive(false);
                    rightSpeakerImage.gameObject.SetActive(false);
                    rightSpeakerName.gameObject.SetActive(false);
                }
            }

            //stop audio sound if there is any
            if (_dialogueAudioSource.isPlaying)
                _dialogueAudioSource.Stop();

            //play audio if there is any
            if (dl.audio != null)
            {
                _dialogueAudioSource.clip = dl.audio;
                _dialogueAudioSource.Play();
            }
        }

        /// <summary>
        /// Do all actions in the list
        /// </summary>
        /// <param name="actions">list of actions needed to be executed</param>
        private void ExecuteAllDialogueActions(List<DialogueAction> actions)
        {
            foreach (DialogueAction act in actions)
                act.DoAction();
        }
    }
}
