using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public abstract class DialogueAction : ScriptableObject
    {
        public abstract void DoAction();
    }
}
