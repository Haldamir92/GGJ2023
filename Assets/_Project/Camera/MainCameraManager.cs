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

    private bool goingDown = false;
    private bool goingUp = false;

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
                }
            }
            else
            {
                cameraTarget.transform.position += Vector3.up * raisingVerticalSpeed.Value * Time.deltaTime;

                virtualCamera.m_Lens.OrthographicSize = projectionSize.Evaluate(normalizedPosition.Value);

                if (raisingNormalizedPosition.Value >= 1)
                {
                    goingUp = false;
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