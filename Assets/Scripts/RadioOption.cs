using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RadioOption : MonoBehaviour
{
    public RadioOptionType type;

    [Header("For Difficulty")]
    public Difficulty difficultyValue;

    [Header("For SFX")]
    public bool sfxEnabled;

    public Toggle toggle;

    private Action<RadioOption> onSelectedCallback;

    public void Initialize(Action<RadioOption> onSelected)
    {
        toggle = GetComponent<Toggle>();
        onSelectedCallback = onSelected;

        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
                onSelectedCallback?.Invoke(this);
        });
    }
}

[CustomEditor(typeof(RadioOption))]
public class RadioOptionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (RadioOption)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("toggle"));

        if (script.type == RadioOptionType.Difficulty)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("difficultyValue"));
        }
        else if (script.type == RadioOptionType.SFX)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sfxEnabled"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

