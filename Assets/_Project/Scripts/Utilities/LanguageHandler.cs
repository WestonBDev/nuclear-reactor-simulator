using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

namespace JapPixel
{
    public class LanguageHandler : MonoBehaviour
    {
		//Make sure this match the locale indexes on project settings
		public enum Langs { English, Spanish};
		[SerializeField] private TMP_Dropdown m_Dropdown;
		public int currLocaleIndex = 0;

		void Start()
		{
			//Add listener for when the value of the Dropdown changes, to take action
			m_Dropdown.onValueChanged.AddListener(delegate {
				SetNewLocale(m_Dropdown);
			});
		}

		public void SetNewLocale(TMP_Dropdown change)
		{
			int language = change.value;
			switch (language)
			{
				case (int)Langs.English:
					LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
					currLocaleIndex = 0;
					break;
				case (int)Langs.Spanish:
					LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
					currLocaleIndex = 1;
					break;
				default:
					break;
			}
		}
	}
}
