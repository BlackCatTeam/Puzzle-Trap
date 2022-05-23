using BlackCat.Control;
using BlackCat.Inventories;
using System;
using UnityEngine;

namespace BlackCat.Core
{
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {

        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }
        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }
    }
}
