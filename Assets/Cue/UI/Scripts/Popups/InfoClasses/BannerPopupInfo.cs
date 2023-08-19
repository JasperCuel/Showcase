using System;
using UnityEngine;

namespace Cue.UI
{
    public class BannerPopupInfo : InformationPopupInfo
    {
        public Sprite bannerSprite;

        public BannerPopupInfo(PopupType type, string titleText, string descriptionText, Sprite bannerSprite, string closeButtonText, Action onClose) : base(type,titleText, descriptionText, closeButtonText, onClose)
        {
            this.bannerSprite = bannerSprite;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}