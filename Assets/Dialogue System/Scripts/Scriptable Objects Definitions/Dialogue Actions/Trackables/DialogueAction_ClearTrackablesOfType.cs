using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Clear Trackables of Type", menuName = "Dialogue System/Dialogue Actions/Trackables/Clear Trackables of Type", order = 8)]
    public class DialogueAction_ClearTrackablesOfType : DialogueAction
    {
        [SerializeField] private TrackableType _type;

        public override void DoAction()
        {
            Conversation.Instance.trackableThings.ClearTrackableList(_type);
        }

    }
}
