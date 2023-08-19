namespace Cue.UI
{
    public class PopupInfo
    {
        public PopupType type;

        public PopupInfo(PopupType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return $"Popup type: {type}";
        }
    }
}