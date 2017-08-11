using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControlManager : MonoBehaviour
{
    private FirstPersonController _fpsController;

	void Awake ()
    {
        _fpsController = GameManager.fpsController;
    }

    /**
     * Disables both movement and look
     **/ 
    public void DisablePlayerControl()
    {
        _fpsController.canLook = false;
        _fpsController.canMove = false;
    }

    /**
     * Enablesa both movement and look
     **/
    public void EnablelayerControl()
    {
        _fpsController.canLook = true;
        _fpsController.canMove = true;
    }

    /**
     * Disables movement
     **/
    public void DisablePlayerMovement()
    {
        _fpsController.canMove = false;
    }

    /**
     * Enables movement
     **/
    public void EnablePlayerMovement()
    {
        _fpsController.canMove = true;
    }
}
