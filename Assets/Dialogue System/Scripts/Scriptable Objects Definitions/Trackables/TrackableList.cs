using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Trackable List", menuName = "Dialogue System/Trackable/New Trackable List", order = 4)]
    public class TrackableList : ScriptableObject
    {
        public TrackableType type;
        [Space(10)]
        public List<TrackableItem> items;
    }
}
