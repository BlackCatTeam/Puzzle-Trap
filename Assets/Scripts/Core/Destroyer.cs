using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cripts.Core {
	public class Destroyer : MonoBehaviour
	{
        [SerializeField] GameObject target = null;
        private void DestroyTarget()
        {
            Destroy(target);
        }
    }
}
