using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string UniqueID;
        public string Text;
        public string[] Children;
        public Rect Rect = new Rect(0,0,200,100);
    }
}