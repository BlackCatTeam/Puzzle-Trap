using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.Attributes {
	public class HealthDisplay : MonoBehaviour
	{
		Health health;
        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = $"{health.GetHealth().ToString()} / {health.GetMaxHealth()}";
        }
    }
}
