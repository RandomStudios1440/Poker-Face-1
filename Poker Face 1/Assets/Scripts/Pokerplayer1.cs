using System.Collections.Generic;
using UnityEngine;

public class Pokerplayer : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public int chips = 1000;
    public int currentBet = 0;
    public bool isHuman = true;
    public bool hasFolded = false;

    public Transform handPosition;

    public void AddCardToHand(Card card)
    {
        hand.Add(card);
        PositionCard(card, hand.Count - 1);
    }

    void PositionCard(Card card, int index)
    {
        if (handPosition != null)
        {
            Vector3 offset = new Vector3(index * 0.15f, 0, 0);
            card.transform.position = handPosition.position + offset;
            card.transform.SetParent(handPosition);
        }
    }

    public void ClearHand()
    {
        foreach (Card card in hand)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        hand.Clear();
        currentBet = 0;
        hasFolded = false;
    }

    public void PlaceBet(int amount)
    {
        if (amount <= chips)
        {
            chips -= amount;
            currentBet += amount;
        }
    }

    public void WinPot(int potAmount)
    {
        chips += potAmount;
    }
}
