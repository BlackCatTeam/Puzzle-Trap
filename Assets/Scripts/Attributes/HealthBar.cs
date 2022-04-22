using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.Attributes {
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] Health healthComponent = null;
		[SerializeField] RectTransform foreground = null;
		[SerializeField] Canvas canvas = null;
		void Update()
		{
			float healthFraction = healthComponent.GetFraction();
			if (Mathf.Approximately(healthFraction, 0f) || Mathf.Approximately(healthFraction, 1f))
			{ 
				canvas.enabled = false;
				return;
			}
			canvas.enabled = true;

			foreground.localScale = new Vector3(healthFraction, 1, 1);
		}
	}
}
