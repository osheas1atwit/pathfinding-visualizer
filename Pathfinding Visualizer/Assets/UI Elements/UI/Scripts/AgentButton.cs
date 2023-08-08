using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AgentButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;


    public void OnPointerDown(PointerEventData eventData)
    {

        _img.sprite = _pressed;


    }


    public void OnPointerUp(PointerEventData eventData)
    {

        _img.sprite = _default;


    }

    public void IWasClicked()
    {
        Debug.Log("Placing Mode: Agent");

    }

}
