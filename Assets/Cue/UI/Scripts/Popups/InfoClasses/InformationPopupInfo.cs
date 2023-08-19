using System;

namespace Cue.UI
{
    public class InformationPopupInfo : PopupInfo
    {
        public string titleText;
        public string descriptionText;
        public string closeButtonText;
        public Action onClose;

        public InformationPopupInfo(PopupType type, string titleText, string descriptionText, string closeButtonText, Action onClose) : base(type)
        {
            this.titleText = titleText;
            this.descriptionText = descriptionText;
            this.closeButtonText = closeButtonText;
            this.onClose = onClose;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Title: {titleText} - Description: {descriptionText} - Close button text: {closeButtonText}";
        }
    }
}