using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoMenu : MonoBehaviour
{
    [SerializeField] int coins;
    int shopIndex = 0;
    [SerializeField] Material[] materials;
    [SerializeField] Button[] buttons;
    [SerializeField] Renderer PlayerRenderer;

    void Update()
    {
        if(coins >= 100)
        {
            buttons[0].interactable = true;
        }

        if(coins >= 250)
        {
            buttons[1].interactable= true;
        }

        PlayerRenderer.material = materials[shopIndex];
    }
    public void BuyNormalCardDeck()
    {
        shopIndex = 0;
    }
}
