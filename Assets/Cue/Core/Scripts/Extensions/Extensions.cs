using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Cue.Core
{
    public static class Extensions
    {
        private static readonly System.Random rand = new();
        private const string colorField = "_BaseColor";

        #region Transform

        /// <summary>
        /// Destroys all children of this transform
        /// </summary>
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Disables all children of this transform
        /// </summary>
        public static void DisableChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        /// <returns>All children transforms of this transform</returns>
        public static Transform[] GetAllChildren(this Transform transform)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }
            return children.ToArray();
        }

        public static Transform Reset(this Transform transform, bool position = true, bool rotation = true, bool scale = true)
        {
            if (position)
                transform.localPosition = Vector3.zero;
            if (rotation)
                transform.localRotation = Quaternion.identity;
            if (scale)
                transform.localScale = Vector3.one;
            return transform;
        }

        #endregion Transform

        #region Renderer

        /// <summary>
        /// Sets the base color of a renderer
        /// </summary>
        /// <param name="renderer">Renderer to apply color to</param>
        /// <param name="color">Color to be applied on the renderer</param>
        public static void SetColor(this Renderer renderer, Color color) => renderer.material.SetColor(colorField, color);

        #endregion Renderer

        #region Camera

        /// <summary>
        /// Casts a ray from the mouse position
        /// </summary>
        /// <param name="cam">The current camera (suggested Camera.main)</param>
        /// <returns>Raycast hit with data on the hit object</returns>
        public static RaycastHit SendRaycastFromMousePosition(this Camera cam)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
                return hitInfo;
            return new RaycastHit();
        }

        #endregion Camera

        #region Collections

        /// <typeparam name="T">Type of item stored in collection</typeparam>
        /// <param name="collection">Collection of objects</param>
        /// <returns>The last item of a collection</returns>
        public static T LastItem<T>(this ICollection<T> collection)
        {
            List<T> list = collection.ToList();
            return list[list.Count - 1];
        }

        /// <typeparam name="T">Type of item stored in collection</typeparam>
        /// <param name="collection">Collection of objects</param>
        /// <returns>The first item of a collection</returns>
        public static T FirstItem<T>(this ICollection<T> collection)
        {
            List<T> list = collection.ToList();
            return list[0];
        }

        /// <typeparam name="T">Type of item stored in collection</typeparam>
        /// <param name="collection">Collection of objects</param>
        /// <returns>A random item from the collection</returns>
        public static T RandomItem<T>(this ICollection<T> collection)
        {
            List<T> list = collection.ToList();
            return list[rand.Next(list.Count)];
        }

        /// <summary>
        /// Turns a collection into a single string with values separated by a ' - '
        /// </summary>
        public static string Stringify(this ICollection<string> collection)
        {
            string[] strings = collection.ToArray();
            string newString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                newString += strings[i];
                if (i < strings.Length - 1)
                    newString += " - ";
            }
            return newString;
        }

        #endregion Collections

        #region Comparisons

        /// <param name="first">First value</param>
        /// <param name="second">Second value</param>
        /// <returns>The positive difference between the values</returns>
        public static float Difference(this float first, float second)
        {
            float valueA = first - second;
            float valueB = second - first;
            return valueA > valueB ? valueA : valueB;
        }

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// Is <paramref name="source"/> equal to any of the <paramref name="values"/>?
        /// </summary>
        public static bool IsAnyOf<T>(this T source, T val1, T val2, T val3 = default, T val4 = default, T val5 = default) where T : struct, System.Enum
        {
            T[] values = new T[] { val1, val2, val3, val4, val5 };
            foreach (T value in values)
            {
                if (source.Equals(value))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// The opposite of <see cref="object.Equals(object)"/>
        /// </summary>
        public static bool NotEquals(this object obj1, object obj2) => !obj1.Equals(obj2);

        /// <summary>
        /// Is this value within two other values
        /// </summary>
        public static bool IsWithin(this float value, float min, float max) => value >= min && value <= max;

        /// <summary>
        /// Is this value within two other values
        /// </summary>
        public static bool IsWithin(this double value, double min, double max) => value >= min && value <= max;

        /// <summary>
        /// Is this value within two other values
        /// </summary>
        public static bool IsWithin(this int value, int min, int max) => value >= min && value <= max;

        #endregion Comparisons

        #region Components

        public static T GetFirstComponentInAllParents<T>(this MonoBehaviour mb) where T : Component
        {
            Transform parent = mb.transform.parent;
            while (parent != null)
            {
                if (parent.TryGetComponent(out T component))
                    return component;

                parent = parent.parent;
            }
            return null;
        }

        /// <summary>
        /// Get a component and add it if it is not yet added to the gameobject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
                component = obj.AddComponent<T>();
            return component;
        }

        #endregion Components

        #region Strings

        public static string WithoutStartingSpaces(this string str)
        {
            string updatedStr = "";

            foreach (char charInName in str.ToCharArray())
            {
                if (charInName == ' ' && updatedStr.IsNullOrEmpty())
                    continue;
                else
                    updatedStr += charInName;
            }

            return updatedStr;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textToConvert"></param>
        /// <returns>Text with first Letter uppercase and others lowercase</returns>
        public static string NormalizeText(this string textToConvert)
        {
            string convertedText = "";
            string[] words = textToConvert.Replace("_", " ").Split(" ");
            foreach (string word in words)
            {
                int index = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    if (char.IsLetter(word[i]))
                    {
                        index = i;
                        break;
                    }
                }
                convertedText += word[..index] + char.ToUpper(word[index]) + word[(index + 1)..].ToLower() + " ";
            }
            convertedText = convertedText.TrimEnd();
            return convertedText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textToConvert"></param>
        /// <returns>Text with all letters uppercase and "_" for each space</returns>
        public static string DeNormalizeText(this string textToConvert)
        {
            return textToConvert.ToUpper().Replace(" ", "_");
        }

        #endregion Strings

        #region Copy

        /// <summary>
        /// Creates a copy of the provided object so it isn't a reference anymore (for example with scriptable objects)
        /// </summary>
        public static T Duplicate<T>(this T original) where T : class, new()
        {
            T copy = new();
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        #endregion Copy

        #region UI

        public static bool IsOutsideCanvas(this RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError($"RectTransform {rectTransform.name} is not a child of a Canvas.");
                return false;
            }

            Rect canvasRect = canvas.pixelRect;
            Rect rect = rectTransform.rect;

            Vector2 min = rectTransform.TransformPoint(rect.min);
            Vector2 max = rectTransform.TransformPoint(rect.max);

            return (min.x < canvasRect.xMin || max.x > canvasRect.xMax ||
                    min.y < canvasRect.yMin || max.y > canvasRect.yMax);
        }

        public static bool IsOutsideCanvas(this Canvas canvas, Vector2 vector)
        {
            Rect canvasRect = canvas.pixelRect;

            return (vector.x < canvasRect.xMin || vector.x > canvasRect.xMax ||
                    vector.y < canvasRect.yMin || vector.y > canvasRect.yMax);
        }

        public static Vector2 GetPivotPosition(this UIPosition pivot) => pivot switch
        {
            UIPosition.Middle => new Vector2(0.5f, 0.5f),
            UIPosition.TopLeft => new Vector2(0, 1),
            UIPosition.TopRight => new Vector2(1, 1),
            UIPosition.BottomLeft => new Vector2(0, 0),
            UIPosition.BottomRight => new Vector2(1, 0),
            UIPosition.LeftMiddle => new Vector2(0, 0.5f),
            UIPosition.TopMiddle => new Vector2(0.5f, 1),
            UIPosition.BottomMiddle => new Vector2(0.5f, 0),
            UIPosition.RightMiddle => new Vector2(1, 0.5f),
            _ => new Vector2(0.5f, 0.5f),
        };

        #endregion UI

        #region Tags

        public static Transform GetFirstObjectWithTagFromChildren(this Transform parentTransform, string tag)
        {
            foreach (Transform child in parentTransform)
            {
                if (child.CompareTag(tag))
                    return child;
            }
            return null;
        }

        #endregion Tags

        #region Vectors

        /// <returns>This vector3 with a Y value of 0</returns>
        public static Vector3 Flat(this Vector3 position) => new Vector3(position.x, 0, position.z);

        /// <returns>This vector2 with a Y value of 0</returns>
        public static Vector2 Flat(this Vector2 position) => new Vector2(position.x, 0);

        /// <returns>This vector3 with provided values changed</returns>
        public static Vector3 With(this Vector3 position, float? x = null, float? y = null, float? z = null) => new Vector3
        {
            x = x == null ? position.x : (float)x,
            y = y == null ? position.y : (float)y,
            z = z == null ? position.z : (float)z
        };

        /// <returns>This vector2 with provided values changed</returns>
        public static Vector2 With(this Vector2 position, float? x = null, float? y = null) => new Vector3
        {
            x = x == null ? position.x : (float)x,
            y = y == null ? position.y : (float)y,
        };

        #endregion Vectors
    }
}