using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventsManager))]
public class EventsManagerEditor : Editor
{
    private SerializedProperty eventsSequenceProp;

    public void OnEnable()
    {
        eventsSequenceProp = serializedObject.FindProperty("EventsSequence"); // the list
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EventsManager myTarget = (EventsManager)target;
        Undo.RecordObject(myTarget, "Change EventsManager");

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();

        myTarget.playOnStart = EditorGUILayout.Toggle("Play on Start", myTarget.playOnStart);
        //myTarget.useScaledTime = EditorGUILayout.Toggle("Use scaled time", myTarget.useScaledTime);

        if (eventsSequenceProp.arraySize > 0)
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Start Events (►)"))
            {
                myTarget.StartEventsSequence();
            }
            EditorGUILayout.Space(10);
        }

        if (eventsSequenceProp != null)
        {
            for (int i = 0; i < eventsSequenceProp.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("▲"))
                {
                    if (i > 0)
                        eventsSequenceProp.MoveArrayElement(i, i - 1);
                }
                else if (GUILayout.Button("▼"))
                {
                    eventsSequenceProp.MoveArrayElement(i, i + 1);
                }
                else if (GUILayout.Button("+"))
                {
                    TimedEventData data = new TimedEventData();
                    myTarget.EventsSequence.Insert(i + 1, data);
                }
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("X"))
                {
                    eventsSequenceProp.DeleteArrayElementAtIndex(i);
                }
                else
                {
                    EditorGUILayout.EndHorizontal();

                    SerializedProperty listElement = eventsSequenceProp.GetArrayElementAtIndex(i);
                    SerializedProperty eventsProp = listElement.FindPropertyRelative("Events");
                    SerializedProperty durationProp = listElement.FindPropertyRelative("Duration");
                    SerializedProperty noteProp = listElement.FindPropertyRelative("Note");

                    noteProp.stringValue = EditorGUILayout.TextField("Event Note: ", noteProp.stringValue);

                    EditorGUIUtility.labelWidth = 0;
                    EditorGUIUtility.fieldWidth = 0;
                    EditorGUILayout.PropertyField(eventsProp);
                    durationProp.floatValue = EditorGUILayout.FloatField("Duration of Event (sec): ", durationProp.floatValue);
                    durationProp.floatValue = Mathf.Clamp(durationProp.floatValue, 0, 3600);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                }
            }
        }

        if (GUILayout.Button("Add Events"))
        {
            TimedEventData data = new TimedEventData();
            myTarget.EventsSequence.Add(data);
            //eventsSequenceProp.InsertArrayElementAtIndex(eventsSequenceProp.arraySize);
        }
        PrefabUtility.RecordPrefabInstancePropertyModifications(myTarget);

        serializedObject.ApplyModifiedProperties();
    }
}