using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlackCat.Saving {
	public class SavingSystem : MonoBehaviour
	{
		public IEnumerator LoadLastScene(string saveFile)
        {
			Dictionary<string, object> state = LoadFile(saveFile);
			if (state.ContainsKey("lastSceneBuildIndex"))
			{
				int BuildIndex = (int)state["lastSceneBuildIndex"];
				if (SceneManager.GetActiveScene().buildIndex != BuildIndex)
					yield return SceneManager.LoadSceneAsync(BuildIndex);
			}
			RestoreState(state);
        }
		public void Save(string saveFile)
        {
			Dictionary<string, object> state = LoadFile(saveFile);
			CaptureState(state);
			SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, object state)
        {
			string path = this.GetPathFromSaveFile(saveFile);
			print("Saving to " + path);
			using (FileStream stream = File.Open(path, FileMode.Create))
			         {

				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream,state);
			}
		}

		public void Load(string saveFile)
		{
			RestoreState(LoadFile(saveFile));
		}

        private Dictionary<string,object> LoadFile(string saveFile)
        {

			string path = this.GetPathFromSaveFile(saveFile);
			if (!File.Exists(path))
            {
				return new Dictionary<string, object>();
            }            
			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return (Dictionary<string, object>)formatter.Deserialize(stream);
			}
		}
		private void CaptureState(Dictionary<string, object> state)
		{
			foreach (SaveableEntity savable in FindObjectsOfType<SaveableEntity>())
			{
				state[savable.GetUniqueIdentifier()] = savable.CaptureState();
			}
			state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
		}
		private void RestoreState(Dictionary<string, object> state)
		{
			foreach (SaveableEntity loadble in FindObjectsOfType<SaveableEntity>())
			{
                string id = loadble.GetUniqueIdentifier();
				if (state.ContainsKey(id))
					loadble.RestoreState(state[id]);
			}
		}
		private string GetPathFromSaveFile(string saveFile)
        {
			return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
	}
}
