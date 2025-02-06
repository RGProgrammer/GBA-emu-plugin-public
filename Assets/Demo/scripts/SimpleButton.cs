using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SimpleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image target;
    RectTransform rectTransform;
    [SerializeField]
    private Sprite pressed;
    [SerializeField]
    private Sprite released;
    [SerializeField]
    GBA_Component.KEYS key;

    public delegate void OnPressHandler(GBA_Component.KEYS keyPressed);
    public delegate void OnReleaseHandler(GBA_Component.KEYS keyReleased);
    public event OnPressHandler OnPress;
    public event OnReleaseHandler OnRelease;

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Image>();
        rectTransform = target.GetComponent<RectTransform>();
        target.sprite = released;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        target.sprite = pressed;
        OnPress?.Invoke(key);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        target.sprite = released;
        OnRelease?.Invoke(key);
    }
}

