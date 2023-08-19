using Cue.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SugarVita.UI.Tutorial
{
    [Serializable]
    public class TutorialTimeline
    {
        public string tutorialName;

        [SerializeField]
        private List<TutorialItem> tutorialItems;

        private List<TutorialItem> validItems;
        private List<TutorialItem> replacementRects;

        private int currentItemIndex = 0;

        public bool ContainsTutorials
        { get { return tutorialItems != null && tutorialItems.Count > 0; } }

        public string TutorialNumberString
        { get { return $"{currentItemIndex + 1}/{validItems.Count}"; } }

        public TutorialItem CurrentItem
        { get { return validItems?[currentItemIndex]; } }

        public TutorialItem NextItem
        {
            get
            {
                TutorialItem item = null;
                if (validItems != null && ++currentItemIndex < validItems.Count)
                    item = validItems[currentItemIndex];
                return item;
            }
        }

        public void ResetTimeline()
        {
            currentItemIndex = 0;
        }

        public void AddReplacement(RectTransform rect, string replacementId)
        {
            if (rect == null)
                return;

            TutorialItem item = new() { highlightRect = rect, replacementId = replacementId };

            replacementRects ??= new();
            if (!replacementRects.Contains(item))
                replacementRects.Add(item);
        }

        public void RemoveReplacement(string replacementId)
        {
            if (replacementRects == null || replacementRects.Count < 1)
                return;

            if (replacementRects.Any(x => x.replacementId.Equals(replacementId)))
            {
                TutorialItem itemToRemove = replacementRects.First(x => x.replacementId.Equals(replacementId));
                int index = tutorialItems.IndexOf(tutorialItems.FirstOrDefault(x => x.replacementId.Equals(replacementId)));
                if (index >= 0)
                    tutorialItems[index].highlightRect = null;
                replacementRects.Remove(itemToRemove);
            }
        }

        public void ValidateItems()
        {
            List<TutorialItem> itemsToRemove = new List<TutorialItem>();

            foreach (TutorialItem item in tutorialItems)
            {
                if (item.highlightRect != null && !item.highlightRect.gameObject.activeInHierarchy)
                    itemsToRemove.Add(item);
                else if (item.highlightRect == null || item.highlightRect.rect == null)
                {
                    if (replacementRects != null && replacementRects.Count > 0 && !item.replacementId.IsNullOrEmpty())
                    {
                        TutorialItem x = replacementRects.SingleOrDefault(x => x.replacementId.Equals(item.replacementId));
                        item.highlightRect = x?.highlightRect;
                    }
                    if (item.highlightRect == null)
                        itemsToRemove.Add(item);
                }
            }

            validItems = tutorialItems.Except(itemsToRemove).ToList();
        }

        public bool ContainsItem(string id)
        {
            List<TutorialItem> item = tutorialItems.Where(x => x.replacementId.Equals(id)).ToList();
            List<TutorialItem> replacementItem = replacementRects.Where(x => x.replacementId.Equals(id)).ToList();

            if (item != null && replacementItem != null && replacementItem.Count > 0)
                return true;

            return false;
        }
    }
}