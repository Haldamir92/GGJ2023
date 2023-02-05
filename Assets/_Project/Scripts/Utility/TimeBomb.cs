using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TimeBomb : MonoBehaviour
{
    [SerializeField] internal float timeToExplode;
    [SerializeField] private UnityEvent started;
    [SerializeField] private UnityEvent exploded;

    [SerializeField] private SingleSpawner spawner;
    [SerializeField] private Vector2 defect;

    void OnEnable()
    {
        StartCoroutine(WaitAndExplodeCoro(this.timeToExplode));
    }
    public IEnumerator WaitAndExplodeCoro(float timeToExplode)
    {
        started?.Invoke();
        yield return new WaitForSeconds(timeToExplode);
        if (this.timeToExplode >= 0.25f)
        {
            GameObject right=spawner.SpawnAtPosition();
            right.GetComponent<TimeBomb>().timeToExplode *= Random.Range(defect.x,defect.y);
            right.GetComponent<FakeDriver>().direction= Vector2.right*Random.Range(0.1f,1f);
            GameObject left=spawner.SpawnAtPosition();
            left.GetComponent<TimeBomb>().timeToExplode *= Random.Range(defect.x,defect.y);
            left.GetComponent<FakeDriver>().direction= Vector2.left*Random.Range(0.1f,1f);
        }
        Boom();
    }

    public void Boom()
    {
        Debug.Log("boom");
        Destroy(this.gameObject);
    }
}
