using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class CameraBoundsChecker : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3Variable minWorldBounds;

    [SerializeField]
    private Vector3Variable maxWorldBounds;

    [SerializeField]
    private BoolVariableInstancer isTargetInsideBounds;

    private void Update()
    {
        isTargetInsideBounds.Value = target.transform.position.x >= minWorldBounds.Value.x &&
            target.transform.position.x <= maxWorldBounds.Value.x &&
            target.transform.position.y >= minWorldBounds.Value.y &&
            target.transform.position.y <= maxWorldBounds.Value.y;
    }
}
