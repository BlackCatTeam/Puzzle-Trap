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
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 1f;
        [SerializeField] CursorMapping[] cursorMappings = null;
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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
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
            if (EventSystem.current.IsPointerOverGameObject())// Verifica se o Mouse está em cima de alguma UI
            {
                SetCursor(CursorType.UI);
                return true;
            }
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
            RaycastHit hit;
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);
            if (hasHit)
            {
                SetCursor(CursorType.Movement);

                if (Input.GetMouseButton(0))
                {
                    this.MoverScript.StartMoveAction(target,1f);
                }
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
            NavMeshPath path = new NavMeshPath();            
            bool HasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas,path);
            if (!HasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i],path.corners[i+1]);
            }
            return total;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
