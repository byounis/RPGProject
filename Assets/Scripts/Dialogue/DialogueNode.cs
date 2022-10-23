using System;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string UniqueID;
        public string Text;
        public string[] Children;
    }
}