using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    private static string XBOX_360_STRING_WIN7 = "Controller (XBOX 360 For Windows)";
    private static string XBOX_ONE_STRING_WIN7 = "Controller (XBOX One For Windows)";

    private static string XBOX_360_STRING_WIN10 = "Controller (Xbox 360 For Windows)";
    private static string XBOX_ONE_STRING_WIN10 = "Controller (Xbox One For Windows)";

    public bool xboxControllerIsConnected = false;

    private bool _controllerStatusChanged = false;

    public GameObject currentPanel;

    private void Start()
    {
        ControllerConnectionListener();
    }

    void Update()
    {
        ControllerConnectionListener();

        if(_controllerStatusChanged)
        {
            StartCoroutine(UpdateStatus(!xboxControllerIsConnected));
        }
    }

    private void ControllerConnectionListener()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                if (Input.GetJoystickNames()[i].ToString().Equals(XBOX_360_STRING_WIN7) ||
                    Input.GetJoystickNames()[i].ToString().Equals(XBOX_ONE_STRING_WIN7) ||
                    Input.GetJoystickNames()[i].ToString().Equals(XBOX_360_STRING_WIN10) ||
                    Input.GetJoystickNames()[i].ToString().Equals(XBOX_ONE_STRING_WIN10))
                {
                    if(!xboxControllerIsConnected)
                    {
                        _controllerStatusChanged = true;
                    }
                }
                else
                {
                    if (xboxControllerIsConnected)
                    {
                        _controllerStatusChanged = true;
                    }
                }
            }
        }
    }

    IEnumerator UpdateStatus(bool status)
    {
        _controllerStatusChanged = false;
        yield return null;
        xboxControllerIsConnected = status;

        if(Cursor.visible)
        {
            GameManager.cursorManager.HideCursor();
            currentPanel.GetComponentsInChildren<Selectable>()[0].Select();
        }
        else
        {
            if(currentPanel != null)
            {
                if (currentPanel.activeInHierarchy)
                {
                    GameManager.cursorManager.ShowCursor();
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }            
        }
    }
}
