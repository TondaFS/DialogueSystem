using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Speaker", menuName = "Dialogue System/New Speaker", order = -2)]
    public class Speaker : ScriptableObject
    {
        public string actorName = "Unknown Name";
        public Sprite image;
        public Color dialogueColor = new Color(0, 0, 0, 1);
    }
}
