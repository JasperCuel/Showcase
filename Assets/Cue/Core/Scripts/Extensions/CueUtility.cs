using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.Core
{
    public static class CueUtility
    {
        /// <summary>
        /// Forces a UI refresh for root transform and all children
        /// </summary>
        public static void RefreshLayoutGroupsImmediateAndRecursive(RectTransform root)
        {
            foreach (RectTransform rect in root.GetComponentsInChildren<RectTransform>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(root);
        }

        /// <summary>
        /// Invoke an action with a specific delay (in seconds)
        /// </summary>
        /// <param name="action">Action to invoke</param>
        /// <param name="delay">Delay (in seconds)</param>
        public static void InvokeDelayed(this MonoBehaviour mb, Action action, float delay)
        {
            mb.StartCoroutine(EInvokeDelayed(action, delay));
        }

        /// <summary>
        /// Invoke an action on the next frame
        /// </summary>
        /// <param name="action">Action to invoke</param>
        public static void InvokeNextFrame(this MonoBehaviour mb, Action action)
        {
            mb.StartCoroutine(EInvokeNextFrame(action));
        }

        #region Private

        private static IEnumerator EInvokeNextFrame(Action action)
        {
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }

        private static IEnumerator EInvokeDelayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        #endregion Private
    }
}