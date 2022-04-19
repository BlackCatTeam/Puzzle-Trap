using BlackCat.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.Attributes {
	public class EnemyHealthDisplay : MonoBehaviour
	{
		Health health;
        GameObject fighter;
        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            health = fighter.GetComponent<Fighter>().GetActualTarget();

            GetComponent<Text>().text = health == null ? "N/A" : String.Format("{0:0}%", health.GetPercentage());

        }
    }
}
