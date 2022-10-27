using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private const float CanvasSize = 4000;
        private const float BackgroundSize = 50;
        
        private Dialogue _selectedDialogue = null;
        private Vector2 _scrollPosition;
        
        [NonSerialized] private GUIStyle _nodeStyle;
        [NonSerialized] private GUIStyle _playerNodeStyle;
        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private Vector2 _draggingOffset;
        [NonSerialized] private DialogueNode _creatingNode;
        [NonSerialized] private DialogueNode _deletingNode;
        [NonSerialized] private DialogueNode _linkingParentNode;
        [NonSerialized] private bool _draggingCanvas;
        [NonSerialized] private Vector2 _draggingCanvasOffset;

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if (dialogue != null)
            {
                ShowWindow();
                return true;
            }
            
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;

            _nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };

            _playerNodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node1") as Texture2D
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }
        
        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        private void SelectionChanged()
        {
            var activeDialogue = Selection.activeObject as Dialogue;
            if (activeDialogue != null)
            {
                _selectedDialogue = activeDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
                return;
            }
            
            ProcessEvents();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            var canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
            var backgroundTexture = Resources.Load("background") as Texture2D;
            var texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);
            GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, texCoords);
            
            foreach (var dialogueNode in _selectedDialogue.GetAllNodes())
            {
                DrawConnections(dialogueNode);
            }
            
            foreach (var dialogueNode in _selectedDialogue.GetAllNodes())
            {
                DrawNode(dialogueNode);
            }
            
            EditorGUILayout.EndScrollView();

            if (_creatingNode != null)
            {
                _selectedDialogue.CreateNode(_creatingNode);
                _creatingNode = null;
            }
            
            if (_deletingNode != null)
            {
                _selectedDialogue.DeleteNode(_deletingNode);
                _deletingNode = null;
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null)
            {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
                if (_draggingNode != null)
                {
                    _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
                
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingCanvas)
            {
                _draggingCanvas = false;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (var node in _selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private void DrawNode(DialogueNode dialogueNode)
        {
            var style = _nodeStyle;
            if (dialogueNode.IsPlayerSpeaking())
            {
                style = _playerNodeStyle;
            }
            
            GUILayout.BeginArea(dialogueNode.GetRect(), style);

            dialogueNode.SetText(EditorGUILayout.TextField(dialogueNode.GetText()));

            EditorGUILayout.Space();
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("x"))
            {
                _deletingNode = dialogueNode;
            }
            
            EditorGUILayout.Space();
            
            DrawLinkButtons(dialogueNode);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("+"))
            {
                _creatingNode = dialogueNode;
            }
            
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode dialogueNode)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    _linkingParentNode = dialogueNode;
                }
            }
            else if (_linkingParentNode == dialogueNode)
            {
                if (GUILayout.Button("Cancel"))
                {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.GetChildren().Contains(dialogueNode.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    _linkingParentNode.RemoveChild(dialogueNode.name);
                    _linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    _linkingParentNode.AddChild(dialogueNode.name);
                    _linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode dialogueNode)
        {
            Vector3 startPosition = new Vector2(dialogueNode.GetRect().xMax, dialogueNode.GetRect().center.y);
            foreach (var childNode in _selectedDialogue.GetAllChildren(dialogueNode))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                var controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                
                Handles.DrawBezier(
                    startPosition,
                    endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.black,
                    null,
                    4f);
            }
        }
    }
}