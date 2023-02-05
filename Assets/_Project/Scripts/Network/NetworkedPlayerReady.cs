using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class NetworkedPlayerReady : MonoBehaviour
{
    public bool IsReady => isReady;

    private bool isReady;

    public void ToggleReady(CallbackContext ctx)
    {
        if (ctx.started)
            isReady = !isReady;
    }
}
