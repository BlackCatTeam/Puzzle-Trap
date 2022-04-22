using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.Stats {
	public class ExperienceDisplay : MonoBehaviour
	{
		Experience experience;
		void Start()
		{
			experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();

		}

		// Update is called once per frame
		void Update()
		{
			GetComponent<Text>().text = experience.GetExperience().ToString();
		}
	}
}
