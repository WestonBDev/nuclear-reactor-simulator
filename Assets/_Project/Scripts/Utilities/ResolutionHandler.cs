using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
    //Make sure this match the locale indexes on project settings
    public enum Resolutions { FullHD, HD, QHD};
    [SerializeField] private TMP_Dropdown m_Dropdown;
    [SerializeField] private Toggle fullScreen;

    void Start()
    {
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            SetResolution(m_Dropdown);
        });
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullScreen.isOn;
    }

    public void SetResolution(TMP_Dropdown change)
    {
        int res = change.value;
        switch (res)
        {
            case (int)Resolutions.FullHD:
                Screen.SetResolution(1920, 1080, fullScreen.isOn);
                break;
            case (int)Resolutions.HD:
                Screen.SetResolution(1280, 720, fullScreen.isOn);
                break;
            case (int)Resolutions.QHD:
                Screen.SetResolution(2560, 1440, fullScreen.isOn);
                break;
            default:
                break;
        }
    }
}
