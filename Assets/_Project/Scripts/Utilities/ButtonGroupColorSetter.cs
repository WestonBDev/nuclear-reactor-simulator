using UnityEngine;
using UnityEngine.UI;

public class ButtonGroupColorSetter : MonoBehaviour
{
    [SerializeField] private Color selectedColor, normalColor;
    [SerializeField] private Button[] buttons;

    private void Start()
    {
        if (buttons == null) return;
        buttons[0].image.color = selectedColor;
    }

    public void SelectButton(Button btn)
    {
        foreach (var button in buttons)
        {
            if(button == btn) button.image.color = selectedColor;
            else button.image.color = normalColor;
        }
    }
}
