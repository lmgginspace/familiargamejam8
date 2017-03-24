#if (UNITY_5 || UNITY_4)
using System;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public static class RectExtensions
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        public static Rect OffsetX(this Rect rect, float amount)
        {
            return new Rect(rect.x + amount, rect.y, rect.width, rect.height);
        }
        
        public static Rect OffsetY(this Rect rect, float amount)
        {
            return new Rect(rect.x, rect.y + amount, rect.width, rect.height);
        }
        
        public static Rect OffsetLeft(this Rect rect, float amount)
        {
            return Rect.MinMaxRect(rect.xMin + amount, rect.yMin, rect.xMax, rect.yMax);
        }
        
        public static Rect OffsetRight(this Rect rect, float amount)
        {
            return Rect.MinMaxRect(rect.xMin, rect.yMin, rect.xMax + amount, rect.yMax);
        }
        
        public static Rect WithSetWidth(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }
    }
    
}
#endif