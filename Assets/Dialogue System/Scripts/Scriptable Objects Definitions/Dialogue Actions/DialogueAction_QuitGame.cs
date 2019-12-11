using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Quit Game", menuName = "Dialogue System/Dialogue Actions/Quit Game", order = 4)]
    public class DialogueAction_QuitGame : DialogueAction
    {
        public override void DoAction()
        {
            FadeToBlack.Instance.FadeInAndQuit();
        }
    }
}
