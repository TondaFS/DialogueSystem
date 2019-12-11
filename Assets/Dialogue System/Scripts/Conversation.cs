using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class Conversation : MonoBehaviour
    {
        public static Conversation Instance;

        public delegate void ConversationAction();
        public static event ConversationAction ConversationEnded;
        
        public Dialogue startingDialogue;
        public Trackables trackableThings;

        [Tooltip("If unchecked the text is aligned to the left.")]
        public bool alignTextToSpeakerSide;

        [Tooltip("If unchecked the text is gonna be white.")]
        public bool applySpeakerColorToDialogueLine;

        [Tooltip("If unchecked the text is aligned to the left.")]
        public bool alignNonSideSpeakerToCenter = true;

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

            _fadeToBlack = FindObjectOfType<FadeToBlack>();
            _dialogueAudioSource = GetComponent<AudioSource>();
            if (_dialogueAudioSource == null)
                _dialogueAudioSource = this.gameObject.AddComponent<AudioSource>();
        }

        private void Start()
        {
            currentDialogue = startingDialogue;
            leftSpeakerImage.gameObject.SetActive(false);
            rightSpeakerImage.gameObject.SetActive(false);
            leftSpeakerName.gameObject.SetActive(false);
            rightSpeakerName.gameObject.SetActive(false);
            nextButton.SetActive(false);
            dialogueText.text = "";
            optionButtons = new List<GameObject>();            

            PrepareNextDialogues();
            ShowNextDialogueLine();

            _fadeToBlack.StartFadeOut();
        }

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
            else
            {
                nextButton.SetActive(false);
                if (currentDialogue.dialogueOptions == null || currentDialogue.dialogueOptions.Length == 0)
                {
                    ConversationEnded?.Invoke();
                    return;
                }

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

        private bool DoesMeetPrerequisities(List<TrackableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!trackableThings.DoesHaveTrackable(items[i]))
                    return false;
            }

            return true;
        }

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

            if (currentDialogue.actorsOnTheLeft.Contains(dl.speaker))
            {
                if (alignTextToSpeakerSide)
                    dialogueText.alignment = TextAnchor.UpperLeft;

                leftSpeakerImage.sprite = dl.speaker.image;
                leftSpeakerName.text = dl.speaker.actorName;
                leftSpeakerName.color = dl.speaker.dialogueColor;

                leftSpeakerImage.gameObject.SetActive(true);
                leftSpeakerName.gameObject.SetActive(true);
                rightSpeakerImage.gameObject.SetActive(false);
                rightSpeakerName.gameObject.SetActive(false);

            }
            else if (currentDialogue.actorsOnTheRight.Contains(dl.speaker))
            {
                if (alignTextToSpeakerSide)
                    dialogueText.alignment = TextAnchor.UpperRight;

                rightSpeakerImage.sprite = dl.speaker.image;
                rightSpeakerName.text = dl.speaker.actorName;
                rightSpeakerName.color = dl.speaker.dialogueColor;
                leftSpeakerImage.gameObject.SetActive(false);
                leftSpeakerName.gameObject.SetActive(false);
                rightSpeakerImage.gameObject.SetActive(true);
                rightSpeakerName.gameObject.SetActive(true);
            }
            else
            {
                if (alignNonSideSpeakerToCenter)
                    dialogueText.alignment = TextAnchor.UpperCenter;

                leftSpeakerImage.gameObject.SetActive(false);
                leftSpeakerName.gameObject.SetActive(false);
                rightSpeakerImage.gameObject.SetActive(false);
                rightSpeakerName.gameObject.SetActive(false);
            }

            if (_dialogueAudioSource.isPlaying)
                _dialogueAudioSource.Stop();

            if (dl.audio != null)
            {
                _dialogueAudioSource.clip = dl.audio;
                _dialogueAudioSource.Play();
            }
        }

        private void ExecuteAllDialogueActions(List<DialogueAction> actions)
        {
            foreach (DialogueAction act in actions)
                act.DoAction();
        }
    }
}
