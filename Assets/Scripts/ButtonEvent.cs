using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : EventTrigger
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Selectable thisSelectable = eventData.pointerEnter.GetComponent<Selectable>();

        if(thisSelectable != null)
        {
            if (thisSelectable.interactable)
            {
                gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_hover_01"), 0.3f);
            }
        }
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        // Handled in button OnClick methods
    }


    public override void OnSelect(BaseEventData eventData)
    {
        if(GameManager.controllerManager.xboxControllerIsConnected)
        {
            gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_hover_01"), 0.3f);
        }
    }
}
