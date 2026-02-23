using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    private List<Card> cards;
    private Random random;

    public Deck()
    {
        random = new Random();
        InitializeDeck();
        Shuffle();
    }

    private void InitializeDeck()
    {
        cards = new List<Card>();

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    public Card DealCard()
    {
        if (cards.Count == 0)
            throw new InvalidOperationException("Cannot deal from empty deck");

        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public int CardsRemaining => cards.Count;
}