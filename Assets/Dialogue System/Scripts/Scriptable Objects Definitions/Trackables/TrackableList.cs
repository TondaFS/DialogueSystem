using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Defines list of item of given Trackable items
    /// Eg. - list of locations player visited
    ///     - list of items in the inventory
    /// </summary>
    [CreateAssetMenu(fileName = "Trackable List", menuName = "Dialogue System/Trackable/New Trackable List", order = 4)]
    public class TrackableList : ScriptableObject
    {
        public TrackableType type;
        [Space(10)]
        public List<TrackableItem> items;
    }
}
