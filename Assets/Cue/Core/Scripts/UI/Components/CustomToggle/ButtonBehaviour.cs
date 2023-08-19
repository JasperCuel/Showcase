using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cue.Core
{
    public abstract class ButtonBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
    {
        public abstract void OnClick();

        private bool selectToggle = false;
        private bool childOfScrollRect = false;

        private void Start()
        {
            childOfScrollRect = this.GetFirstComponentInAllParents<ScrollRect>() != null;
        }

        private void OnEnable()
        {
            childOfScrollRect = this.GetFirstComponentInAllParents<ScrollRect>() != null;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!selectToggle)
                return;
            OnClick();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (childOfScrollRect)
                selectToggle = false;
        }

        public void OnPointerDown(PointerEventData eventData) => selectToggle = true;
    }
}