using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image buttonImage; // The UI Image component
    public Sprite idleSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        buttonImage.sprite = idleSprite; // Default to idle sprite
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
        AudioManager.instance.PlaySound(AudioManager.instance.hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = idleSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
        AudioManager.instance.PlaySound(AudioManager.instance.click);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }
}

