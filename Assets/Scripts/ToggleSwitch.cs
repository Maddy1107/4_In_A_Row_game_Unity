using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public RectTransform knob;
    public Vector2 onPosition;
    public Vector2 offPosition;
    public float lerpSpeed = 10f;

    private bool isOn;
    private Vector2 targetPosition;

    void OnEnable()
    {
        isOn = PlayerPrefs.GetString("ToggleSwitchState", "Easy") == "Hard";
        knob.anchoredPosition = isOn ? onPosition : offPosition;

        targetPosition = knob.anchoredPosition;

        if (knob.TryGetComponent(out Button button))
        {
            button.onClick.AddListener(Toggle);
        }
    }

    void Update()
    {
        knob.anchoredPosition = Vector2.Lerp(knob.anchoredPosition, targetPosition, Time.deltaTime * lerpSpeed);
    }

    public void Toggle()
    {
        isOn = !isOn;
        targetPosition = isOn ? onPosition : offPosition;

        string difficulty = isOn ? "Hard" : "Easy";
        PlayerPrefs.SetString("ToggleSwitchState", difficulty);
        Debug.Log("Difficulty is " + difficulty);
    }
}
