using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Trackable Item", menuName = "Dialogue System/Trackable/New Trackable Item", order = 3)]
    public class TrackableItem : ScriptableObject
    {
        public TrackableType type;

        [Space(10)]
        public string itemName;
        public Sprite itemImage;
    }
}
