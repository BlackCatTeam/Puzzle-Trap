using System.Collections;
using System.Collections.Generic;
using TMPro;
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
			GetComponent<TextMeshProUGUI>().text = experience.GetExperience().ToString();
		}
	}
}
