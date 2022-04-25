using UnityEngine;

namespace Padoru.Core
{
    public static class CursorUtils
    {
        private static Ray ray;
        private static RaycastHit hit;

        public static bool Locked
        {
            get
            {
                return Cursor.lockState == CursorLockMode.Locked;
            }
        }

        public static void Lock()
        {
            if (Cursor.lockState == CursorLockMode.Locked) return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void Unlock()
        {
            if (Cursor.lockState == CursorLockMode.None) return;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public static Vector3 GetWorldPos2D()
        {
            return GetWorldPos2D(Camera.main);
        }

        public static Vector3 GetWorldPos2D(Camera camera)
        {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public static Vector3 GetWorldPos3D()
        {
            return GetWorldPos3D(Camera.main);
        }

        public static Vector3 GetWorldPos3D(Camera camera)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}