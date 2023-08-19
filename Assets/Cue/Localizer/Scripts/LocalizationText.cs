using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Cue.Localizer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private List<string> replacementVariables;

        private TMP_Text textField;

        private void Awake()
        {
            textField = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            LocalizationManager.onLanguageChanged.AddListener(SetText);
            SetText(LocalizationManager.currentLanguage);
        }

        private void OnDisable()
        {
            LocalizationManager.onLanguageChanged.RemoveListener(SetText);
        }

        public void SetText(string overrideKey, ICollection<string> optionalVariables = null)
        {
            SetText(overrideKey, LocalizationManager.currentLanguage, optionalVariables);
        }

        public void SetText(string overwriteKey, Language language, ICollection<string> optionalVariables = null)
        {
            this.replacementVariables.Clear();
            if (optionalVariables != null && optionalVariables.Count > 0)
                this.replacementVariables = optionalVariables.ToList();

            key = overwriteKey;
            SetText(language);
        }

        public void SetText(Language language)
        {
            string text = LocalizationManager.GetTextFromId(key, language);
            if (replacementVariables != null && replacementVariables.Count > 0)
            {
                for (int i = 0; i < replacementVariables.Count; i++)
                {
                    text = text.Replace($"[{i}]", replacementVariables[i]);
                }
            }

            textField.SetText(text);
        }
    }
}