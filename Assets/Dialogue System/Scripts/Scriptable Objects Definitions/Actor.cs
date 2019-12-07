using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Dialogue System/New Actor")]
    public class Actor : ScriptableObject
    {
        public string actorName = "Unknown Name";
        public Sprite image;
        public Color dialogueColor;
    }
}
