using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Playablehands : MonoBehaviour
{
    public enum HandRank
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        FourOfAKind = 7,
        StraightFlush = 8,
        RoyalFlush = 9
    }

    public static HandRank EvaluateHand(List<Card> hand)
    {
        if (hand.Count < 5) return HandRank.HighCard;

        bool isFlush = IsFlush(hand);
        bool isStraight = IsStraight(hand);

        if (isFlush && isStraight && hand.Min(c => c.GetValue()) == 10)
            return HandRank.RoyalFlush;

        if (isFlush && isStraight)
            return HandRank.StraightFlush;

        var rankCounts = GetRankCounts(hand);
        var counts = rankCounts.Values.OrderByDescending(v => v).ToList();

        if (counts[0] == 4)
            return HandRank.FourOfAKind;

        if (counts[0] == 3 && counts[1] == 2)
            return HandRank.FullHouse;

        if (isFlush)
            return HandRank.Flush;

        if (isStraight)
            return HandRank.Straight;

        if (counts[0] == 3)
            return HandRank.ThreeOfAKind;

        if (counts[0] == 2 && counts[1] == 2)
            return HandRank.TwoPair;

        if (counts[0] == 2)
            return HandRank.OnePair;

        return HandRank.HighCard;
    }

    static bool IsFlush(List<Card> hand)
    {
        return hand.All(c => c.suit == hand[0].suit);
    }

    static bool IsStraight(List<Card> hand)
    {
        var sortedValues = hand.Select(c => c.GetValue()).OrderBy(v => v).ToList();
        for (int i = 0; i < sortedValues.Count - 1; i++)
        {
            if (sortedValues[i + 1] - sortedValues[i] != 1)
                return false;
        }
        return true;
    }

    static Dictionary<int, int> GetRankCounts(List<Card> hand)
    {
        var counts = new Dictionary<int, int>();
        foreach (var card in hand)
        {
            int value = card.GetValue();
            if (counts.ContainsKey(value))
                counts[value]++;
            else
                counts[value] = 1;
        }
        return counts;
    }
}
