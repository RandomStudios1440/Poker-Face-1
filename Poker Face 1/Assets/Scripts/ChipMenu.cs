using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Chop_In : MonoBehaviour
{
    public int chips;
    int shopIndex = 0;
    [SerializeField] Material[] materials;
    [SerializeField] ButtonScript[] buttons;
    [SerializeField] Renderer PlayerRenderer;

    bool RoyalCardDeckBought, LuxuryCardDeckBought;


    void Update()
    {
        if(chips >= 120)
        {
            buttons[0].interactable = true;
        }

        if(chips >= 250)
        {
            buttons[1].interactable = true;
        }


       PlayerRenderer.material = materials[shopIndex];
    }

    public void BuyNormalCardDeck()
    {
        shopIndex = 0;
    }


    public void BuyRoyalCardDeck()
    {
        shopIndex = 1;
        if (!RoyalCardDeckBought)
        {
            chips -= 120;
            RoyalCardDeckBought = true;
        }
    }


    public void BuyLuxuryCardDeck()
    {
        shopIndex = 2;
        if (!LuxuryCardDeckBought)
        {
            chips -= 250;
            LuxuryCardDeckBought = true;
        }
    }



    void Start()
    {

    }


}