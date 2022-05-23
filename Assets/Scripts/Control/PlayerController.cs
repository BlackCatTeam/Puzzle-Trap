using BlackCat.Attributes;
using BlackCat.Combat;
using BlackCat.Core;
using BlackCat.Movement;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace BlackCat.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover MoverScript;
        Fighter FighterScript;
        Health healthScript;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }
        [SerializeField] float playerSpeed = 1f;
        [SerializeField] float raycastRadius = 1f;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;

        [SerializeField] CursorMapping[] cursorMappings = null;

        bool isDragginUI = false;
        void Awake()
        {
            MoverScript = this.GetComponent<Mover>();
            FighterScript = this.GetComponent<Fighter>();
            healthScript = this.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            this.Inputs();
        }

        private void Inputs()
        {
            if (InteractWithUI()) return;
            if (healthScript.IsDead()) 
            {
                SetCursor(CursorType.None);
                return; 
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.None);

        }

        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in RaycastAllSorted())
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }


       private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(),raycastRadius);
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;

        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
                isDragginUI = false;
            if (EventSystem.current.IsPointerOverGameObject())// Verifica se o Mouse está em cima de alguma UI
            {
                if (Input.GetMouseButtonDown(0))
                    isDragginUI = true;
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDragginUI)
                return true;

            return false; 
        }


        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];

        }
        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);
            if (hasHit)
            {
                if (!this.MoverScript.CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    this.MoverScript.StartMoveAction(target, playerSpeed);
                }
                SetCursor(CursorType.Movement);
            }
            return hasHit;
        }
        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;

            target = navMeshHit.position;

            
            return MoverScript.CanMoveTo(target);
        }



        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
