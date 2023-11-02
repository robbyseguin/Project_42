using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private bool _localizationOngoing;
        private List<TMP_Text> _texts = new List<TMP_Text>();
        private List<string> _frTexts = new List<string>();
        private List<string> _enTexts = new List<string>();
        
        // LocaleID 0 is English, LocaleID 1 is French
        private static int LocaleID => PlayerPrefs.GetInt(LocaleKey);
        private static string LocaleKey = "LocaleID";

        protected override void Awake()
        {
            base.Awake();
            
            LocalizationSettings.SelectedLocaleChanged += SetLocalizedString;
        }

        public void AddLocalizedString(TMP_Text text, string fr, string en)
        {
            text.text = LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0] ? en : fr;
            
            _texts.Add(text);
            _frTexts.Add(fr);
            _enTexts.Add(en);
        }

        public static string GetLocalizedString(string fr, string en)
        {
            return LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0] ? en : fr;
        }
        
        public void LoadLocalization()
        {
            if(!PlayerPrefs.HasKey(LocaleKey))
                PlayerPrefs.SetInt(LocaleKey, 1);
            
            ChangeLocale(LocaleID);
        }
        
        public void ChangeLocale(int localeID)
        {
            if (_localizationOngoing)
                return;

            PlayerPrefs.SetInt(LocaleKey, localeID);
            StartCoroutine(SetLocale(localeID));
        }

        private IEnumerator SetLocale(int locale)
        {
            _localizationOngoing = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[locale];
            _localizationOngoing = false;
        }

        private void SetLocalizedString(Locale locale)
        {
            if (!_texts.Any())
                return;
            
            for (int i = 0; i < _texts.Count; ++i)
            {
                _texts[i].text = locale == LocalizationSettings.AvailableLocales.Locales[0] ? _enTexts[i] : _frTexts[i];
            }
        }
    }

    public static class LocalizationHelper
    {
        
        public static string GetLocalizedString(this string[] s)
        {
            return LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0] ? s[0] : s[1];
        }
    }
}