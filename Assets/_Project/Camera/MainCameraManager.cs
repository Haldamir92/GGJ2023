using Cinemachine;
using UnityAtoms.BaseAtoms;
using UnityEngine;

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

    [SerializeField]
    private Vector3Variable minWorldBounds;

    [SerializeField]
    private Vector3Variable maxWorldBounds;

    bool started = false;

    public void StartGame()
    {
        started = true;
    }

    void Update()
    {
        if (!started) return;

        cameraTarget.transform.position += Vector3.down * verticalSpeed.Value * Time.deltaTime;

        virtualCamera.m_Lens.OrthographicSize = projectionSize.Evaluate(normalizedPosition.Value);

        if (normalizedPosition.Value >= 1)
        {
            enabled = false;
        }

        minWorldBounds.Value = Camera.main.ViewportToWorldPoint(Vector3.zero);
        maxWorldBounds.Value = Camera.main.ViewportToWorldPoint(Vector3.one);
    }
}
