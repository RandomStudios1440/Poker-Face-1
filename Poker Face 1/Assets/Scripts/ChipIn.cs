using UnityEngine;
using UnityEngine.UI;

public class ChipIn : MonoBehaviour
{
    [SerializeField] int chips;
    int shopIndex = 0;
    [SerializeField] Material[] materials;
    [SerializeField] Button[] buttons;
    [SerializeField] Renderer PlayerRenderer;


    // Update is called once per frame
    void Update()
    {
        if (chips >= 120)
        {
            buttons[0].interactable = true;
        }

        if (chips >= 250)
        {
            buttons[1].interactable = true;
        }



        PlayerRenderer.material = materials[shopIndex];
    }


    public void BuyNormalCarddeck()
    {
        shopIndex = 0;
    }
}

