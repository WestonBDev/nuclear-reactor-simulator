using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteState : MonoBehaviour
{
    private bool state;
    public Sprite inactiveSprite, activeSprite;
    private Image img;

    private void Awake()
    {
        if (TryGetComponent<Image>(out Image img)){ this.img = img; }
    }

    public void ChangeState(SpriteRenderer renderer)
    {
        state = !state;
        if (!state)
        {
            renderer.sprite = inactiveSprite;
        }
        else
        {
            renderer.sprite = activeSprite;
        }
    }

    public void ChangeState(Image renderer)
    {
        state = !state;
        if (!state)
        {
            renderer.sprite = inactiveSprite;
        }
        else
        {
            renderer.sprite = activeSprite;
        }
    }

    public void Deactivate()
    {
        state = false;
        img.sprite = inactiveSprite;
    }
}
