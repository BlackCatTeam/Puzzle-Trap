using BlackCat.Attributes;
using BlackCat.Control;
using UnityEngine;

namespace BlackCat.Combat {
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (!enabled) return false;
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;

        }
    }
}
