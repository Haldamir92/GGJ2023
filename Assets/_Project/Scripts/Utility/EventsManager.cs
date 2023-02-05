using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Handles a list of timed events
/// </summary>
[AddComponentMenu("Events System/Events Manager")]
public class EventsManager : MonoBehaviour
{
    [SerializeField]
    public bool playOnStart;

    //[SerializeField]
    //public bool useScaledTime;
    /// <summary>
    /// Array of sequence of events
    /// </summary>
    [SerializeField]
    public List<TimedEventData> EventsSequence = new List<TimedEventData>();


    private void Start()
    {
        if(playOnStart)
        {
            StartEventsSequence();
        }
    }
    /// <summary>
    /// Starts the sequence of the events
    /// </summary>
    public void StartEventsSequence()
    {
        StartCoroutine(StartEvents());
    }

    /// <summary>
    /// Stops the sequence of the events
    /// </summary>
    public void StopEventsSequence()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// IEnumerator method that execute the sequence of events
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartEvents()
    {
        foreach(TimedEventData e in EventsSequence)
        {
            e.Events.Invoke();
            //if (!useScaledTime)
            //{
            //    yield return new WaitForUnscaledSeconds(e.Duration);
            //}
            //else
            //{
                yield return new WaitForSeconds(e.Duration);

            //}
        }
    }
}

/// <summary>
/// Struct for timed event
/// </summary>
[Serializable]
public struct TimedEventData
{
    /// <summary>
    /// Note of timed event
    /// </summary>
    public string Note;
    /// <summary>
    /// Variable for events
    /// </summary>
    public UnityEvent Events;
    /// <summary>
    /// Duration of the events
    /// </summary>
    public float Duration;
}
