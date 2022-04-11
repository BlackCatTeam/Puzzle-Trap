using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat {
	public class WeaponPickup : MonoBehaviour
	{
		[SerializeField]Weapon weapon = null;
		// Start is called before the first frame update
		void Start()
		{
			
		}
	
		// Update is called once per frame
		void Update()
		{
			gameObject.transform.Rotate(0f, 1f, 0f, Space.Self);
		}

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
				other.GetComponent<Fighter>().EquipWeapon(weapon);
				Destroy(gameObject);
            }
        }
    }
}
