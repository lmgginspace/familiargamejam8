#if (UNITY_5 || UNITY_4)
using System;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public static class GameObjectExtensions
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos de gestión de hijos
        /// <summary>
        /// Busca un componente en los antecesores de esta instancia, y devuelve el más cercano si existe.
        /// </summary>
        public static T GetComponentInAncestor<T>(this GameObject g) where T : Component
        {
            if (g == null) return null;
            
            T comp = g.GetComponent<T>();
            if (comp != null)
                return comp;
            
            Transform t = g.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }
    }
    
}
#endif