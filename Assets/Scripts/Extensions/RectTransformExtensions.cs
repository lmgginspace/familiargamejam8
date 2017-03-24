#if (UNITY_5 || UNITY_4)
using System;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public static class RectTransformExtensions
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Anchor control methods
        /// <summary>
        /// Sets the x position of the anchor of this RectTransform to a single point.
        /// </summary>
        public static void SetAnchorX(this RectTransform rt, float anchorX)
        {
            rt.anchorMin = new Vector2(anchorX, rt.anchorMin.y);
            rt.anchorMax = new Vector2(anchorX, rt.anchorMax.y);
        }
        
        /// <summary>
        /// Sets the x position of the anchor of this RectTransform to a range of values.
        /// </summary>
        public static void SetAnchorX(this RectTransform rt, float anchorMinX, float anchorMaxX)
        {
            rt.anchorMin = new Vector2(anchorMinX, rt.anchorMin.y);
            rt.anchorMax = new Vector2(anchorMaxX, rt.anchorMax.y);
        }
        
        /// <summary>
        /// Sets the y position of the anchor of this RectTransform to a single point.
        /// </summary>
        public static void SetAnchorY(this RectTransform rt, float anchorY)
        {
            rt.anchorMin = new Vector2(rt.anchorMin.x, anchorY);
            rt.anchorMax = new Vector2(rt.anchorMax.x, anchorY);
        }
        
        /// <summary>
        /// Sets the y position of the anchor of this RectTransform to a range of values.
        /// </summary>
        public static void SetAnchorY(this RectTransform rt, float anchorMinY, float anchorMaxY)
        {
            rt.anchorMin = new Vector2(rt.anchorMin.x, anchorMinY);
            rt.anchorMax = new Vector2(rt.anchorMax.x, anchorMaxY);
        }
        
        // Margin control methods
        /// <summary>
        /// Sets all the margins of this RectTransform as if its anchors correspond to a rectangle.
        /// </summary>
        public static void SetAllMargins(this RectTransform rt, float margin)
        {
            rt.offsetMin = new Vector2(margin, margin);
            rt.offsetMax = new Vector2(-margin, -margin);
        }
        
        /// <summary>
        /// Sets the bottom margin of this RectTransform as if its anchors correspond to a rectangle.
        /// </summary>
        public static void SetBottomMargin(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
        
        /// <summary>
        /// Sets the left margin of this RectTransform as if its anchors correspond to a rectangle.
        /// </summary>
        public static void SetLeftMargin(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }
        
        /// <summary>
        /// Sets the right margin of this RectTransform as if its anchors correspond to a rectangle.
        /// </summary>
        public static void SetRightMargin(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }
        
        /// <summary>
        /// Sets the top margin of this RectTransform as if its anchors correspond to a rectangle.
        /// </summary>
        public static void SetTopMargin(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }
        
        // Position control methods
        /// <summary>
        /// Sets the x position of this RectTransform as if its anchors correspond to a single point.
        /// </summary>
        public static void SetPosX(this RectTransform rt, float posX)
        {
            rt.anchoredPosition = new Vector2(posX, rt.anchoredPosition.y);
        }
        
        /// <summary>
        /// Sets the y position of this RectTransform as if its anchors correspond to a single point.
        /// </summary>
        public static void SetPosY(this RectTransform rt, float posY)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);
        }
    }
    
}
#endif