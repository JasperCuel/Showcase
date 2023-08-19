using TMPro;
using UnityEngine;

namespace SugarVita.UI.Tutorial
{
    public class TutorialPopup : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleField, descriptionField, numberField;

        public void Setup(string titleText, string descriptionText, string numberText)
        {
            titleField.text = titleText;
            descriptionField.text = descriptionText;
            numberField.text = numberText;
        }
    }
}