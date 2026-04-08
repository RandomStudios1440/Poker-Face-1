using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField volumeInputField;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 100f);

        float saved = PlayerPrefs.GetFloat("musicVolume");
        volumeSlider.value = saved;
        UpdateInputField(saved);
        AudioListener.volume = saved / 100f;

        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        volumeInputField.onEndEdit.AddListener(OnInputChanged);
    }

    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
        volumeInputField.onEndEdit.RemoveListener(OnInputChanged);
    }

    private void OnSliderChanged(float value)
    {
        // round to whole number
        int rounded = Mathf.RoundToInt(value);
        volumeSlider.SetValueWithoutNotify(rounded);
        UpdateInputField(rounded);
        ApplyVolume(rounded);
    }

    private void OnInputChanged(string text)
    {
        if (float.TryParse(text, out float parsed))
        {
            // round up any decimals
            int rounded = Mathf.CeilToInt(parsed);
            rounded = Mathf.Clamp(rounded, 0, 200);
            volumeSlider.value = rounded;
            UpdateInputField(rounded);
            ApplyVolume(rounded);
        }
        else
        {
            // invalid input, revert to current slider value
            UpdateInputField(Mathf.RoundToInt(volumeSlider.value));
        }
    }

    private void ApplyVolume(int displayValue)
    {
        AudioListener.volume = displayValue / 100f;
        PlayerPrefs.SetFloat("musicVolume", displayValue);
    }

    private void UpdateInputField(float value)
    {
        volumeInputField.SetTextWithoutNotify(Mathf.RoundToInt(value).ToString());
    }
}

