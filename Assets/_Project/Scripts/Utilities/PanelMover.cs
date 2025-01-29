using UnityEngine;

public class PanelMover : MonoBehaviour
{
    [SerializeField] private float hiddenYPos = 900f;
    public void HidePanel(GameObject panel)
    {
        panel.transform.localPosition = new Vector3(0, hiddenYPos, 0);   
    }

    public void ShowPanel(GameObject panel)
    {
        panel.transform.localPosition = new Vector3(0, 0, 0);
    }

}
