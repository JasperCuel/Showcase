using UnityEngine.UI;

namespace Cue.UI
{
    public class UniquePopup : Popup
    {
        public string uniquePopupId;
        public Button closeButton;

        public UniquePopup()
        {
            type = PopupType.Unique;
        }

        public override string ToString()
        {
            return $"Popup type: {type} - Unique popup id: {uniquePopupId}";
        }
    }
}