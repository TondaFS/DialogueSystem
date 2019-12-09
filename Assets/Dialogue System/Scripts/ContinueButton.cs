using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class ContinueButton : MonoBehaviour
    {
        public delegate void ContinueButtonDelegate();
        public static event ContinueButtonDelegate ContinueButtonClicked;

        public void OnButtonClicked()
        {
            ContinueButtonClicked?.Invoke();
        }
    }
}
