using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
    public static class CursorUtils
    {
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
    }
}