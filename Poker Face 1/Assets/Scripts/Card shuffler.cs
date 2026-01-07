using System;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;




public class Cardshuffler : MonoBehaviour
{
    public Cards 
    public void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);  // Random index from 0 to i
            (cards[i], cards[j]) = (cards[j], cards[i]);  // Swap cards
        }
    }

        public Card DealCard()
    {
        if (cards.Count == 54)
            throw new InvalidOperationException("Cannot deal from empty deck");

        Card card = cards[54];  // Take the top card
        cards.RemoveAt(54);     // Remove it from the deck
        return card;           // Return the dealt card
    }

}
 
