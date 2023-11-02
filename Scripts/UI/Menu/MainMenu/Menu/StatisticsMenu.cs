using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.Menu.MainMenu
{
    public class StatisticsMenu : Menu
    {
        [SerializeField] private TMP_Text _values;
        [SerializeField] private LocalizedString _localizedString;
        
        protected void Awake()
        {
            IList<object> statsValue = new object[StatisticsManager.Instance._sessionStatistics.Length];

            for (int i = 0; i < StatisticsManager.Instance._sessionStatistics.Length; i++)
            {
                statsValue[i] = StatisticsManager.Instance._sessionStatistics[i];
            }

            _localizedString.Arguments = statsValue;
            _localizedString.StringChanged += UpdateStats;
        }

        private void UpdateStats(string value)
        {
            _values.text = value;
        }
    }
}
