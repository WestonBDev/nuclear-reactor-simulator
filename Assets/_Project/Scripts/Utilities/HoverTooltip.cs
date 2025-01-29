using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public TooltipBox box;
    public Canvas canvas;
    public List<string> labels = new List<string>(); //index by locales indexes, eng, spa...

    [SerializeField] private float hoverTriggerTime = 1f;

    private TooltipBox tooltipInstance;
    private bool countdown;
    private float t;
    private TMP_Text tooltipTMP;

    private void Start()
    {
        tooltipInstance = Instantiate(box, gameObject.transform);
        tooltipInstance.transform.SetParent(canvas.transform, true);
        tooltipInstance.transform.SetAsLastSibling();
        UpdateLanguage();
        
    }

    private void Update()
    {
        if (countdown) 
        {
            t += Time.deltaTime;

            if( t > hoverTriggerTime)
            {
                tooltipInstance.gameObject.SetActive(true);
                countdown = false;
                t = 0;
            }
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        countdown = true;
        t = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipInstance.gameObject.SetActive(false);
        countdown = false;
    }

    public void UpdateLanguage()
    {
        int currLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        if (currLocaleIndex >= labels.Count) return;
        tooltipInstance.text.text = labels[currLocaleIndex];
    }
}
