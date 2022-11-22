using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool _isPlayerSpeaking;
        [SerializeField] private string _text;
        [SerializeField] private List<string> _children = new List<string>();
        [SerializeField] private Rect _rect = new Rect(50,50,200,100);
        [SerializeField] private string _onEnterAction;
        [SerializeField] private string _onExitAction;
        [SerializeField] private Condition _condition;
        
        public Rect GetRect() => _rect;
        public List<string> GetChildren() => _children;
        public string GetText() => _text;
        public bool IsPlayerSpeaking() => _isPlayerSpeaking;
        public string GetEnterAction() => _onEnterAction;
        public string GetExitAction() => _onExitAction;

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return _condition.Check(evaluators);
        }
        
#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            _rect.position = position;
            EditorUtility.SetDirty(this);
        }
        
        public void SetText(string text)
        {
            if (text == _text)
            {
                return;
            }
            
            Undo.RecordObject(this, "Update Dialogue Node Text");
            _text = text;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Added Dialogue Link");
            _children.Add(childID);
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Removed Dialogue Link");
            _children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
        
        public void SetIsPlayerSpeaking(bool isPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            _isPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}