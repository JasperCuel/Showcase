namespace Cue.UI
{
    public class UniquePopupInfo : PopupInfo
    {
        public string uniquePopupId;

        public UniquePopupInfo(string uniquePopupId) : base(PopupType.Unique)
        {
            this.uniquePopupId = uniquePopupId;
        }
    }
}