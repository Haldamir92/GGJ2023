using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ReadyStatus : MonoBehaviour
{
    public bool IsReady => isReady;

    private bool isReady = false;

    public void ToggleReady(CallbackContext ctx)
    {
        if (ctx.started)
        {
            isReady = !isReady;
        }
    }
}
