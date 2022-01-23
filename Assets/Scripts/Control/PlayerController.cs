using BlackCat.Combat;
using BlackCat.Movement;
using System;
using System.Linq;
using UnityEngine;

namespace BlackCat.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover MoverScript;
        Fighter FighterScript;
        void Start()
        {
            MoverScript = this.GetComponent<Mover>();
            FighterScript = this.GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            this.Inputs();
        }

        private void Inputs()
        {
            if (InteractWithCombat()) return;
            if (!InteractWithMovement()) print("Nothing to Click");
            
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            
            foreach(var hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (!FighterScript.CanAttack(target)) continue;
                if (Input.GetMouseButtonDown(0))
                {
                    FighterScript.Attack(target);
                }
                return true;

            }
            return false;
        }


        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
            if (Input.GetMouseButton(0))
                {
                    this.MoverScript.MoveToAction(hit.point);
                }
            }
            return hasHit;


        }
        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
