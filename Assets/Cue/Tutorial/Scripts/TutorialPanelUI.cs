using Cue.Core;
using Cue.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SugarVita.UI.Tutorial
{
    public class TutorialPanelUI : MonoBehaviour
    {
        [Header("Offsets")]
        [SerializeField]
        [Range(0f, 40f)]
        private float verticalMaskOffset;

        [SerializeField]
        [Range(0f, 40f)]
        private float horizontalMaskOffset = 10f;

        [SerializeField]
        private Vector2 popupCorrectionOffset = new(10f, 10f);

        [Header("References")]
        [SerializeField]
        private RectTransform maskRect;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Transform popupParent;

        [SerializeField]
        private TutorialPopup tutorialPopupPrefab;

        private TutorialTimeline currentTimeline;
        private TutorialPopup currentPopup;
        private bool isTutorialActive = false;

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextTutorialItem);
        }

        private void OnDisable() => nextButton.onClick.RemoveListener(NextTutorialItem);

        public void StartTutorial(TutorialTimeline timeline)
        {
            if (isTutorialActive)
                return;

            timeline.ValidateItems();

            isTutorialActive = true;
            currentTimeline = timeline;
            SetupTutorialItem(timeline.CurrentItem);
        }

        public void NextTutorialItem()
        {
            if (isTutorialActive)
                SetupTutorialItem(currentTimeline.NextItem);
        }

        private void SetupTutorialItem(TutorialItem item)
        {
            if (item == null) //Tutorial is finished
            {
                currentTimeline.ResetTimeline();
                PlayerPrefs.SetInt(currentTimeline.tutorialName, 1);

                isTutorialActive = false;
                UIManager.Instance.ShowPreviousPanel();
                return;
            }

            if (item.highlightRect == null || item.highlightRect.rect == null || !item.highlightRect.gameObject.activeSelf)
            {
                NextTutorialItem();
                return;
            }

            SetHighlight(item.highlightRect);
            ConfigureTutorialPopup(item);
        }

        #region Popup handling

        private void ConfigureTutorialPopup(TutorialItem item)
        {
            if (currentPopup == null)
                currentPopup = Instantiate(tutorialPopupPrefab, popupParent);
            currentPopup.Setup(item.titleText, item.descriptionText, currentTimeline.TutorialNumberString);

            RectTransform popupRect = currentPopup.GetComponent<RectTransform>();

            if (TryPlacePreferedPosition(popupRect, item.preferedPopupPosition))
                return;

            if (!item.preferedPopupPosition.Equals(UIPosition.None))
                DebugLogger.Log("Cannot place at prefered position");

            bool successfulFirstAttempt = item.tryPlaceInCornersFirst ? TryPlacePopupInCorners(popupRect) : TryPlacePopupOnSides(popupRect);
            if (successfulFirstAttempt)
                return;

            bool successfulSecondAttempt = !item.tryPlaceInCornersFirst ? TryPlacePopupInCorners(popupRect) : TryPlacePopupOnSides(popupRect);
            if (successfulSecondAttempt)
                return;

            DebugLogger.Log("Out of bounds!");
            ResetRectTransform(popupRect);
        }

        private bool TryPlacePreferedPosition(RectTransform popupRect, UIPosition preferedPosition)
        {
            if (preferedPosition.Equals(UIPosition.None))
                return false;

            Vector3[] corners = new Vector3[4];
            maskRect.GetWorldCorners(corners);

            Vector3 topMiddle = (corners[1] + corners[2]) / 2f;
            Vector3 bottomMiddle = (corners[0] + corners[3]) / 2f;
            Vector3 middleRight = (corners[2] + corners[3]) / 2f;
            Vector3 leftMiddle = (corners[0] + corners[1]) / 2f;

            //Order: TopMiddle, BottomMiddle, LeftMiddle, RightMiddle
            Vector3[] sides = new[]{
                topMiddle,
                bottomMiddle,
                leftMiddle,
                middleRight,
            };

            Vector3 middle = (topMiddle + bottomMiddle) / 2f;

            return preferedPosition switch
            {
                UIPosition.Middle => SetPopupPosition(popupRect, middle, UIPosition.Middle, false),

                UIPosition.BottomLeft => SetPopupPosition(popupRect, corners[0], UIPosition.TopRight, true),
                UIPosition.BottomRight => SetPopupPosition(popupRect, corners[3], UIPosition.TopLeft, true),
                UIPosition.TopRight => SetPopupPosition(popupRect, corners[2], UIPosition.BottomLeft, true),
                UIPosition.TopLeft => SetPopupPosition(popupRect, corners[1], UIPosition.BottomRight, true),

                UIPosition.TopMiddle => SetPopupPosition(popupRect, sides[0], UIPosition.BottomMiddle, false),
                UIPosition.BottomMiddle => SetPopupPosition(popupRect, sides[1], UIPosition.TopMiddle, false),
                UIPosition.LeftMiddle => SetPopupPosition(popupRect, sides[2], UIPosition.RightMiddle, false),
                UIPosition.RightMiddle => SetPopupPosition(popupRect, sides[3], UIPosition.LeftMiddle, false),

                UIPosition.None => false,
                _ => false,
            };
        }

        private bool TryPlacePopupInCorners(RectTransform popupRect)
        {
            Vector3[] corners = new Vector3[4];
            maskRect.GetWorldCorners(corners);

            //Order of popup location is 2, 0, 3, 1. This translates to: TR,  BL, TL, BR
            if (SetPopupPosition(popupRect, corners[2], UIPosition.BottomLeft, true))
                return true;
            if (SetPopupPosition(popupRect, corners[0], UIPosition.TopRight, true))
                return true;
            if (SetPopupPosition(popupRect, corners[1], UIPosition.BottomRight, true))
                return true;
            if (SetPopupPosition(popupRect, corners[3], UIPosition.TopLeft, true))
                return true;

            return false;
        }

        private bool TryPlacePopupOnSides(RectTransform popupRect)
        {
            Vector3[] corners = new Vector3[4];
            maskRect.GetWorldCorners(corners);

            Vector3 topMiddle = (corners[1] + corners[2]) / 2f;
            Vector3 bottomMiddle = (corners[0] + corners[3]) / 2f;
            Vector3 middleRight = (corners[2] + corners[3]) / 2f;
            Vector3 leftMiddle = (corners[0] + corners[1]) / 2f;

            //Order: TopMiddle, BottomMiddle, LeftMiddle, RightMiddle
            Vector3[] sides = new[]{
                topMiddle,
                bottomMiddle,
                leftMiddle,
                middleRight,
            };

            if (SetPopupPosition(popupRect, sides[0], UIPosition.BottomMiddle, false))
                return true;
            if (SetPopupPosition(popupRect, sides[1], UIPosition.TopMiddle, false))
                return true;
            if (SetPopupPosition(popupRect, sides[2], UIPosition.RightMiddle, false))
                return true;
            if (SetPopupPosition(popupRect, sides[3], UIPosition.LeftMiddle, false))
                return true;

            return false;
        }

        private bool SetPopupPosition(RectTransform popupRect, Vector3 placementPosition, UIPosition pivotLocation, bool isCorner)
        {
            popupRect.pivot = pivotLocation.GetPivotPosition();

            //Calculate offsets
            Vector3 popupOffset = Vector3.zero;
            if (isCorner)
            {
                popupOffset = new(popupRect.pivot.x < 0.5f ? popupCorrectionOffset.x * -1 : popupCorrectionOffset.x,
                popupRect.pivot.y < 0.5f ? popupCorrectionOffset.y * -1 : popupCorrectionOffset.y);
            }
            else
            {
                if (pivotLocation.IsAnyOf(UIPosition.TopMiddle, UIPosition.BottomMiddle))
                {
                    float pivotY = pivotLocation.Equals(UIPosition.TopMiddle) ? popupCorrectionOffset.y : popupCorrectionOffset.y * -1;
                    popupOffset = new(0, pivotY);
                }
                else if (pivotLocation.IsAnyOf(UIPosition.LeftMiddle, UIPosition.RightMiddle))
                {
                    float pivotX = pivotLocation.Equals(UIPosition.LeftMiddle) ? popupCorrectionOffset.x * -1 : popupCorrectionOffset.x;
                    popupOffset = new(pivotX, 0);
                }
            }

            popupRect.position = placementPosition + popupOffset;

            RectTransform hitbox = popupRect.GetFirstObjectWithTagFromChildren("Hitbox") as RectTransform;
            bool isOutsideCanvas = hitbox == null ? popupRect.IsOutsideCanvas() : hitbox.IsOutsideCanvas();
            if (!isOutsideCanvas)
                return true;
            return false;
        }

        #endregion Popup handling

        private void SetHighlight(RectTransform highlightedRect)
        {
            //Set custom pivot based on the highlighted rect so it doesn't interfere with the outline
            Vector2 highlightCenter = new(highlightedRect.rect.width * highlightedRect.pivot.x, highlightedRect.rect.height * highlightedRect.pivot.y);
            Vector2 pivot = new((highlightCenter.x + horizontalMaskOffset / 2f) / (highlightedRect.rect.width + horizontalMaskOffset),
                (highlightCenter.y + verticalMaskOffset / 2f) / (highlightedRect.rect.height + verticalMaskOffset));

            maskRect.pivot = pivot;
            maskRect.anchorMin = highlightedRect.anchorMin;
            maskRect.anchorMax = highlightedRect.anchorMax;

            //Set position
            Vector3 highlightedPosition = highlightedRect.position;
            Vector3 maskPosition = maskRect.position;

            Vector3 relativePosition = highlightedPosition - maskPosition;
            maskRect.position += relativePosition;

            //Set size, scale and rotation
            maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, highlightedRect.rect.width + horizontalMaskOffset);
            maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, highlightedRect.rect.height + verticalMaskOffset);

            maskRect.rotation = highlightedRect.rotation;
            maskRect.localScale = highlightedRect.localScale;

            //Force a UI rebuild on the mask
            LayoutRebuilder.ForceRebuildLayoutImmediate(maskRect);
        }

        private void ResetRectTransform(RectTransform rectTransform)
        {
            rectTransform.pivot = UIPosition.BottomLeft.GetPivotPosition();
            rectTransform.position = Vector3.zero;
        }
    }
}