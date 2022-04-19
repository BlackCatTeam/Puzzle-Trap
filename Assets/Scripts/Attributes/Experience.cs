using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Attributes {
	public class Experience : MonoBehaviour
	{
		[SerializeField] float experiencePoints = 0f;

		public void GainExperience(float experience)
        {
			experiencePoints += experience;
        }
	}
}
