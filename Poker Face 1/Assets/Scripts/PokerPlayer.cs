using System.Collections.Generic;
using UnityEngine;

public class PokerPlayer : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public int chips = 1000;
    public int currentBet = 0;
    public bool isHuman = true;
    public bool hasFolded = false;
    
    public Transform handPosition;

    [Header("Card Layout")]
    public Vector3 cardScale = Vector3.one;
    public Vector3 cardRotation = Vector3.zero;
    public float cardSpacing = 0.15f;

    public void AddCardToHand(Card card)
    {
        if (card == null)
        {
            Debug.LogError("AddCardToHand: received a null card. Check that CardDeck prefabs are assigned.");
            return;
        }
        hand.Add(card);
        PositionCard(card, hand.Count - 1);
    }
    
    void PositionCard(Card card, int index)
    {
        if (handPosition != null)
        {
            Vector3 offset = new Vector3(index * cardSpacing, 0, 0);
            card.transform.position = handPosition.position + offset;
            card.transform.rotation = Quaternion.Euler(cardRotation);
            card.transform.localScale = cardScale;
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
