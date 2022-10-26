using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string UniqueID;
        public string Text;
        public List<string> Children = new List<string>();
        public Rect Rect = new Rect(50,50,200,100);
    }
}