using Cue.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Cue.Localizer
{
    public class LocalizationManager : MonoSingleton<LocalizationManager>
    {
        [SerializeField]
        private TextAsset csvFile;
        private static TextAsset staticCsvFile;
        public static UnityEvent<Language> onLanguageChanged = new();

        public static Language currentLanguage = Language.Nederlands;

        private const string regexPattern = @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)";
        private const RegexOptions options = RegexOptions.ExplicitCapture;

        private static readonly List<string> languageList = new();
        private static readonly Dictionary<string, List<string>> languageDictionary = new();

        private void Awake()
        {
            if (csvFile == null)
            {
                DebugLogger.LogError("LANGUAGE - No .csv file provided", DebugType.Localization);
                return;
            }
            staticCsvFile = csvFile;
        }

        public void LanguageChanged(Language language)
        {
            currentLanguage = language;
            onLanguageChanged?.Invoke(language);
        }

        private static string[] SplitLine(string line)
        {
            return (from Match match in Regex.Matches(line, regexPattern, options)
                    select match.Groups[1].Value).ToArray().Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        /// <summary>
        /// Gets a list of languages available in .csv file provided
        /// </summary>
        public static List<string> GetAvailableLanguages()
        {
            if (languageList.Count <= 0)
            {
                string[] lines = staticCsvFile.text.Split("\n"[0]);
                languageList.Clear();
                string[] languages = SplitLine(lines[0]);
                languageList.AddRange(languages);
                languageList.RemoveAt(0);
            }
            return languageList;
        }

        /// <summary>Get readable text from id & language</summary>
        /// <param name="Id">Id as mentioned in .csv file [ID_example]</param>
        /// <param name="languageIndex">Language to return text in</param>
        public static string GetTextFromId(string Id, Language languageIndex)
        {
            if (staticCsvFile == null)
                return "";
            if (languageDictionary.Count <= 0)
                LoadLanguageDictionary();

            List<string> values = languageDictionary[Id];
            string translation = values[(int)languageIndex];
            return translation;
        }

        /// <summary>
        /// Caches the csv file and splits it up into a dictionary
        /// </summary>
        private static void LoadLanguageDictionary()
        {
            string[] lines = staticCsvFile.text.Split("\n"[0]);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] row = SplitLine(lines[i]);
                if (row.Length <= 2)
                    continue;

                List<string> translations = new List<string>(row);
                translations.RemoveAt(0);
                languageDictionary.Add(row[0], translations);
            }
        }
    }
}