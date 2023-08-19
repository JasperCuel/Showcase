using System;
using UnityEngine;

namespace SugarVita.UI.Tutorial
{
    [Serializable]
    public class TutorialItem
    {
        [Header("Settings")]
        public RectTransform highlightRect;
        [Tooltip("Only provide this when the rect is instantiated in runtime")]
        public string replacementId;

        [Header("Positioning")]
        public UIPosition preferedPopupPosition = UIPosition.None;
        public bool tryPlaceInCornersFirst = true;

        [Header("Text")]
        public string titleText;
        [TextArea]
        public string descriptionText;
    }
}