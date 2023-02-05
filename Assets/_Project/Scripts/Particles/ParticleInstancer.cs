using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInstancer : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    [SerializeField]
    private float timeToDestroy = 1;

    public void Instance()
    {
        var part = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(part, timeToDestroy);
    }
}
