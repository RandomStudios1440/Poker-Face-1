using System;
using System.Collections.Generic;
using System.Linq;

public enum HandRank
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush
}

public class Hand
{
    public List<Card> Cards { get; private set; }

    public Hand()
    {
        Cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        if (Cards.Count >= 5)
            throw new InvalidOperationException("Hand cannot have more than 5 cards");
        Cards.Add(card);
    }

    public HandRank GetHandRank()
    {
        if (Cards.Count != 5)
            throw new InvalidOperationException("Hand must have exactly 5 cards to evaluate");

        var sortedCards = Cards.OrderBy(c => c.Rank).ToList();

        bool isFlush = Cards.All(c => c.Suit == Cards[0].Suit);
        bool isStraight = IsStraight(sortedCards);

        var rankGroups = Cards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ToList();
        var groupSizes = rankGroups.Select(g => g.Count()).ToList();

        // Royal Flush
        if (isFlush && isStraight && sortedCards[0].Rank == Rank.Ten)
            return HandRank.RoyalFlush;

        // Straight Flush
        if (isFlush && isStraight)
            return HandRank.StraightFlush;

        // Four of a Kind
        if (groupSizes[0] == 4)
            return HandRank.FourOfAKind;

        // Full House
        if (groupSizes[0] == 3 && groupSizes[1] == 2)
            return HandRank.FullHouse;

        // Flush
        if (isFlush)
            return HandRank.Flush;

        // Straight
        if (isStraight)
            return HandRank.Straight;

        // Three of a Kind
        if (groupSizes[0] == 3)
            return HandRank.ThreeOfAKind;

        // Two Pair
        if (groupSizes[0] == 2 && groupSizes[1] == 2)
            return HandRank.TwoPair;

        // One Pair
        if (groupSizes[0] == 2)
            return HandRank.OnePair;

        return HandRank.HighCard;
    }

    private bool IsStraight(List<Card> sortedCards)
    {
        for (int i = 1; i < sortedCards.Count; i++)
        {
            if ((int)sortedCards[i].Rank != (int)sortedCards[i - 1].Rank + 1)
                return false;
        }
        return true;
    }

    public override string ToString()
    {
        return string.Join(", ", Cards);
    }
}