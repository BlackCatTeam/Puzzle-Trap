using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat {
	public class WeaponPickup : MonoBehaviour
	{
		[SerializeField]Weapon weapon = null;
		[SerializeField] float respawnTime = 5f;
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

				StartCoroutine(HideForSeconds(5f));
            }
        }

		private IEnumerator HideForSeconds(float seconds)
        {
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
			GetComponent<SphereCollider>().enabled = shouldShow;
			foreach (Transform child in transform)
            {
				child.gameObject.SetActive(shouldShow);
            }
		}        
    }
}
