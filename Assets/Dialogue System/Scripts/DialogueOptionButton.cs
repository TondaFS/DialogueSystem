using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueOptionButton : MonoBehaviour
    {
        public delegate void DialogueOptionButtonAction(int id);
        public static event DialogueOptionButtonAction DialogueOptionClicked;

        public int dialogueOptionID;
        public Text optionText;


        public void DialogueOptionButtonClicked()
        {
            DialogueOptionClicked?.Invoke(dialogueOptionID);
        }
    }
}