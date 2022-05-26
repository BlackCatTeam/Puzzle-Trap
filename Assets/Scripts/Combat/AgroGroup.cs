using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat
{
    public class AgroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;

        [SerializeField] bool ActivateOnStart = false;
        private void Start()
        {
            Activate(ActivateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach(var fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = shouldActivate; 
                }
                fighter.enabled = shouldActivate;
            }
        }
    }
}