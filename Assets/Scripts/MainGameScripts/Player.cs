using System.Collections.Generic;
using UnityEngine;

// Base class for a player.
public class Player : MonoBehaviour
{
    [Header("Player Identity")]
    public string playerName; // Name of the player

    [Header("Player State")]
    public List<CardController> hand;
    public bool isAI = false;
    public bool calledUno = false;

    [Header("Scene Links")]
    [Tooltip("Drag the HandManager component (on this same object) here.")]
    public HandManager handManager; // Reference to the 3D hand visualizer

    void Awake()
    {
        hand = new List<CardController>();
        if (handManager == null)
        {
            handManager = GetComponent<HandManager>();
        }
    }

    // Loops through all cards in hand and updates their visual state
    // (dimmed or not) based on if they are playable.
    public void UpdatePlayableCardVisuals()
    {
        // Don't bother checking for the AI
        if (isAI) return;

        foreach (CardController card in hand)
        {
            // Ask the GameManager (the rules engine) if this card is valid
            bool isPlayable = GameManager.Instance.IsValidPlay(card.cardData);
            
            // Tell the card to update its appearance
            card.SetPlayable(isPlayable);
        }
    }

    // Adds a card to the player's logical hand and tells the
    // HandManager to create the 3D visual.
    public CardController DrawCard(CardData cardData)
    {
        // Tell HandManager to create the 3D card object
        CardController cardController = handManager.AddCardToHand(cardData);
        
        // Add to logical hand
        hand.Add(cardController);

        // Tell HandManager to update the spline layout
        handManager.UpdateCardPositions();
        
        return cardController;
    }

    // Removes a card from the logical hand, destroys its GameObject,
    // and tells the HandManager to update the spline layout.
    public void RemoveCardFromHand(CardController card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);

            // Tell HandManager to update the spline layout
            handManager.UpdateCardPositions();
        }
    }
    
    // Clears the hand at the start of a new game.
    public void ClearHand()
    {
        foreach (CardController card in hand)
        {
            Destroy(card.gameObject);
        }
        hand.Clear();
        
        // Tell HandManager to clear the spline
        if (handManager != null)
        {
            handManager.UpdateCardPositions();
        }
    }
}