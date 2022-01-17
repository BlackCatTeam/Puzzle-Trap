using UnityEngine;
namespace BlackCat.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        Transform target;
        // Update is called once per frame
        void LateUpdate()
        {
            this.transform.position = target.position;

        }
    }
}
