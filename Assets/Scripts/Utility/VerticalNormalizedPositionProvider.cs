using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class VerticalNormalizedPositionProvider : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform startingPoint;
    [SerializeField]
    private Transform endingPoint;

    [SerializeField]
    private FloatVariable verticalNormalizedPosition;

    private void Update()
    {
        float value = (target.position.y - startingPoint.position.y) / (endingPoint.position.y - startingPoint.position.y);
        verticalNormalizedPosition.Value = Mathf.Clamp(Mathf.Abs(value), 0, 1);
    }
}
