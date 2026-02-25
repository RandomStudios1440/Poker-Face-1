using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public GameObject cardPrefab;

    void Start()
    {
        InitializeDeck();
        Shuffle();
    }

    void InitializeDeck()
    {
        cards.Clear();

        foreach (Card.Suit suit in System.Enum.GetValues(typeof(Card.Suit)))
        {
            foreach (Card.Rank rank in System.Enum.GetValues(typeof(Card.Rank)))
            {
                GameObject cardObj = Instantiate(cardPrefab, transform.position, Quaternion.identity);
                cardObj.transform.SetParent(transform);
                Card card = cardObj.GetComponent<Card>();
                if (card == null)
                    card = cardObj.AddComponent<Card>();
                card.Initialize(suit, rank);
                cards.Add(card);
                cardObj.SetActive(false);
            }
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        drawnCard.gameObject.SetActive(true);
        return drawnCard;
    }
}
