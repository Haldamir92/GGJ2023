using Cinemachine;
using System.Collections;
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
    private FloatVariable raisingNormalizedPosition;

    [SerializeField]
    private FloatVariable verticalSpeed;

    [SerializeField]
    private FloatVariable raisingVerticalSpeed;

    [SerializeField]
    private Vector3Variable minWorldBounds;

    [SerializeField]
    private Vector3Variable maxWorldBounds;

    [SerializeField]
    private FloatVariable treeLength;

    [SerializeField]
    private float treeLengthSpeed;

    [SerializeField]
    private float raisingOrthographicSizeSpeed;

    [SerializeField]
    private Transform endingPosition;

    [SerializeField]
    private Transform raisingEndingPosition;

    private bool goingDown = false;
    private bool goingUp = false;

    private void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = projectionSize.Evaluate(0);
    }

    public void StartGame()
    {
        goingDown = true;

        StartCoroutine(IncreaseTreeLength());
    }

    public void EndGame()
    {
        goingDown = false;
        goingUp = true;
    }

    private void Update()
    {
        if (goingDown || goingUp)
        {
            if (goingDown)
            {
                cameraTarget.transform.position += Vector3.down * verticalSpeed.Value * Time.deltaTime;

                virtualCamera.m_Lens.OrthographicSize = projectionSize.Evaluate(normalizedPosition.Value);

                if (normalizedPosition.Value >= 1)
                {
                    goingDown = false;

                    cameraTarget.transform.position = new Vector3(cameraTarget.transform.position.x, endingPosition.position.y, cameraTarget.transform.position.z);
                }
            }
            else
            {
                cameraTarget.transform.position += Vector3.up * raisingVerticalSpeed.Value * Time.deltaTime;

                float orthographicSizeToReach = projectionSize.Evaluate(1);
                if (virtualCamera.m_Lens.OrthographicSize < orthographicSizeToReach)
                {
                    virtualCamera.m_Lens.OrthographicSize += raisingOrthographicSizeSpeed * Time.deltaTime;

                    if (virtualCamera.m_Lens.OrthographicSize > orthographicSizeToReach)
                    {
                        virtualCamera.m_Lens.OrthographicSize = orthographicSizeToReach;
                    }
                }

                if (raisingNormalizedPosition.Value >= 1)
                {
                    goingUp = false;

                    cameraTarget.transform.position = new Vector3(cameraTarget.transform.position.x, raisingEndingPosition.position.y, cameraTarget.transform.position.z);
                }
            }

            minWorldBounds.Value = Camera.main.ViewportToWorldPoint(Vector3.zero);
            maxWorldBounds.Value = Camera.main.ViewportToWorldPoint(Vector3.one);
        }
    }

    private IEnumerator IncreaseTreeLength()
    {
        while (goingDown)
        {
            treeLength.Value += 1;

            yield return new WaitForSeconds(treeLengthSpeed);
        }
    }
}