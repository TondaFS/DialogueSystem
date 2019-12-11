using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Play Sound", menuName = "Dialogue System/Dialogue Actions/Play Sound", order = 0)]
    public class DialogueAction_PlaySound : DialogueAction
    {
        [SerializeField] private AudioClip _audioClip;

        public override void DoAction()
        {
            ActionSoundHandler.Instance.PlaySound(_audioClip);
        }
    }
}
