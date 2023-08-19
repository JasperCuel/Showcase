using System;

namespace Cue.UI
{
    public class ConfirmationPopupInfo : InformationPopupInfo
    {
        public string confirmButtonText;
        public Action onConfirm;

        public ConfirmationPopupInfo(PopupType type, string titleText, string descriptionText, string closeButtonText, Action onClose, string confirmButtonText, Action onConfirm)
            : base(type, titleText, descriptionText, closeButtonText, onClose)
        {
            this.confirmButtonText = confirmButtonText;
            this.onConfirm = onConfirm;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Confirm button text: {confirmButtonText}";
        }
    }
}