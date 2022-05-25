using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackCat.Dialogue
{    
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private SpeakerType speaker = SpeakerType.NPC1;
        [TextArea][SerializeField] private string text;
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect rect = new Rect(0,0,200,200);
        [SerializeField] private ActionType beginAction;
        [SerializeField] private ActionType exitAction;
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private GameObject audioDubb;

        public GameObject GetDubbing()
        {
            return audioDubb;
        }
        public void SetDubbing(GameObject audio)
        {
            audioDubb = audio;
        }
        public AnimationClip GetAnimationClip()
        {
            return animationClip;
        }
        public void SetAnimationClip(AnimationClip animation)
        {
            animationClip = animation;
        }
        public string GetText()
        {
            return this.text;
        }
        public List<string> GetChildren()
        {
            return this.children;
        }
        public ActionType GetAction(bool IsBeginAction)
        {
            if (IsBeginAction)
                return this.beginAction;
            else
                return this.exitAction;
        }
        public void SetAction(ActionType action,bool IsBeginAction)
        {
            if (IsBeginAction)
            {
                this.beginAction = action;
            }
            else
                this.exitAction = action;
        }
        public Rect GetRect()
        {
            return this.rect;
        }
        public bool IsPlayerSpeaking()
            {if (speaker == SpeakerType.Player)
                return true;
            return false;
        }
        public SpeakerType GetSpeaker()
        {
            return speaker;
        }
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node Position");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
        public void SetText(string newText)
        {
            if (newText != this.text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                this.text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string ChildId)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(ChildId);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childId)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childId);
            EditorUtility.SetDirty(this);
        }


        public void SetSpeaker(SpeakerType newSpeaker)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");

            speaker = newSpeaker;
            EditorUtility.SetDirty(this);
        }


#endif
    }
}
