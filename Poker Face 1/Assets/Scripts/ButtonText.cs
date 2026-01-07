using UnityEngine;
using UnityEngine.UI;
public class ButtonText : MonoBehaviour
{
    public Text Textfield;

    public void SetText(string text)
    {
        Textfield.text = text;
    }
}
