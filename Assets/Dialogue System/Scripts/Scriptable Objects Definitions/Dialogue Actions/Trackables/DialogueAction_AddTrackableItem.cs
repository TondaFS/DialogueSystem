using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Add Item to Trackables", menuName = "Dialogue System/Dialogue Actions/Trackables/Add Item To trackables", order = 5)]
    public class DialogueAction_AddTrackableItem : DialogueAction
    {
        [SerializeField] private TrackableItem _trackableItem; 

        public override void DoAction()
        {
            Conversation.Instance.trackableThings.AddItemToTrackable(_trackableItem);
        }
    }
}
