using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private DialogueNode[] _dialogueNodes;
    }
}