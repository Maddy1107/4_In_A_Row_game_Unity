using UnityEngine;
using UnityEngine.UI;
using System;

// Represents a toggle-based radio option for difficulty or SFX
public class RadioOption : MonoBehaviour
{
    public RadioOptionType type;             // Whether this option is for difficulty or SFX

    [Header("For Difficulty")]
    public Difficulty difficultyValue;       // The difficulty value this toggle represents

    [Header("For SFX")]
    public bool sfxEnabled;                  // Whether SFX is enabled for this option

    public Toggle toggle;                    // Toggle UI component assigned in Inspector or fetched

    private Action<RadioOption> onSelectedCallback;  // Callback to notify when selected

    // Initialize the radio option and set up its event listener
    public void Initialize(Action<RadioOption> onSelected)
    {
        toggle = GetComponent<Toggle>();     // Auto-assign toggle if not manually set
        onSelectedCallback = onSelected;

        toggle.onValueChanged.AddListener(OnToggleChanged); // Register event
    }

    // Called when toggle value changes
    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
            onSelectedCallback?.Invoke(this); // Notify selection
    }

    // Clean up listeners to avoid memory leaks or duplicate callbacks
    public void Cleanup()
    {
        toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }
}

namespace UnityEditor
{
    // Custom Inspector to conditionally show fields based on type
    [CustomEditor(typeof(RadioOption))]
    public class RadioOptionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (RadioOption)target;

            serializedObject.Update();

            // Always show type and toggle reference
            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("toggle"));

            // Conditionally show only the relevant field
            if (script.type == RadioOptionType.Difficulty)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("difficultyValue"));
            }
            else if (script.type == RadioOptionType.SFX)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sfxEnabled"));
            }

            serializedObject.ApplyModifiedProperties(); // Apply changes
        }
    }
}
