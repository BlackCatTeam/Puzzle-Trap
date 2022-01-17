using BlackCat.Movement;
using UnityEngine;

namespace BlackCat.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover MoverScript;
        void Start()
        {
            MoverScript = this.GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                this.MoveToCursor();
            }
        }
        private void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {
                this.MoverScript.MoveTo(hit.point);
            }
        }
    }
}
