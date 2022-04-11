using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Saving {
	[System.Serializable]
	public class SerializableVector3
	{
		float x, y, z;

		public SerializableVector3(Vector3 vector)
        {
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
        }

		public Vector3 ToVector3()
        {
			return new Vector3(this.x, this.y, this.z);
        }
	}
}
