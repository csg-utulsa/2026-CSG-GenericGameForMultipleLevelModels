using UnityEngine;

namespace RPG.Core
//core namespaces apply to core game scripts
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        void LateUpdate()
        {
            transform.position = target.position;
            //sets the camera position to the position of the target
        }
    }
}