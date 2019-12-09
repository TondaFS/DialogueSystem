using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class Conversation : MonoBehaviour
    {
        public Image leftSpeakerImage;
        public Text leftSpeakerName;
        
        [Space(5)]
        public Image rightSpeakerImage;
        public Text rightSpeakerName;

        [Space(15)]
        public Dialogue startingDialogue;

        [Space(15)]
        public Transform dialogueWindow;
        public Text dialogueText;
        public GameObject dialogueOptionPrefab;
        public GameObject nextButton;


        private Queue<DialogueLine> lines;
        private Dialogue currentDialogue;

        private List<GameObject> optionButtons;

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

            currentDialogue = currentDialogue.dialogueOptions[id].subsequentDialogue;
            PrepareNextDialogues();
            ShowNextDialogueLine();
        }

        private void PrepareNextDialogues()
        {
            lines = new Queue<DialogueLine>();
            foreach (DialogueLine dl in currentDialogue.dialogue)
            {
                lines.Enqueue(dl);
            }
        }



    private void ShowNextDialogueLine()
        {
            if (lines.Count == 0)
            {
                Debug.LogWarning("there is no dialogue line left!");
                return;
            }

            DialogueLine dl = lines.Dequeue();
            dialogueText.text = dl.dialogueText;
            LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);

            if (currentDialogue.actorsOnTheLeft.Contains(dl.speaker))
            {
                
                leftSpeakerImage.sprite = dl.speaker.image;
                leftSpeakerName.text = dl.speaker.actorName;
                leftSpeakerName.color = dl.speaker.dialogueColor;
                leftSpeakerImage.gameObject.SetActive(true);
                leftSpeakerName.gameObject.SetActive(true);
                rightSpeakerImage.gameObject.SetActive(false);
                rightSpeakerName.gameObject.SetActive(false);

            }
            else
            {
                rightSpeakerImage.sprite = dl.speaker.image;
                rightSpeakerName.text = dl.speaker.actorName;
                rightSpeakerName.color = dl.speaker.dialogueColor;
                leftSpeakerImage.gameObject.SetActive(false);
                leftSpeakerName.gameObject.SetActive(false);
                rightSpeakerImage.gameObject.SetActive(true);
                rightSpeakerName.gameObject.SetActive(true);
            }

            if (lines.Count != 0)
            {
                nextButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(false);
                if (currentDialogue.dialogueOptions == null || currentDialogue.dialogueOptions.Length == 0)
                {
                    Debug.Log("This is the end.");
                    return;
                }

                for (int i = 0; i < currentDialogue.dialogueOptions.Length; i++)
                {
                    DialogueOption opt = currentDialogue.dialogueOptions[i];
                    GameObject go = Instantiate(dialogueOptionPrefab) as GameObject;
                    go.transform.SetParent(dialogueWindow);
                    DialogueOptionButton button = go.gameObject.GetComponent<DialogueOptionButton>();
                    button.dialogueOptionID = i;
                    button.optionText.text = opt.optionText;                    
                    optionButtons.Add(go);
                }
                
            }
        }

    }
}
