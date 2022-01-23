using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackCat.Control {
	public class PatrolPath : MonoBehaviour
	{
        [SerializeField]
        const float radiusGizmos = 0.3f;
        [SerializeField]
        Color colorGizmo = Color.white;
        private void OnDrawGizmos()
        {
            Gizmos.color = colorGizmo;
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 wayPointStart = GetWayPoint(i);
                Vector3 wayPointEnd = GetWayPoint(GetNextPosition(i));
                Gizmos.DrawSphere(wayPointStart, radiusGizmos);
                Gizmos.DrawLine(wayPointStart, wayPointEnd);
            }
        }

        public int GetNextPosition(int i)
        {

            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }
        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
