using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Remove Trackable Item", menuName = "Dialogue System/Dialogue Actions/Trackables/Remove Trackable Item", order = 6)]
    public class DialogueAction_RemoveTrackableItem : DialogueAction
    {
        [SerializeField] private TrackableItem _trackableItem;

        public override void DoAction()
        {
            Conversation.Instance.trackableThings.RemoveItemFromTrackable(_trackableItem);
        }
    }
}
