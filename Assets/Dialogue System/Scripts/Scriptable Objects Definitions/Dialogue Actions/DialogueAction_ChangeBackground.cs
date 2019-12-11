using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Change Background", menuName = "Dialogue System/Dialogue Actions/Change Background", order = 3)]
    public class DialogueAction_ChangeBackground : DialogueAction
    {
        [SerializeField] private Sprite _newBackground;

        public override void DoAction()
        {
            BackgroundImageHandler.Instance.ChangeBackground(_newBackground);
        }
    }
}