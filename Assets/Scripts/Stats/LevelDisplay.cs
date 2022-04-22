using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.Stats {
	public class LevelDisplay : MonoBehaviour
	{
		// Start is called before the first frame update
		BaseStats experience;
		void Start()
		{
			experience = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();

		}

		// Update is called once per frame
		void Update()
		{
			GetComponent<Text>().text = experience.GetLevel().ToString();
		}
	}
}
