using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "NewLogEventData", menuName = "NuclearSim/LogEventData")]
public class LogEventData : ScriptableObject
{
    public LogType logType;
    public LogEventType eventType;
    [SerializeField] private List<string> labels = new List<string>(); //index by locales indexes, eng, spa...
    public string timeEmited;

    public string localeLabel()
    {
        int currLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        return labels[currLocaleIndex];
    }
}
