using UnityEngine;

public class FullScreenToggle : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    Camera cam;
    private bool zoomed = false;
    private float ortoSizeZoomed = 2.75f, ortoSizeDefault = 6f;
    private Vector3 ortoPosZoomed = new Vector3(-2.3f, .6f, -10f);

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            zoomed = !zoomed;
            ChangeFullScreenState();
        }
    }
    private void ChangeFullScreenState()
    {
        if (zoomed)
        {
            cam.orthographicSize = ortoSizeZoomed;
            cam.transform.position = ortoPosZoomed;
            ui.SetActive(false);
        }
        else
        {
            cam.orthographicSize = ortoSizeDefault;
            cam.transform.position = new Vector3(0,0,-10);
            ui.SetActive(true);
        }
    }
}
