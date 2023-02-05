using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerGameStarter : MonoBehaviour
{
    [SerializeField]
    VoidEvent onPlayerReady;

    public void StartGame(CallbackContext ctx)
    {
        if (ctx.started)
        {
            onPlayerReady?.Raise();
        }
    }
}
