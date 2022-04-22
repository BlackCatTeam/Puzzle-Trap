using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.UI.DamageText {
	public class DamageTextSpawner : MonoBehaviour
	{

        [SerializeField] DamageText DamageTextPrefab = null;

        public void Spawn(float damage)
        {
            if (DamageTextPrefab == null) return;
            DamageText instance =Instantiate<DamageText>(DamageTextPrefab, transform);
            instance.SetDamage(damage);

        }
	}
}
