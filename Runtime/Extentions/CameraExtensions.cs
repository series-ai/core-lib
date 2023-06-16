using UnityEngine;

namespace Padoru.Core
{
    public static class CameraExtensions
    {
        public static Vector2 GetSize(this Camera camera)
        {
            var cameraSize = Vector2.zero;

            if (camera == null)
            {
                return cameraSize;
            }
            
            var topRightCorner = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
            var sizeX = topRightCorner.x * 2;
            var sizeY = topRightCorner.y * 2;
            cameraSize = new Vector2(sizeX, sizeY);

            return cameraSize;
        }
    }
}