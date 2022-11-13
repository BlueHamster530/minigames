using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    GameObject playerObject;
    Playerinput pinput;
    public int playerindex;
    private void Awake()
    {
       // playerindex = CharaterManager.instance.MaxPlayerIndex;
        playerObject = transform.GetChild(0).gameObject;
        pinput = playerObject.GetComponent<Playerinput>();
       // pinput.SetPlayerInDex(playerindex);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        pinput.OnMove(context);
    }
    public void OnPressAButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pinput.OnPressAButton();
        }
    }
    public void OnPressBButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pinput.OnPressBButton();
        }
    }
}
