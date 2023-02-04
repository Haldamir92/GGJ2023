using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class ResourceCollectable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private IntVariable resource;
    public IntVariable Resource => resource;

    [SerializeField]
    private int amount;
    public int Amount => amount;

    [SerializeField]
    private UnityEvent onInteract;

    public void Interact()
    {
        resource.Value += amount;

        onInteract?.Invoke();
    }
}
