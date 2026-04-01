using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    public string Name { get; }
    public Hand Hand { get; private set; }
    public int Chips { get; private set; }
    public bool HasFolded { get; private set; }

    public Player(string name, int startingChips = 1000)
    {
        Name = name;
        Chips = startingChips;
        Hand = new Hand();
        HasFolded = false;
    }

    public void DealCard(Card card)
    {
        Hand.AddCard(card);
    }

    public void Bet(int amount)
    {
        if (amount > Chips)
            throw new System.InvalidOperationException("Cannot bet more chips than available");
        Chips -= amount;
    }

    public void WinChips(int amount)
    {
        Chips += amount;
    }

    public void Fold()
    {
        HasFolded = true;
    }

    public void NewRound()
    {
        Hand = new Hand();
        HasFolded = false;
    }

    public override string ToString()
    {
        return $"{Name} - Chips: {Chips}";
    }
}
