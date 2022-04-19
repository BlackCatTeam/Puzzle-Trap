using BlackCat.Attributes;
using BlackCat.Combat;
using BlackCat.Core;
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
        Health healthScript;
        
        void Start()
        {
            MoverScript = this.GetComponent<Mover>();
            FighterScript = this.GetComponent<Fighter>();
            healthScript = this.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (healthScript.IsDead()) return;

            this.Inputs();
        }

        private void Inputs()
        {
            if (InteractWithCombat()) return;
            if (!InteractWithMovement()) print("Nothing to Click");
            
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null) continue;                
                if (!FighterScript.CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(0))
                {
                    FighterScript.Attack(target.gameObject);
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
                    this.MoverScript.StartMoveAction(hit.point,1f);
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
