using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public struct DialogueLine
    {
        public Actor speaker;
        [TextArea(3, 4)]
        public string dialogueText;
    }

    [System.Serializable]
    public struct DialogueOption
    {
        [TextArea(2,3)]
        public string optionText;
        public Dialogue subsequentDialogue;
    }


    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue System/New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public Actor[] actorsOnTheLeft;
        public Actor[] actorsOnTheRight;

        [Header("Dialogue")]
        public DialogueLine[] dialogue;
        public DialogueOption[] dialogueOptions;
    }
}
