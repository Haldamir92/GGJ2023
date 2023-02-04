using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityAtoms.BaseAtoms;

public class MainCameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTarget;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private AnimationCurve projectionSize;

    [SerializeField]
    private FloatVariable normalizedPosition;

    [SerializeField]
    private FloatVariable verticalSpeed;

    void Update()
    {
        cameraTarget.transform.position += Vector3.down * verticalSpeed.Value * Time.deltaTime;

        virtualCamera.m_Lens.OrthographicSize = projectionSize.Evaluate(normalizedPosition.Value);

        if (normalizedPosition.Value >= 1)
        {
            enabled = false;
        }
    }
}
