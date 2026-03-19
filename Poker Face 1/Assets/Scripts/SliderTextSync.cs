using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextSync : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private bool useWholeNumbers = false;
    [SerializeField] private int decimalPlaces = 2;

    private bool isUpdatingSlider = false;
    private bool isUpdatingText = false;

    private void Start()
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderChanged);
        }

        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnTextChanged);
        }

        UpdateTextFromSlider();
    }

    private void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderChanged);
        }

        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnTextChanged);
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
            slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            isUpdatingSlider = false;
        }
        else
        {
            UpdateTextFromSlider();
        }
    }

    private void UpdateTextFromSlider()
    {
        if (inputField == null || slider == null) return;

        if (useWholeNumbers)
        {
            inputField.text = Mathf.RoundToInt(slider.value).ToString();
        }
        else
        {
            inputField.text = slider.value.ToString("F" + decimalPlaces);
        }
    }

    public float GetValue()
    {
        return slider != null ? slider.value : 0f;
    }

    public void SetValue(float value)
    {
        if (slider != null)
        {
            slider.value = value;
        }
    }
}
