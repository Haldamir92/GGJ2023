using Coherence.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.InputSystem.InputAction;

public class NetworkedInputManager : MonoBehaviour
{
    [SerializeField]
    CoherenceInput inputSync;
    [SerializeField]
    CoherenceSync objectSync;
    [SerializeField]
    NetworkedCharacterController charController;

    Vector2 horizontalValue = Vector2.zero;

    const string HORIZONTALAXIS = "horizontal";

    public void GetHorizontalAxis(CallbackContext ctx)
    {
        if (objectSync.HasInputAuthority)
        {
            horizontalValue = ctx.ReadValue<Vector2>();
            inputSync.SetAxisState(HORIZONTALAXIS, horizontalValue);
            Debug.Log($"Sending input value: {horizontalValue}");
        }
    }

    private void Update()
    {
        if (inputSync == null)
            return;
        if (objectSync == null)
            return;

        if (objectSync.HasStateAuthority || objectSync.HasInputAuthority)
        {
            var horizontal = inputSync.GetAxisState(HORIZONTALAXIS);
            charController.SetMoveInput(horizontal);
            Debug.Log($"Receiving value: {horizontal}");
        }
    }
}
