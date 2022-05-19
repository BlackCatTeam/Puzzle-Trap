using BlackCat.Attributes;
using BlackCat.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat {
	public class WeaponPickup : MonoBehaviour , IRaycastable
	{
        [Header("Caso o Item seja equipado automaticamente")]
        [SerializeField]WeaponConfig weapon = null;
		[SerializeField] float respawnTime = 5f;
        [Space(5)]
        [Header("Caso o Item Restaure Vida ao ser pego")]
        [SerializeField] bool isHealth = false;
        [SerializeField] float healthToRestore = 0;
	
		// Update is called once per frame
		void Update()
		{
			gameObject.transform.Rotate(0f, 1f, 0f, Space.Self);
		}

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (isHealth)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            
            StartCoroutine(HideForSeconds(5f));
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

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
