using System.Collections.Generic;
using System.Linq;

public class Hand
{
    public List<Card> Cards { get; private set; } = new List<Card>();

    public void AddCard(Card card) => Cards.Add(card);

    public string GetHandRank()
    {
        if (Cards.Count < 5) return "Incomplete Hand";

        bool isFlush = Cards.All(c => c.suit == Cards[0].suit);
        var sortedValues = Cards.Select(c => c.GetValue()).OrderBy(v => v).ToList();
        bool isStraight = !sortedValues.Zip(sortedValues.Skip(1), (a, b) => b - a).Any(d => d != 1);

        if (isFlush && isStraight && sortedValues[0] == 10) return "Royal Flush";
        if (isFlush && isStraight) return "Straight Flush";

        var counts = Cards.GroupBy(c => c.GetValue())
                          .Select(g => g.Count())
                          .OrderByDescending(c => c)
                          .ToList();

        if (counts[0] == 4) return "Four of a Kind";
        if (counts[0] == 3 && counts[1] == 2) return "Full House";
        if (isFlush) return "Flush";
        if (isStraight) return "Straight";
        if (counts[0] == 3) return "Three of a Kind";
        if (counts[0] == 2 && counts[1] == 2) return "Two Pair";
        if (counts[0] == 2) return "One Pair";
        return "High Card";
    }

    public override string ToString() => string.Join(", ", Cards);
}
