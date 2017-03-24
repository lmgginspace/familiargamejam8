#if (UNITY_5 || UNITY_4)
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public enum TransformAxis
    {
        X,
        Y,
        Z
    }
    
    public static class TransformExtensions
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos de gestión de hijos
        /// <summary>
        /// Destruye todos los hijos de esta instancia, llamando al método GameObject.Destroy de cada uno.
        /// </summary>
        public static void DestroyChildren(this Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
                GameObject.Destroy(t.GetChild(i).gameObject);
        }
        
        /// <summary>
        /// Destruye todos los hijos de esta instancia, llamando al método GameObject.DestroyImmediate de cada uno.
        /// </summary>
        public static void DestroyChildrenImmediate(this Transform t)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in t)
                children.Add(child);
            
            children.ForEach(child => GameObject.DestroyImmediate(child.gameObject));
        }
        
        /// <summary>
        /// Devuelve el primer hijo de esta instancia, o null en el caso de que no exista.
        /// </summary>
        public static Transform FirstChild(this Transform t)
        {
            return t.childCount > 0 ? t.GetChild(0) : null;
        }
        
        /// <summary>
        /// Busca en el primer hijo de esta instancia un componente que coincida con el parámetro de tipo especificado,
        /// y lo devuelve si es posible encontrarlo. En caso de que el hijo no disponga de un componente del tipo
        /// adecuado, o de que el propio hijo no exista, devolverá null.
        /// </summary>
        public static T FirstChild<T>(this Transform t) where T : Component
        {
            return t.childCount > 0 ? t.GetChild(0).GetComponent<T>() : null;
        }
        
        /// <summary>
        /// Devuelve el hijo de esta instancia cuyo índice coincide con el especificado como parámetro, o null en el
        /// caso de que no exista.
        /// </summary>
        public static Transform ChildAt(this Transform t, int index)
        {
            return (index >= 0 && index < t.childCount) ? t.GetChild(index) : null;
        }
        
        /// <summary>
        /// Busca en el hijo de esta instancia cuyo íncide coincide con el especificado, un componente que coincida con
        /// el parámetro de tipo especificado, y lo devuelve si es posible encontrarlo. En caso de que el hijo no
        /// disponga de un componente del tipo adecuado, o de que el propio hijo no exista, devolverá null.
        /// </summary>
        public static T ChildAt<T>(this Transform t, int index) where T : Component
        {
            return (index >= 0 && index < t.childCount) ? t.GetChild(index).GetComponent<T>() : null;
        }
        
        /// <summary>
        /// Devuelve el último hijo de esta instancia, o null en el caso de que no exista.
        /// </summary>
        public static Transform LastChild(this Transform t)
        {
            return t.childCount > 0 ? t.GetChild(t.childCount - 1) : null;
        }
        
        /// <summary>
        /// Busca en el último hijo de esta instancia un componente que coincida con el parámetro de tipo especificado,
        /// y lo devuelve si es posible encontrarlo. En caso de que el hijo no disponga de un componente del tipo
        /// adecuado, o de que el propio hijo no exista, devolverá null.
        /// </summary>
        public static T LastChild<T>(this Transform t) where T : Component
        {
            return t.childCount > 0 ? t.GetChild(t.childCount - 1).GetComponent<T>() : null;
        }
        
        /// <summary>
        /// Devuelve uno de los hijos directos de esta instancia elegido al azar, o null en el caso de que no disponga
        /// de ninguno.
        /// </summary>
        public static Transform RandomChild(this Transform t)
        {
            return t.childCount > 0 ? t.GetChild(global::UnityEngine.Random.Range(0, t.childCount)) : null;
        }
        
        // Métodos de transformación
        /// <summary>
        /// Cambia de signo uno de los componentes de la escala local de esta instancia.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar el cambio de signo.</param>
        public static void FlipScaleByAxis(this Transform t, TransformAxis axis)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    t.localScale.Scale(new Vector3(-1.0f, 1.0f, 1.0f));
                    break;
                
                case TransformAxis.Y:
                    t.localScale.Scale(new Vector3(1.0f, -1.0f, 1.0f));
                    break;
                
                case TransformAxis.Z:
                    t.localScale.Scale(new Vector3(1.0f, 1.0f, -1.0f));
                    break;
                
            }
        }
        
        /// <summary>
        /// Reinicia la transformación de esta instancia, moviéndolo al origen del sistema de coordenadas local,
        /// eliminando la rotación, y asignando la escala unitaria.
        /// </summary>
        public static void Reset(this Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
        
        /// <summary>
        /// Rota uno solo de los componentes de rotación de esta instancia según el espacio de coordenadas global.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar la rotación.</param>
        /// <param name="angle">Ángulo a añadir en torno al eje especificado.</param>
        public static void RotateByAxis(this Transform t, TransformAxis axis, float angle)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    t.rotation *= Quaternion.AngleAxis(angle, Vector3.right);
                    break;
                
                case TransformAxis.Y:
                    t.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
                    break;
                
                case TransformAxis.Z:
                    t.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
            }
        }
        
        /// <summary>
        /// Rota uno solo de los componentes de rotación de esta instancia según el espacio de coordenadas
        /// especificado.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar la rotación.</param>
        /// <param name="angle">Ángulo a añadir en torno al eje especificado.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void RotateByAxis(this Transform t, TransformAxis axis, float angle, Space transformSpace)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    if (transformSpace == Space.World)
                        t.rotation *= Quaternion.AngleAxis(angle, Vector3.right);
                    else
                        t.Rotate(angle, 0.0f, 0.0f, Space.Self);
                    break;
                
                case TransformAxis.Y:
                    if (transformSpace == Space.World)
                        t.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
                    else
                        t.Rotate(0.0f, angle, 0.0f, Space.Self);
                    break;
                
                case TransformAxis.Z:
                    if (transformSpace == Space.World)
                        t.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
                    else
                        t.Rotate(0.0f, 0.0f, angle, Space.Self);
                    break;
            }
        }
        
        /// <summary>
        /// Asigna el signo uno de los componentes de la escala local de esta instancia, sin modificar la magnitud.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar la asignación de signo.</param>
        public static void SetScaleFlipByAxis(this Transform t, TransformAxis axis, bool flipped)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    t.localScale = new Vector3(Mathf.Abs(t.localScale.x) * (flipped ? -1.0f : 1.0f),
                                               t.localScale.y,
                                               t.localScale.z);
                    break;
                
                case TransformAxis.Y:
                    t.localScale = new Vector3(t.localScale.x,
                                               Mathf.Abs(t.localScale.y) * (flipped ? -1.0f : 1.0f),
                                               t.localScale.z);
                    break;
                
                case TransformAxis.Z:
                    t.localScale = new Vector3(t.localScale.x,
                                               t.localScale.y,
                                               Mathf.Abs(t.localScale.z) * (flipped ? -1.0f : 1.0f));
                    break;
                
            }
        }
        
        /// <summary>
        /// Asigna el componente X de la posición global de esta instancia.
        /// </summary>
        /// <param name="value">Valor a asignar al componente X.</param>
        public static void SetPositionX(this Transform t, float value)
        {
            t.position = new Vector3(value, t.position.y, t.position.z);
        }
        
        /// <summary>
        /// Asigna el componente X de la posición de esta instancia en el espacio de coordenadas especificado.
        /// </summary>
        /// <param name="value">Valor a asignar al componente X.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void SetPositionX(this Transform t, float value, Space transformSpace)
        {
            if (transformSpace == Space.World)
                t.position = new Vector3(value, t.position.y, t.position.z);
            else
                t.localPosition = new Vector3(value, t.localPosition.y, t.localPosition.z);
        }
        
        /// <summary>
        /// Asigna el componente Y de la posición global de esta instancia.
        /// </summary>
        /// <param name="value">Valor a asignar al componente Y.</param>
        public static void SetPositionY(this Transform t, float value)
        {
            t.position = new Vector3(t.position.x, value, t.position.z);
        }
        
        /// <summary>
        /// Asigna el componente Y de la posición de esta instancia en el espacio de coordenadas especificado.
        /// </summary>
        /// <param name="value">Valor a asignar al componente Y.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void SetPositionY(this Transform t, float value, Space transformSpace)
        {
            if (transformSpace == Space.World)
                t.position = new Vector3(t.position.x, value, t.position.z);
            else
                t.localPosition = new Vector3(t.localPosition.x, value, t.localPosition.z);
        }
        
        /// <summary>
        /// Asigna el componente Z de la posición global de esta instancia.
        /// </summary>
        /// <param name="value">Valor a asignar al componente Z.</param>
        public static void SetPositionZ(this Transform t, float value)
        {
            t.position = new Vector3(t.position.x, t.position.y, value);
        }
        
        /// <summary>
        /// Asigna el componente Z de la posición de esta instancia en el espacio de coordenadas especificado.
        /// </summary>
        /// <param name="value">Valor a asignar al componente Z.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void SetPositionZ(this Transform t, float value, Space transformSpace)
        {
            if (transformSpace == Space.World)
                t.position = new Vector3(t.position.x, t.position.y, value);
            else
                t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, value);
        }
        
        /// <summary>
        /// Asigna el componente Z de la rotación global de esta instancia (en su representación de Euler).
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="value">Valor a asignar al componente Z.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void SetRotationEulerZ(this Transform t, float value)
        {
            t.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, value);
        }
        
        /// <summary>
        /// Asigna el componente Z de la rotación de esta instancia (en su representación de Euler) en el espacio de
        /// coordenadas especificado.
        /// </summary>
        /// <param name="value">Valor a asignar al componente Z.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void SetRotationEulerZ(this Transform t, float value, Space transformSpace)
        {
            if (transformSpace == Space.World)
                t.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, value);
            else
                t.localRotation = Quaternion.Euler(t.localRotation.eulerAngles.x,
                                                   t.localRotation.eulerAngles.y,
                                                   value);
        }
        
        /// <summary>
        /// Desplaza uno solo de los componentes de la posición global de esta instancia.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar el desplazamiento.</param>
        /// <param name="value">Valor a añadir a lo largo del eje especificado.</param>
        public static void TranslateByAxis(this Transform t, TransformAxis axis, float value)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    t.position += new Vector3(value, 0.0f, 0.0f);
                    break;
                
                case TransformAxis.Y:
                    t.position += new Vector3(0.0f, value, 0.0f);
                    break;
                
                case TransformAxis.Z:
                    t.position += new Vector3(0.0f, 0.0f, value);
                    break;
                
            }
        }
        
        /// <summary>
        /// Desplaza uno solo de los componentes de la posición de esta instancia en el espacio de coordenadas
        /// especificado.
        /// </summary>
        /// <param name="axis">Eje a lo largo del cual efectuar el desplazamiento.</param>
        /// <param name="value">Valor a añadir a lo largo del eje especificado.</param>
        /// <param name="transformSpace">Espacio de coordenadas a utilizar.</param>
        public static void TranslateByAxis(this Transform t, TransformAxis axis, float value, Space transformSpace)
        {
            switch (axis)
            {
                case TransformAxis.X:
                    if (transformSpace == Space.World)
                        t.position += new Vector3(value, 0.0f, 0.0f);
                    else
                        t.localPosition += new Vector3(value, 0.0f, 0.0f);
                    break;
                
                case TransformAxis.Y:
                    if (transformSpace == Space.World)
                        t.position += new Vector3(0.0f, value, 0.0f);
                    else
                        t.localPosition += new Vector3(0.0f, value, 0.0f);
                    break;
                
                case TransformAxis.Z:
                    if (transformSpace == Space.World)
                        t.position += new Vector3(0.0f, 0.0f, value);
                    else
                        t.localPosition += new Vector3(0.0f, 0.0f, value);
                    break;
                
            }
        }
    }
    
}
#endif