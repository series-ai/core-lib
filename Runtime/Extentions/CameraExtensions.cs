using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;
using Object = UnityEngine.Object;

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

        public static void AddLayer(this Camera camera, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            camera.AddLayer(layer);
        }

        public static void AddLayer(this Camera camera, int layer)
        {
            camera.cullingMask |= 1 << layer;
        }

        public static void RemoveLayer(this Camera camera, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            camera.RemoveLayer(layer);
        }

        public static void RemoveLayer(this Camera camera, int layer)
        {
            camera.cullingMask &=  ~(1 << layer);
        }

        public static void ToggleLayer(this Camera camera, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            camera.ToggleLayer(layer);
        }

        public static void ToggleLayer(this Camera camera, int layer)
        {
            camera.cullingMask ^= 1 << layer;
        }
        
        public static Texture2D TakeScreenShot(this Camera camera, int width, int height)
        {
            // Create render texture
            var renderTexture = new RenderTexture(width, height, 24);
            camera.targetTexture = renderTexture;
            
            // Create screenshot texture
            var screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            screenShot.name = $"Screenshot {DateTime.Now}";
            
            // Render screenshot
            camera.Render();
            
            // Save texture
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenShot.Apply();
            
            // Cleanup
            RenderTexture.active = null; // Added to avoid errors
            camera.targetTexture = null;
            Object.Destroy(renderTexture);
            
            // Debug.Log($"Took screenshot to: {screenShot.name}");

            return screenShot;
        }
    }
}