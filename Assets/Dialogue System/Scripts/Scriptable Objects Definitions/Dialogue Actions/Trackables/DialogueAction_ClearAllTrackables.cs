using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Clear All Trackables", menuName = "Dialogue System/Dialogue Actions/Trackables/Clear All Trackables", order = 7)]
    public class DialogueAction_ClearAllTrackables : DialogueAction
    {
        public override void DoAction()
        {
            Conversation.Instance.trackableThings.ClearAllTrackables();
        }
    }
}
