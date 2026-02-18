using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
            //sets the transform of the object to the cameras forward vector, facing the object the same way as the camera
            //set as a late update so character rotation wont affect the health bar
        }
    }

}