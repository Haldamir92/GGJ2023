using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalizedFloatCurve : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve valueCurve;

    [SerializeField]
    private float time;
    
    [SerializeField]
    private float speed;

    [SerializeField]
    private UnityEvent<float> onValueChanged;
    [SerializeField]
    private UnityEvent onMaxValueReached;

    void Update()
    {
        time += speed * Time.deltaTime;

        if(time > 1)
        {
            time = 1;
        }

        onValueChanged?.Invoke(valueCurve.Evaluate(time));

        if (time >= 1)
        {
            onMaxValueReached?.Invoke();
        }
    }
}
