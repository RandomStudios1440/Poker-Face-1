using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerGame : MonoBehaviour
{
    public static PokerGame Instance;

    public CardDeck deck;
    public PokerPlayer humanPlayer;
    public PokerPlayer cpuPlayer;

    public List<Card> communityCards = new List<Card>();
    public Transform communityCardsPosition;

    public int pot = 0;
    public int smallBlind = 10;
    public int bigBlind = 20;

    public Text potText;
    public Text playerChipsText;
    public Text cpuChipsText;
    public Text gameStatusText;

    public Button foldButton;
    public Button callButton;
    public Button raiseButton;
    public Button checkButton;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Button replayButton;
    public Button menuButton;

    enum GameState { WaitingToStart, Dealing, PreFlop, Flop, Turn, River, Showdown, RoundEnd, GameOver }
    GameState currentState = GameState.WaitingToStart;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (replayButton) replayButton.onClick.AddListener(OnReplay);
        if (menuButton) menuButton.onClick.AddListener(OnMenu);
        StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        if (CheckGameOver()) yield break;

        currentState = GameState.Dealing;
        UpdateUI("New round starting...");

        yield return new WaitForSeconds(1f);

        ClearTable();
        pot = 0;

        // Post blinds
        humanPlayer.PlaceBet(smallBlind);
        cpuPlayer.PlaceBet(bigBlind);
        pot = smallBlind + bigBlind;

        UpdateUI("Dealing cards...");

        // Deal 2 cards to each player
        for (int i = 0; i < 2; i++)
        {
            humanPlayer.AddCardToHand(deck.DrawCard());
            yield return new WaitForSeconds(0.3f);
            cpuPlayer.AddCardToHand(deck.DrawCard());
            yield return new WaitForSeconds(0.3f);
        }

        // Flip human player cards face up
        foreach (Card card in humanPlayer.hand)
        {
            card.Flip();
        }

        UpdateUI("Your turn - Check, Call, or Raise");
        currentState = GameState.PreFlop;
        EnablePlayerActions(true);
    }

    public void PlayerFold()
    {
        humanPlayer.hasFolded = true;
        EnablePlayerActions(false);
        StartCoroutine(EndRound(cpuPlayer));
    }

    public void PlayerCall()
    {
        int callAmount = cpuPlayer.currentBet - humanPlayer.currentBet;
        humanPlayer.PlaceBet(callAmount);
        pot += callAmount;
        EnablePlayerActions(false);
        StartCoroutine(CPUTurn());
    }

    public void PlayerRaise()
    {
        int raiseAmount = bigBlind * 2;
        humanPlayer.PlaceBet(raiseAmount);
        pot += raiseAmount;
        EnablePlayerActions(false);
        StartCoroutine(CPUTurn());
    }

    IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1f);

        UpdateUI("CPU is thinking...");

        // Simple CPU AI
        int decision = Random.Range(0, 100);

        if (decision < 20)
        {
            UpdateUI("CPU folds");
            cpuPlayer.hasFolded = true;
            yield return new WaitForSeconds(1f);
            StartCoroutine(EndRound(humanPlayer));
        }
        else if (decision < 70)
        {
            int callAmount = humanPlayer.currentBet - cpuPlayer.currentBet;
            cpuPlayer.PlaceBet(callAmount);
            pot += callAmount;
            UpdateUI("CPU calls");
            yield return new WaitForSeconds(1f);
            StartCoroutine(DealCommunityCards());
        }
        else
        {
            int raiseAmount = bigBlind * 2;
            cpuPlayer.PlaceBet(raiseAmount);
            pot += raiseAmount;
            UpdateUI("CPU raises!");
            yield return new WaitForSeconds(1f);
            EnablePlayerActions(true);
        }
    }

    IEnumerator DealCommunityCards()
    {
        if (currentState == GameState.PreFlop)
        {
            UpdateUI("Dealing the Flop...");
            currentState = GameState.Flop;

            for (int i = 0; i < 3; i++)
            {
                Card card = deck.DrawCard();
                communityCards.Add(card);
                PositionCommunityCard(card, communityCards.Count - 1);
                card.Flip();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (currentState == GameState.Flop)
        {
            UpdateUI("Dealing the Turn...");
            currentState = GameState.Turn;

            Card card = deck.DrawCard();
            communityCards.Add(card);
            PositionCommunityCard(card, communityCards.Count - 1);
            card.Flip();
            yield return new WaitForSeconds(0.5f);
        }
        else if (currentState == GameState.Turn)
        {
            UpdateUI("Dealing the River...");
            currentState = GameState.River;

            Card card = deck.DrawCard();
            communityCards.Add(card);
            PositionCommunityCard(card, communityCards.Count - 1);
            card.Flip();
            yield return new WaitForSeconds(0.5f);
        }

        if (currentState == GameState.River)
        {
            StartCoroutine(Showdown());
        }
        else
        {
            EnablePlayerActions(true);
        }
    }

    IEnumerator Showdown()
    {
        currentState = GameState.Showdown;
        UpdateUI("Showdown!");

        yield return new WaitForSeconds(1f);

        // Flip CPU cards
        foreach (Card card in cpuPlayer.hand)
        {
            card.Flip();
        }

        yield return new WaitForSeconds(1f);

        // Evaluate hands
        List<Card> humanFullHand = new List<Card>(humanPlayer.hand);
        humanFullHand.AddRange(communityCards);

        List<Card> cpuFullHand = new List<Card>(cpuPlayer.hand);
        cpuFullHand.AddRange(communityCards);

        var humanRank = PokerHandEvaluator.EvaluateHand(humanFullHand);
        var cpuRank = PokerHandEvaluator.EvaluateHand(cpuFullHand);

        PokerPlayer winner = null;

        if (humanRank > cpuRank)
        {
            winner = humanPlayer;
            UpdateUI($"You win with {humanRank}!");
        }
        else if (cpuRank > humanRank)
        {
            winner = cpuPlayer;
            UpdateUI($"CPU wins with {cpuRank}!");
        }
        else
        {
            UpdateUI("It's a tie!");
            humanPlayer.WinPot(pot / 2);
            cpuPlayer.WinPot(pot / 2);
            pot = 0;
        }

        if (winner != null)
        {
            winner.WinPot(pot);
            pot = 0;
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine(StartNewRound());
    }

    IEnumerator EndRound(PokerPlayer winner)
    {
        currentState = GameState.RoundEnd;

        if (winner == humanPlayer)
            UpdateUI("You win the pot!");
        else
            UpdateUI("CPU wins the pot!");

        winner.WinPot(pot);
        pot = 0;

        yield return new WaitForSeconds(2f);

        StartCoroutine(StartNewRound());
    }

    void ClearTable()
    {
        humanPlayer.ClearHand();
        cpuPlayer.ClearHand();

        foreach (Card card in communityCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        communityCards.Clear();

        if (deck != null)
        {
            deck.Shuffle();
        }
    }

    void PositionCommunityCard(Card card, int index)
    {
        if (communityCardsPosition != null)
        {
            Vector3 offset = new Vector3(index * 0.2f, 0, 0);
            card.transform.position = communityCardsPosition.position + offset;
            card.transform.SetParent(communityCardsPosition);
        }
    }

    void EnablePlayerActions(bool enable)
    {
        if (foldButton) foldButton.interactable = enable;
        if (callButton) callButton.interactable = enable;
        if (raiseButton) raiseButton.interactable = enable;
        if (checkButton) checkButton.interactable = enable;
    }

    void UpdateUI(string status)
    {
        if (gameStatusText) gameStatusText.text = status;
        if (potText) potText.text = "Pot: $" + pot;
        if (playerChipsText) playerChipsText.text = "$" + humanPlayer.chips;
        if (cpuChipsText) cpuChipsText.text = "$" + cpuPlayer.chips;
    }

    bool CheckGameOver()
    {
        if (humanPlayer.chips <= 0)
        {
            ShowGameOver("CPU Wins!");
            return true;
        }
        if (cpuPlayer.chips <= 0)
        {
            ShowGameOver("Player Wins!");
            return true;
        }
        return false;
    }

    void ShowGameOver(string message)
    {
        currentState = GameState.GameOver;
        EnablePlayerActions(false);
        if (gameOverText) gameOverText.text = message;
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    void OnReplay()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        humanPlayer.chips = 10000;
        cpuPlayer.chips = 10000;
        StartCoroutine(StartNewRound());
    }

    void OnMenu()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(0);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
