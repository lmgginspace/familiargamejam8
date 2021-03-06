#if (UNITY_5 || UNITY_4)
using System;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public static class SpriteExtensions
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        public static void SetAbsoluteScale(this SpriteRenderer spriteRenderer, float scale)
        {
            spriteRenderer.transform.localScale = Vector3.one;
            Vector3 spriteSize = spriteRenderer.bounds.size;
            if (spriteSize.x > spriteSize.y)
                spriteRenderer.transform.localScale = new Vector3(scale / spriteSize.x, scale / spriteSize.x, 1.0f);
            else
                spriteRenderer.transform.localScale = new Vector3(scale / spriteSize.y, scale / spriteSize.y, 1.0f);
        }
        
        public static void SetAbsoluteScale(this SpriteRenderer spriteRenderer, Vector2 scale)
        {
            Vector3 spriteSize = spriteRenderer.bounds.size;
            spriteRenderer.transform.localScale = new Vector3(scale.x / spriteSize.x, scale.y / spriteSize.y, 1.0f);
        }
        
        public static void SetAbsoluteScale(this SpriteRenderer spriteRenderer, float scaleX, float scaleY)
        {
            Vector3 spriteSize = spriteRenderer.bounds.size;
            spriteRenderer.transform.localScale = new Vector3(scaleX / spriteSize.x, scaleY / spriteSize.y, 1.0f);
        }
    }
    
}
#endif