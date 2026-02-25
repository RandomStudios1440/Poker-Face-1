using UnityEngine;

public class Cards : MonoBehaviour
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Eleven, Queen, King, Ace }

    public Suit suit;
    public Rank rank;
    public bool isFaceUp = false;

    public void Initialize(Suit cardSuit, Rank cardRank)
    {
        suit = cardSuit;
        rank = cardRank;
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
        // Rotate card to show face or back
        transform.rotation = isFaceUp ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }

    public int GetValue()
    {
        return (int)rank;
    }

    public override string ToString()
    {
        return $"{rank} of {suit}";
    }
}
