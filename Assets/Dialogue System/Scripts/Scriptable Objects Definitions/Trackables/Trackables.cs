using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// List of all trackable types in the dialogue.
    /// </summary>
    [CreateAssetMenu(fileName = "Trackables", menuName = "Dialogue System/Trackable/Trackables", order = 0)]
    public class Trackables : ScriptableObject
    {
        public List<TrackableList> trackables;

        public void AddItemToTrackable(TrackableItem item)
        {
            for (int i = 0; i < trackables.Count; i++)
            {
                if (trackables[i].type.Equals(item.type))
                {
                    trackables[i].items.Add(item);
                    return;
                }
            }

            Debug.LogWarning("There is no trackable list of the type item belongs. " + item.type);
        }
        public void RemoveItemFromTrackable(TrackableItem item)
        {
            for (int i = 0; i < trackables.Count; i++)
            {
                if (trackables[i].type.Equals(item.type))
                {
                    if (trackables[i].items.Contains(item))
                    {
                        trackables[i].items.Remove(item);
                        return;
                    }
                    else
                    {
                        Debug.LogWarning("Item is not in the list! " + item);
                    }
                }
            }

            Debug.LogWarning("There is no trackable list of the type item belongs " + item.type);
        }
        public void ClearTrackableList(TrackableType type)
        {
            for (int i = 0; i < trackables.Count; i++)
            {
                if (trackables[i].type.Equals(type))
                {
                    trackables[i].items.Clear();
                    return;
                }
            }

            Debug.LogWarning("There is no trackable list of the type " + type);
        }
        public void ClearAllTrackables()
        {
            for (int i = 0; i < trackables.Count; i++)
            {
                trackables[i].items.Clear();
            }
        }

        public bool DoesHaveTrackable(TrackableItem item)
        {
            for (int i = 0; i < trackables.Count; i++)
            {
                if (trackables[i].type.Equals(item.type))
                    return trackables[i].items.Contains(item);
            }

            return false;
        }
    }
}
