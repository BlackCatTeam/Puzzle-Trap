using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BlackCat.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle;  
        [NonSerialized]
        GUIStyle playerNodeStyle;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        DialogueNode creatingNode = null; 
        [NonSerialized]
        DialogueNode deletingNode = null;    
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        Vector2 scrollPosition;

        const float backgroundSize = 50;



        [MenuItem("Window/Black Cat/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor),false,"Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            //TODO : Verificar porque não está carregando quando clica
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;

        }
        private void OnEnable()
        {            
            selectedDialogue = Selection.activeObject as Dialogue; 
            Selection.selectionChanged += OnSelectionChanged;
            SetNodeStyle();
        }        
        private void SetNodeStyle()
        {
            // IA
            nodeStyle = new GUIStyle();
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white; 
            
            //Falas do Jogador
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;
        }

        private void OnSelectionChanged()
        {
           Dialogue newDialog = Selection.activeObject as Dialogue;
            if (newDialog != null)
            {
                selectedDialogue = newDialog;
                Repaint();
            }
        }
        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");

            }
            else
            {
                ProcessEvents();
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
               Rect canvas =  GUILayoutUtility.GetRect(selectedDialogue.GetCanvasWidth(), selectedDialogue.GetCanvasHeight());
                Texture2D backgroundTexture =Resources.Load("DialogueEditorBackground") as Texture2D;
                Rect texCoords = new Rect(0, 0, selectedDialogue.GetCanvasWidth() / backgroundSize,selectedDialogue.GetCanvasHeight() / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture,texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {                   
                    DrawNode(node);                    
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                GUILayout.EndScrollView();
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }

            }
            
                        
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition, 
                    endPosition, 
                    startPosition + controlPointOffset, 
                    endPosition - controlPointOffset, 
                    Color.white, 
                    null, 
                    4f);


            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }

                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;

                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);


                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset -Event.current.mousePosition ;
                GUI.changed = true;
            }

            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }


        private void DrawNode(DialogueNode node)
        {
            SetNewColorNode(node);

            GUILayout.BeginArea(node.GetRect(), nodeStyle);

            node.SetText(EditorGUILayout.TextArea(node.GetText()));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove"))
            {
                deletingNode = node;
            }

            DrawLinkButton(node);

            if (GUILayout.Button("Add"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void SetNewColorNode(DialogueNode node)
        {
            switch (node.GetSpeaker())
            {
                case SpeakerType.Player:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
                        break;
                    }
                case SpeakerType.NPC1:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
                        break;
                    }
                case SpeakerType.NPC2:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
                        break;
                    }
                case SpeakerType.NPC3:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node4") as Texture2D;
                        break;
                    }
                case SpeakerType.Enemy1:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
                        break;
                    }
                case SpeakerType.Enemy2:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node6") as Texture2D;
                        break;
                    }
                case SpeakerType.Enemy3:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
                        break;
                    }

                default:
                    {
                        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
                        break;
                    }
            }
        }

        private void DrawLinkButton(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {

                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else{
                if (GUILayout.Button("Child"))
                {

                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;                    
                }
                    
            }
            return foundNode;
        }

    }
}