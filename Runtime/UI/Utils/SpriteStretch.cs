using UnityEngine;

namespace Padoru.Core
{
    /// <summary>
    /// Component to stretch a sprite to the camera size
    /// </summary>
    public class SpriteStretch : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [SerializeField] private bool keepAspectRatio;

        [SerializeField] private bool stretchOnStart;
        
        /// <summary>
        /// Offset used to expand the sprite's height
        /// </summary>
        [SerializeField, Range(0f,1f)] private float spriteHeightOffsetPercentage;
        
        /// <summary>
        /// Offset used to expand the sprite's width
        /// </summary>
        [SerializeField, Range(0f,1f)] private float spriteWidthOffsetPercentage;
        
        private float worldSpaceWidth;
        private float worldSpaceHeight;
        
        private void Start()
        {
            if (spriteRenderer.sprite != null && stretchOnStart)
            {
                CalculateCameraSize();
                StretchSprite();
            }
        }

        public void StretchSprite()
        {
            if (worldSpaceHeight == 0f || worldSpaceWidth == 0f)
            {
                CalculateCameraSize();
            }
            
            //Reset scale
            gameObject.transform.localScale = Vector3.one;
            
            var spriteSize = spriteRenderer.bounds.size;

            var spriteSizeOffsetX = spriteSize.x * spriteWidthOffsetPercentage;
            var spriteSizeOffsetY = spriteSize.y * spriteHeightOffsetPercentage;
            
            var scaleFactorX = worldSpaceWidth / (spriteSize.x - spriteSizeOffsetX);
            var scaleFactorY = worldSpaceHeight / (spriteSize.y - spriteSizeOffsetY);

            if (keepAspectRatio)
            {
                if (scaleFactorX > scaleFactorY)
                {
                    scaleFactorY = scaleFactorX;
                }
                else
                {
                    scaleFactorX = scaleFactorY;
                }
            }

            gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
        }

        public void CalculateCameraSize()
        {
            var mainCamera = Camera.main;
            var topRightCorner = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
            worldSpaceWidth = topRightCorner.x * 2;
            worldSpaceHeight = topRightCorner.y * 2;
        }
    }
}
