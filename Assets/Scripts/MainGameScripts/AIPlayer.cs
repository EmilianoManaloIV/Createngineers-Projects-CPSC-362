using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Inherits from Player and contains the logic for the AI's turn.

public class AIPlayer : Player
{
    void Awake()
    {
        isAI = true;
        hand = new List<CardController>();
    }

    // The main coroutine for the AI's turn (UC-5).
    public IEnumerator TakeTurnCoroutine()
    {
        while (GameManager.Instance.isPaused)
        {
            yield return null;
        }
        
        GameManager.Instance.uiManager.UpdateStatusText($"{playerName}'s Turn...");

        // Simulate "thinking"
        yield return new WaitForSeconds(Random.Range(0.8f, 1.5f));

        // Wait if the game is paused again
        while (GameManager.Instance.isPaused)
        {
            yield return null;
        }

        // Step 1: Find a playable card in the current hand
        CardController cardToPlay = FindPlayableCard();

        // Step 2: If no card is found, draw one
        if (cardToPlay == null)
        {
            GameManager.Instance.uiManager.UpdateStatusText($"{playerName} is drawing a card...");
            CardData drawnCardData = GameManager.Instance.deck.Draw();
            CardController drawnCardObject = DrawCard(drawnCardData);

            if (SoundManager.Instance != null) SoundManager.Instance.PlayDrawCard();
            
            drawnCardObject.SetHandActive(true);
            
            yield return new WaitForSeconds(0.7f);

            while (GameManager.Instance.isPaused)
            {
                yield return null;
            }

            // Step 3: Check *again* if a card can be played
            cardToPlay = FindPlayableCard();
        }

        // Step 4: Play the card or pass the turn
        if (cardToPlay != null)
        {
            // Check if playing this card will leave the AI with 1 card left
            if (hand.Count == 2)
            {
                // Get a random number between 0.0 and 1.0
                float unoChance = Random.Range(0f, 1f); 

                if (unoChance <= 0.80f)
                {
                    // 80% chance: The AI "remembers"
                    this.calledUno = true;
                    GameManager.Instance.uiManager.UpdateStatusText($"{playerName} calls UNO!");

                    // Play UNO sound effect
                    if (SoundManager.Instance != null) SoundManager.Instance.PlayUno();
                }
                else
                {
                    // 20% chance: The AI "forgets"
                    this.calledUno = false;
                    GameManager.Instance.uiManager.UpdateStatusText($"{playerName} is playing a card...");
                }
            }
            else
            {
                // Not the second-to-last card, just play normally
                this.calledUno = false;
                GameManager.Instance.uiManager.UpdateStatusText($"{playerName} is playing a card...");
            }

            // Wait a moment, then play the card
            yield return new WaitForSeconds(1.0f);

            while (GameManager.Instance.isPaused)
            {
                yield return null;
            }

            GameManager.Instance.PlayCard(this, cardToPlay);
        }
        else
        {
            // Step 5: No card to play, even after drawing. Pass the turn.
            GameManager.Instance.uiManager.UpdateStatusText($"{playerName} passes.");
            yield return new WaitForSeconds(1.0f);

            while (GameManager.Instance.isPaused)
            {
                yield return null;
            }

            GameManager.Instance.NextTurn(); // Manually end the turn
        }
    }

    // AI logic for finding the best card to play.
    // Based on UC-5: "Prioritizes matching number > color > wild."
    private CardController FindPlayableCard()
    {
        CardData topCard = GameManager.Instance.topCard;
        CardColor? wildColor = GameManager.Instance.CurrentWildColor;

        // 1. Prioritize Number/Type match (if not a wild)
        foreach (CardController card in hand)
        {
            if (card.cardData.cardType == topCard.cardType && 
                card.cardData.cardType < CardType.Wild && 
                !wildColor.HasValue) // Don't match type if a wild color is active
            {
                return card; // Found a type match
            }
        }

        // 2. Prioritize Color match
        // Use the current wild color if one is set, otherwise use the top card's color
        CardColor matchColor = wildColor ?? topCard.cardColor;
        foreach (CardController card in hand)
        {
            if (card.cardData.cardColor == matchColor)
            {
                return card; // Found a color match
            }
        }

        // 3. Play a Wild
        foreach (CardController card in hand)
        {
            if (card.cardData.cardType == CardType.Wild)
            {
                return card; // Found a regular Wild
            }
        }
        
        // 4. Play a WildDrawFour as a last resort
        foreach (CardController card in hand)
        {
            if (card.cardData.cardType == CardType.WildDrawFour)
            {
                return card; // Found a WildDrawFour
            }
        }

        return null; // No playable card found
    }
    
    // AI logic for choosing a color after playing a Wild.
    // It will choose the color it has the most of in its hand.
    public CardColor ChooseColor()
    {
        // Count cards of each color
        int redCount = hand.Count(c => c.cardData.cardColor == CardColor.Red);
        int greenCount = hand.Count(c => c.cardData.cardColor == CardColor.Green);
        int blueCount = hand.Count(c => c.cardData.cardColor == CardColor.Blue);
        int yellowCount = hand.Count(c => c.cardData.cardColor == CardColor.Yellow);

        // Find the max count
        int maxCount = new[] { redCount, greenCount, blueCount, yellowCount }.Max();

        // Return the color with the max count
        if (maxCount == redCount) return CardColor.Red;
        if (maxCount == greenCount) return CardColor.Green;
        if (maxCount == blueCount) return CardColor.Blue;
        return CardColor.Yellow;
    }
}