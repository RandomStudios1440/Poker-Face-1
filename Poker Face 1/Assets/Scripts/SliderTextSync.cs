using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SliderTextSync : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private bool useWholeNumbers = false;
    [SerializeField] private int decimalPlaces = 2;
    [Tooltip("When enabled, displays 0-100 in the text box but maps to 0-1 on the slider")]
    [SerializeField] private bool usePercentageMode = false;

    [System.Serializable]
    public class SliderButton
    {
        public Button button;
        [Tooltip("Positive = right/increase, Negative = left/decrease")]
        public float amount = 10f;
        [Tooltip("If true, sets the slider to an exact value instead of moving it by 'amount'")]
        public bool setExactValue = false;
        [Tooltip("Only used if Set Exact Value is true")]
        public float exactValue = 0f;
    }

    [SerializeField] private List<SliderButton> sliderButtons = new List<SliderButton>();

    private bool isUpdatingSlider = false;
    private bool isUpdatingText = false;

    private void Start()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);

        if (inputField != null)
            inputField.onEndEdit.AddListener(OnTextChanged);

        foreach (var sb in sliderButtons)
        {
            if (sb.button != null)
            {
                var captured = sb;
                captured.button.onClick.AddListener(() => OnButtonPressed(captured));
            }
        }

        UpdateTextFromSlider();
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);

        if (inputField != null)
            inputField.onEndEdit.RemoveListener(OnTextChanged);

        foreach (var sb in sliderButtons)
        {
            if (sb.button != null)
                sb.button.onClick.RemoveAllListeners();
        }
    }

    private void OnSliderChanged(float value)
    {
        if (isUpdatingSlider) return;
        isUpdatingText = true;
        UpdateTextFromSlider();
        isUpdatingText = false;
    }

    private void OnTextChanged(string text)
    {
        if (isUpdatingText) return;

        if (float.TryParse(text, out float value))
        {
            isUpdatingSlider = true;
            // always set slider directly in its own range (0-100)
            slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            isUpdatingSlider = false;
        }
        else
        {
            UpdateTextFromSlider();
        }
    }

    private void OnButtonPressed(SliderButton sb)
    {
        if (slider == null) return;

        if (sb.setExactValue)
            slider.value = Mathf.Clamp(sb.exactValue, slider.minValue, slider.maxValue);
        else
            slider.value = Mathf.Clamp(slider.value + sb.amount, slider.minValue, slider.maxValue);
    }

    private void UpdateTextFromSlider()
    {
        if (inputField == null || slider == null) return;

        // always display the raw slider value as a whole number
        inputField.text = Mathf.RoundToInt(slider.value).ToString();
    }

    // Returns 0-1 when percentage mode is on, raw value otherwise
    public float GetValue()
    {
        if (slider == null) return 0f;
        return usePercentageMode ? slider.value / 100f : slider.value;
    }

    public void SetValue(float value)
    {
        if (slider == null) return;
        // if passing in a 0-1 value in percentage mode, scale it up
        slider.value = usePercentageMode
            ? Mathf.Clamp(value * 100f, slider.minValue, slider.maxValue)
            : Mathf.Clamp(value, slider.minValue, slider.maxValue);
    }
}
