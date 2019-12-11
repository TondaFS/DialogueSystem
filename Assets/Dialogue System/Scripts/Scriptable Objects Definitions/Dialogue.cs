using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public struct DialogueLine
    {
        [TextArea(3, 4)]
        public string dialogueText;
        public Speaker speaker;
        public AudioClip audio;
        public List<DialogueAction> actions;
    }

    [System.Serializable]
    public struct DialogueOption
    {
        [TextArea(2,3)]
        public string optionText;
        public Dialogue subsequentDialogue;
        public List<DialogueAction> actions;
        public List<TrackableItem> prerequisities;
    }


    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue System/New Dialogue", order = -1)]
    public class Dialogue : ScriptableObject
    {
        public List<Speaker> actorsOnTheLeft;
        public List<Speaker> actorsOnTheRight;

        [Header("Actions at the start of Dialogue")]
        public List<DialogueAction> actions;

        [Header("Dialogue")]
        public DialogueLine[] dialogue;

        [Header("Options")]
        public DialogueOption[] dialogueOptions;
    }
}
