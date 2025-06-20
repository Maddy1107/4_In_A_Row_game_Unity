using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public Button knob;
    private RectTransform knobRectTransform;
    public Vector2 onPosition;
    public Vector2 offPosition;
    public float lerpSpeed = 10f;

    private bool isOn;
    private Vector2 targetPosition;

    void OnEnable()
    {
        knobRectTransform = knob.GetComponent<RectTransform>();
        isOn = PlayerPrefs.GetString("ToggleSwitchState", "Easy") == "Hard";
        knobRectTransform.anchoredPosition = isOn ? onPosition : offPosition;

        targetPosition = knobRectTransform.anchoredPosition;

        knob.onClick.AddListener(Toggle);
    }
    void OnDisable()
    {
        if (knob.TryGetComponent(out Button button))
        {
            button.onClick.RemoveListener(Toggle);
        }
    }

    void Update()
    {
        knobRectTransform.anchoredPosition = Vector2.Lerp(knobRectTransform.anchoredPosition, targetPosition, Time.deltaTime * lerpSpeed);
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
