using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core {
	public class DestroyAfterEffect : MonoBehaviour
	{
		void Update()
		{
			if (!GetComponent<ParticleSystem>().IsAlive())
				Destroy(this.gameObject);
		}
	}
}
