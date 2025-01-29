using UnityEngine;

public class UpdateDynamicContentLeng : MonoBehaviour
{
    public void UpdateTooltips()
    {
        HoverTooltip[] tooltips = FindObjectsByType<HoverTooltip>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (HoverTooltip tooltip in tooltips) 
        {
            tooltip.UpdateLanguage();
        }
    }
}
