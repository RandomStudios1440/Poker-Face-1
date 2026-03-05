using UnityEngine;
using TMPro;

public class RandomTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private string[] textOptions;
    [SerializeField] private bool changeOnStart = true;
    [SerializeField] private bool changeOnEnable = false;
    [SerializeField] private float autoChangeInterval = 0f; // 0 = disabled

    private float timer = 0f;

    private void Start()
    {
        if (textDisplay == null)
        {
            textDisplay = GetComponent<TextMeshProUGUI>();
        }

        if (changeOnStart)
        {
            DisplayRandomText();
        }
    }

    private void OnEnable()
    {
        if (changeOnEnable && textDisplay != null)
        {
            DisplayRandomText();
        }
    }

    private void Update()
    {
        if (autoChangeInterval > 0f)
        {
            timer += Time.deltaTime;
            if (timer >= autoChangeInterval)
            {
                DisplayRandomText();
                timer = 0f;
            }
        }
    }

    public void DisplayRandomText()
    {
        if (textOptions.Length == 0 || textDisplay == null) return;

        int randomIndex = Random.Range(0, textOptions.Length);
        textDisplay.text = textOptions[randomIndex];
    }

    public void SetText(string text)
    {
        if (textDisplay != null)
        {
            textDisplay.text = text;
        }
    }
}
