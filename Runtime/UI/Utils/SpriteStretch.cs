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
        
        [Tooltip("This will use the main camera screen size to stretch the sprite")]
        [SerializeField] private bool stretchOnStart;
        
        /// <summary>
        /// Offset used to expand the sprite's height
        /// </summary>
        [SerializeField, Range(0f,1f)] private float spriteHeightMarginPercentage;
        
        /// <summary>
        /// Offset used to expand the sprite's width
        /// </summary>
        [SerializeField, Range(0f,1f)] private float spriteWidthMarginPercentage;
        
        private void Start()
        {
            if (spriteRenderer.sprite != null && stretchOnStart)
            {
                var cameraSize = Camera.main.GetSize();
                StretchSprite(cameraSize);
            }
        }

        public void StretchSprite(Vector2 spaceBounds)
        {
            //Reset scale
            gameObject.transform.localScale = Vector3.one;
            
            var spriteSize = spriteRenderer.bounds.size;

            var spriteSizeOffsetX = spriteSize.x * spriteWidthMarginPercentage;
            var spriteSizeOffsetY = spriteSize.y * spriteHeightMarginPercentage;
            
            var scaleFactorX = spaceBounds.x / (spriteSize.x - spriteSizeOffsetX);
            var scaleFactorY = spaceBounds.y / (spriteSize.y - spriteSizeOffsetY);

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
    }
}
