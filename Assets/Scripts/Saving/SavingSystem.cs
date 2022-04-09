using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BlackCat.Saving {
	public class SavingSystem : MonoBehaviour
	{
		public void Save(string saveFile)
        {
            string path = this.GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
			using (FileStream stream = File.Open(path, FileMode.Create))
            {
				byte[] bytes = Encoding.UTF8.GetBytes("Hello World!");
				stream.Write(bytes, 0, bytes.Length);
			}
				

        }

		public void Load(string saveFile)
		{
			string path = this.GetPathFromSaveFile(saveFile);
			print("Saving to " + path);
			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				print(Encoding.UTF8.GetString(buffer));
				
			}
		}

		private string GetPathFromSaveFile(string saveFile)
        {
			return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
	}
}
