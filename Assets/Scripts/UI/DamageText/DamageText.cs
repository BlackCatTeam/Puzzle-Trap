using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackCat.UI.DamageText {
	public class DamageText : MonoBehaviour
	{
        [SerializeField] Text textPrefab = null;

		public void SetDamage(float damage)
        {
            textPrefab.text = damage.ToString();
        }
        
    }
}
