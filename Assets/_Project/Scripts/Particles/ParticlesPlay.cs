using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPlay : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    float interval;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayParticle());
    }

    IEnumerator PlayParticle()
    {
        while (true)
        {
            particle.Play();
            yield return new WaitForSeconds(interval);
        }
    }
}
