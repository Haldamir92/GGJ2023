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

    [SerializeField] private GameObjectEvent onCollected;

    public void Interact()
    {
        resource.Value += amount;
        onCollected.Raise(this.gameObject);

        onInteract?.Invoke();
    }
}