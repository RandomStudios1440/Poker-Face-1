using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple card dealing script for Unity 3D.
/// Attach this to an empty GameObject in your scene.
/// Assign a card prefab and player positions in the Inspector.
/// </summary>
public class CardDealer : MonoBehaviour
{
    [Header("Card Settings")]
    public GameObject cardPrefab; // Prefab of a single card
    public int cardsPerPlayer = 5;

    [Header("Players")]
    public Transform[] playerPositions; // Positions where cards will be dealt

    [Header("Dealing Settings")]
    public float dealDelay = 0.2f; // Delay between each card deal
    public float cardSpacing = 0.3f; // Offset between cards in a player's hand

    private List<string> deck = new List<string>();

    void Start()
    {
        // Build and shuffle the deck
        BuildDeck();
        ShuffleDeck();

        // Start dealing cards
        StartCoroutine(DealCards());
    }

    /// <summary>
    /// Creates a standard 52-card deck.
    /// </summary>
    void BuildDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        deck.Clear();
        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                deck.Add(rank + " of " + suit);
            }
        }
    }

    /// <summary>
    /// Fisher–Yates shuffle algorithm.
    /// </summary>
    void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    /// <summary>
    /// Coroutine to deal cards to each player.
    /// </summary>
    IEnumerator DealCards()
    {
        for (int i = 0; i < cardsPerPlayer; i++)
        {
            foreach (Transform playerPos in playerPositions)
            {
                if (deck.Count == 0) yield break;

                // Instantiate card prefab
                GameObject card = Instantiate(cardPrefab, transform.position, Quaternion.identity);

                // Move card to player's hand position with spacing
                Vector3 targetPos = playerPos.position + (playerPos.right * (i * cardSpacing));
                StartCoroutine(MoveCard(card, targetPos, 0.2f));

                // Remove card from deck
                deck.RemoveAt(0);

                yield return new WaitForSeconds(dealDelay);
            }
        }
    }

    /// <summary>
    /// Smoothly moves a card to a target position.
    /// </summary>
    IEnumerator MoveCard(GameObject card, Vector3 target, float duration)
    {
        Vector3 start = card.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            card.transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.position = target;
    }
}