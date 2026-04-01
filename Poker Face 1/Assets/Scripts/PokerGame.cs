using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokerGame
{
    private Player humanPlayer;
    private Player cpuPlayer;
    private Deck deck;
    private int pot;
    private int currentBet;
    private System.Random random;

    public PokerGame(string playerName)
    {
        humanPlayer = new Player(playerName);
        cpuPlayer = new Player("CPU");
        deck = new Deck();
        pot = 0;
        currentBet = 0;
        random = new System.Random();
    }

    public void StartNewRound()
    {
        // Reset for new round
        humanPlayer.NewRound();
        cpuPlayer.NewRound();

        deck = new Deck();
        pot = 0;
        currentBet = 0;

        // Deal 5 cards to each player
        for (int i = 0; i < 5; i++)
        {
            if (!humanPlayer.HasFolded)
                humanPlayer.DealCard(deck.DealCard());
            if (!cpuPlayer.HasFolded)
                cpuPlayer.DealCard(deck.DealCard());
        }

        Debug.Log("New round started! Cards dealt.");
        ShowPlayerHands();
    }

    public void ShowPlayerHands()
    {
        // Show human player's hand
        if (!humanPlayer.HasFolded)
        {
            Debug.Log($"{humanPlayer.Name}: {humanPlayer.Hand}");
            if (humanPlayer.Hand.Cards.Count == 5)
            {
                Debug.Log($"Hand Rank: {humanPlayer.Hand.GetHandRank()}");
            }
        }

        // Don't show CPU's cards (hidden)
        if (!cpuPlayer.HasFolded)
        {
            Debug.Log($"{cpuPlayer.Name}: [Hidden]");
        }
    }

    public void PlayerBet(int amount)
    {
        if (humanPlayer.HasFolded)
        {
            Debug.Log("You have already folded.");
            return;
        }
        if (humanPlayer.IsAllIn)
        {
            Debug.Log("You are already all-in.");
            return;
        }

        try
        {
            humanPlayer.Bet(amount);
            pot += amount;
            if (amount > currentBet)
                currentBet = amount;

            Debug.Log($"{humanPlayer.Name} bets {amount}. Pot: {pot}");

            // CPU responds to player's bet
            CPUTurn();
        }
        catch (Exception ex)
        {
            Debug.Log($"Error: {ex.Message}");
        }
    }

    public void PlayerAllIn()
    {
        if (humanPlayer.HasFolded)
        {
            Debug.Log("You have already folded.");
            return;
        }
        if (humanPlayer.IsAllIn)
        {
            Debug.Log("You are already all-in.");
            return;
        }

        int allInAmount = humanPlayer.Chips;
        pot += allInAmount;
        if (allInAmount > currentBet)
            currentBet = allInAmount;
        humanPlayer.AllIn();

        Debug.Log($"{humanPlayer.Name} goes ALL-IN with {allInAmount}! Pot: {pot}");
        CPUTurn();
    }

    // Swap up to 3 cards. indices = positions in hand (0-4) to discard.
    public void PlayerSwapCards(List<int> indices)
    {
        if (humanPlayer.HasFolded || humanPlayer.IsAllIn)
        {
            Debug.Log("Cannot swap cards right now.");
            return;
        }
        if (indices.Count > 3)
        {
            Debug.Log("You can only swap up to 3 cards.");
            return;
        }

        // Sort descending so removing by index doesn't shift positions
        indices = indices.OrderByDescending(i => i).ToList();
        foreach (int i in indices)
        {
            if (i < 0 || i >= humanPlayer.Hand.Cards.Count) continue;
            humanPlayer.Hand.Cards.RemoveAt(i);
            humanPlayer.DealCard(deck.DealCard());
        }

        Debug.Log($"Swapped {indices.Count} card(s). New hand: {humanPlayer.Hand}");
        Debug.Log($"Hand Rank: {humanPlayer.Hand.GetHandRank()}");
    }

    public void PlayerFold()
    {
        humanPlayer.Fold();
        Debug.Log($"{humanPlayer.Name} folds.");
    }

    private void CPUTurn()
    {
        if (cpuPlayer.HasFolded || cpuPlayer.IsAllIn)
            return;

        // Simple CPU AI logic
        int handStrength = EvaluateHandStrength(cpuPlayer.Hand);
        int decision = random.Next(100);

        // Decision making based on hand strength
        if (handStrength >= 7) // Strong hand (Straight or better)
        {
            int raiseAmount = currentBet + random.Next(50, 150);
            if (raiseAmount >= cpuPlayer.Chips) // Go all-in instead
            {
                pot += cpuPlayer.Chips;
                currentBet = cpuPlayer.Chips;
                cpuPlayer.AllIn();
                Debug.Log($"CPU goes ALL-IN! Pot: {pot}");
            }
            else
            {
                cpuPlayer.Bet(raiseAmount);
                pot += raiseAmount;
                currentBet = raiseAmount;
                Debug.Log($"CPU raises to {raiseAmount}. Pot: {pot}");
            }
        }
        else if (handStrength >= 4) // Medium hand (Two Pair or Three of a Kind)
        {
            if (decision < 70)
            {
                CPUCall();
            }
            else
            {
                int raiseAmount = currentBet + random.Next(20, 50);
                if (raiseAmount >= cpuPlayer.Chips)
                {
                    pot += cpuPlayer.Chips;
                    currentBet = cpuPlayer.Chips;
                    cpuPlayer.AllIn();
                    Debug.Log($"CPU goes ALL-IN! Pot: {pot}");
                }
                else
                {
                    cpuPlayer.Bet(raiseAmount);
                    pot += raiseAmount;
                    currentBet = raiseAmount;
                    Debug.Log($"CPU raises to {raiseAmount}. Pot: {pot}");
                }
            }
        }
        else if (handStrength >= 2) // Weak hand (One Pair)
        {
            if (decision < 50)
                CPUCall();
            else
            {
                cpuPlayer.Fold();
                Debug.Log("CPU folds.");
            }
        }
        else // Very weak hand (High Card)
        {
            if (decision < 20)
                CPUCall();
            else
            {
                cpuPlayer.Fold();
                Debug.Log("CPU folds.");
            }
        }
    }

    private void CPUCall()
    {
        if (currentBet >= cpuPlayer.Chips)
        {
            // Can't fully cover — go all-in
            pot += cpuPlayer.Chips;
            cpuPlayer.AllIn();
            Debug.Log($"CPU goes ALL-IN to call! Pot: {pot}");
        }
        else
        {
            cpuPlayer.Bet(currentBet);
            pot += currentBet;
            Debug.Log($"CPU calls {currentBet}. Pot: {pot}");
        }
    }

    private int EvaluateHandStrength(Hand hand)
    {
        if (hand.Cards.Count < 5)
            return 0;

        string rank = hand.GetHandRank();

        // Return strength value based on hand rank
        if (rank.Contains("Royal Flush")) return 10;
        if (rank.Contains("Straight Flush")) return 9;
        if (rank.Contains("Four of a Kind")) return 8;
        if (rank.Contains("Full House")) return 7;
        if (rank.Contains("Flush")) return 6;
        if (rank.Contains("Straight")) return 5;
        if (rank.Contains("Three of a Kind")) return 4;
        if (rank.Contains("Two Pair")) return 3;
        if (rank.Contains("One Pair")) return 2;
        return 1; // High Card
    }

    public void DetermineWinner()
    {
        var activePlayers = new List<Player>();
        if (!humanPlayer.HasFolded && humanPlayer.Hand.Cards.Count == 5)
            activePlayers.Add(humanPlayer);
        if (!cpuPlayer.HasFolded && cpuPlayer.Hand.Cards.Count == 5)
            activePlayers.Add(cpuPlayer);

        if (activePlayers.Count == 0)
        {
            Debug.Log("No active players!");
            return;
        }

        if (activePlayers.Count == 1)
        {
            var soleWinner = activePlayers[0];
            soleWinner.WinChips(pot);
            Debug.Log($"{soleWinner.Name} wins by default! Pot: {pot}");
            ShowFinalStandings();
            return;
        }

        // Both players still in - compare hands
        var winner = activePlayers.OrderByDescending(p => EvaluateHandStrength(p.Hand)).First();
        winner.WinChips(pot);

        Debug.Log($"\n--- WINNER ---");
        Debug.Log($"{winner.Name} wins with {winner.Hand.GetHandRank()}!");
        Debug.Log($"Winning hand: {winner.Hand}");

        // Reveal CPU's hand
        Debug.Log($"\nCPU's hand: {cpuPlayer.Hand}");
        Debug.Log($"CPU's rank: {cpuPlayer.Hand.GetHandRank()}");

        Debug.Log($"Pot won: {pot}");

        ShowFinalStandings();
    }

    public void ShowFinalStandings()
    {
        Debug.Log("\n--- CHIP COUNTS ---");
        Debug.Log(humanPlayer.ToString());
        Debug.Log(cpuPlayer.ToString());
    }
}
