using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Represents the game deck and is created and managed by the GameManager.
public class Deck
{
    private List<CardData> cards;
    private List<CardData> allCardAssets; // The master list of card assets

    // Constructor: Creates a new, full deck from the list of CardData assets.
    public Deck(List<CardData> cardAssets)
    {
        allCardAssets = cardAssets;
        InitializeDeck();
    }

    // Populates the deck with the standard 108 UNO cards.
    public void InitializeDeck()
    {
        cards = new List<CardData>();

        // Find the CardData assets in the master list
        foreach (var cardAsset in allCardAssets)
        {
            if (cardAsset.cardType == CardType.Zero)
            {
                // Add one '0' card for each color
                cards.Add(cardAsset);
            }
            else if (cardAsset.cardType < CardType.Wild)
            {
                // Add two of each card number (1-9), Skip, Reverse, DrawTwo
                cards.Add(cardAsset);
                cards.Add(cardAsset);
            }
            else
            {
                // Add four of Wild and WildDrawFour
                // This assumes you only have one asset for "Wild" and one for "WildDrawFour"
                for(int i = 0; i < 4; i++)
                {
                    cards.Add(cardAsset);
                }
            }
        }
    }

    // Shuffles the deck using the Fisher-Yates algorithm.
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardData value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    // Draws a single card from the top of the deck.
    // Refills and reshuffles the deck from the discard pile if empty.
    public CardData Draw()
    {
        if (cards.Count == 0)
        {
            RefillDeck();
        }

        CardData drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }

    // Reshuffles the discard pile (minus the top card) back into the draw pile.
    private void RefillDeck()
    {
        // Get the discard pile from the GameManager
        List<CardData> discard = GameManager.Instance.discardPile;
        
        if (discard.Count <= 1)
        {
            Debug.LogWarning("Deck is empty, but discard pile has no cards to refill!");
            return;
        }

        // Get the top card (the last one in the list)
        CardData topCard = discard[discard.Count - 1];
        
        // Remove it from the list *temporarily*
        discard.RemoveAt(discard.Count - 1);
        
        // Add the *entire rest* of the discard pile to the deck
        cards.AddRange(discard);
        
        // Clear the (now empty) discard pile
        discard.Clear();
        
        // Add the top card back
        discard.Add(topCard);

        Shuffle();
    }

    // Used if a card is drawn (like a WD4 first) and needs to be put back.
    public void AddCardToDeck(CardData card)
    {
        cards.Add(card);
    }
}