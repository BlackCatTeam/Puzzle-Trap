using BlackCat.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BlackCat.Saving { 
    [ExecuteAlways]
	public class SaveableEntity : MonoBehaviour
	{
		[SerializeField] private string uniqueIdentifier = "";
        
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

		public string GetUniqueIdentifier()
		{
			return uniqueIdentifier.ToString();
        }
		public object CaptureState()
        {		
            Dictionary<string,object> state = new Dictionary<string, object>();
            foreach ( ISaveable saveable in GetComponents<ISaveable>())
            {
              state[saveable.GetType().ToString()] =  saveable.CaptureState();
            }
            return state;
        }
		public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }

          
        }
        #if UNITY_EDITOR
        void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;


            UnityEditor.SerializedObject serializeObject = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty property = serializeObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializeObject.ApplyModifiedProperties();
            }
            globalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {

            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
#endif
    }
}
