using System.Collections;
using System.Collections.Generic;
using System.Linq; // Used for shuffling
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

// This is the main controller for the entire game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Components")]
    [Tooltip("A list of all CardData assets. Populate this in the Inspector.")]
    public List<CardData> allCardData; // All possible cards in the game
    public Deck deck;
    public UIManager uiManager;
    public PauseManager pauseManager;
    
    [Tooltip("The 3D Material for the back of cards (Used for Draw Pile & AI Hand)")]
    public Material cardBackMaterial;

    [Header("Player Management")]
    public Player humanPlayer;
    
    [Header("AI Players (Clockwise)")]
    public AIPlayer aiPlayerLeft;
    public AIPlayer aiPlayerTop;
    public AIPlayer aiPlayerRight;
    
    private List<Player> players;
    
    private int currentPlayerIndex = 0;
    private bool isGameReversed = false;

    private Player previousPlayer = null; // To un-highlight the last player
    private bool hasDrawnCardThisTurn = false; // To prevent multiple draws
    private bool isProcessingMove = false;

    [Header("Game State")]
    public List<CardData> discardPile;
    public CardData topCard; 
    
    private CardColor? currentWildColor = null; 

    public CardColor? CurrentWildColor
    {
        get { return currentWildColor; }
    }

    private bool gameIsActive = false;

    [Header("Pause State")]
    public bool isPaused = false;
    
    void Awake()
    {
        // Set up the singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        int botCount = 3; // Default to 3 bots
        
        // Find the GameSettings object that came from the other scene
        GameSettings settings = FindObjectOfType<GameSettings>();
        if (settings != null)
        {
            botCount = settings.botCount;
            // Can destroy it now
            Destroy(settings.gameObject);
        }

        deck = new Deck(allCardData); // Create a new deck
        deck.Shuffle();

        discardPile = new List<CardData>();
        
        players = new List<Player>();
        players.Add(humanPlayer);     // Index 0
        
        // Deactivate all AI players by default
        if (aiPlayerLeft != null) aiPlayerLeft.gameObject.SetActive(false);
        if (aiPlayerTop != null) aiPlayerTop.gameObject.SetActive(false);
        if (aiPlayerRight != null) aiPlayerRight.gameObject.SetActive(false);

        // Add and activate only the AI players we need
        if (botCount == 1)
        {
            // 1 Bot: Place them at the top
            if (aiPlayerTop != null) {
                players.Add(aiPlayerTop);
                aiPlayerTop.gameObject.SetActive(true);
            }
        }
        else if (botCount == 2)
        {
            // 2 Bots: Place them Left and Right
            if (aiPlayerLeft != null) {
                players.Add(aiPlayerLeft);
                aiPlayerLeft.gameObject.SetActive(true);
            }
            if (aiPlayerRight != null) {
                players.Add(aiPlayerRight);
                aiPlayerRight.gameObject.SetActive(true);
            }
        }
        else // 3 Bots
        {
            // 3 Bots: Use all three (Clockwise)
            if (aiPlayerLeft != null) {
                players.Add(aiPlayerLeft);
                aiPlayerLeft.gameObject.SetActive(true);
            }
            if (aiPlayerTop != null) {
                players.Add(aiPlayerTop);
                aiPlayerTop.gameObject.SetActive(true);
            }
            if (aiPlayerRight != null) {
                players.Add(aiPlayerRight);
                aiPlayerRight.gameObject.SetActive(true);
            }
        }
        // Clear the previous player from the last game
        if (previousPlayer != null)
        {
            previousPlayer.handManager.SetHandActive(false);
            previousPlayer = null;
        }
        
        // Clear all hands
        foreach (Player player in players)
        {
            player.ClearHand();
        }

        // Deal 7 cards to each player
        for (int i = 0; i < 7; i++)
        {
            foreach (Player player in players)
            {
                player.DrawCard(deck.Draw());
            }
        }

        // Place the first card on the discard pile (as per UC-1)
        topCard = deck.Draw();
        // Rule: The first card cannot be a Wild Draw Four.
        while (topCard.cardType == CardType.WildDrawFour)
        {
            deck.AddCardToDeck(topCard); // Put it back
            deck.Shuffle(); // Reshuffle
            topCard = deck.Draw(); // Draw a new one
        }
        
        discardPile.Add(topCard);
        uiManager.UpdateDiscardPile(topCard);
        
        // Handle if the first card is special
        HandleSpecialFirstCard(topCard);

        gameIsActive = true;

        if (SoundManager.Instance != null) SoundManager.Instance.PlayGameStart();
        
        // If the first card wasn't a Skip or Reverse, start the first player's turn
        if (gameIsActive) 
        {
            // Reset "drawn" status for the first turn
            hasDrawnCardThisTurn = false;
            uiManager.ShowDrawButton(true);
            uiManager.ShowPassButton(false);
            
            StartTurn();
        }
    }

    private void CheckWinCondition(Player player)
    {
        if (player.hand.Count == 0)
        {
            gameIsActive = false;

            string winnerMessage;
            if (player.isAI)
            {
                // Find out *which* AI won
                string winnerName = "AI";
                // Check which specific AI slot the winner was in
                if (player == aiPlayerLeft) winnerName = "AI Left";
                else if (player == aiPlayerTop) winnerName = "AI Top";
                else if (player == aiPlayerRight) winnerName = "AI Right";
                winnerMessage = $"{winnerName} wins!";
            }
            else
            {
                winnerMessage = "You Win!";
            }

            uiManager.ShowWinnerScreen(winnerMessage);

            if (SoundManager.Instance != null) SoundManager.Instance.PlayWin();
        }
    }


    // --- Turn Management Methods (UC-4, UC-5) ---

    // Starts the turn for the currently active player.
    private void StartTurn()
    {
        if (!gameIsActive) return;

        // Un-highlight the previous player
        if (previousPlayer != null)
        {
            previousPlayer.handManager.SetHandActive(false);
        }
        
        Player currentPlayer = players[currentPlayerIndex];

        // Highlight the new current player
        currentPlayer.handManager.SetHandActive(true);
        
        // Store this player for the next turn
        previousPlayer = currentPlayer;

        isProcessingMove = false;
        
        // At the start of ANY new turn (AI or Human),
        // we should hide the Pass button from the previous turn.
        uiManager.ShowPassButton(false);

        // If it's the AI's turn, trigger its logic (UC-5)
        if (currentPlayer.isAI)
        {
            uiManager.ShowUnoButton(false); // Hide Uno button during AI turns
            StartCoroutine((currentPlayer as AIPlayer).TakeTurnCoroutine());
        }
        else
        {
            // If it's the human's turn, update status.
            uiManager.UpdateStatusText("Your Turn");
            
            // Reset draw status
            hasDrawnCardThisTurn = false;
            uiManager.ShowDrawButton(true);
            uiManager.ShowPassButton(false); // This line is still correct
            
            // Tell the player to update their hand visuals (dim/un-dim cards)
            humanPlayer.UpdatePlayableCardVisuals();
            
            UpdateUnoButtonVisibility();
        }
    }

    // Advances the current player index, based on game direction.
    private void AdvancePlayerIndex()
    {
        //
        if (isGameReversed)
        {
            currentPlayerIndex--;
            if (currentPlayerIndex < 0)
                currentPlayerIndex = players.Count - 1;
        }
        else
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
                currentPlayerIndex = 0;
        }
    }

    // Checks if it's the given player's turn.    
    public bool IsPlayerTurn(Player player)
    {
        return players[currentPlayerIndex] == player && gameIsActive;
    }

    public void NextTurn()
    {
        AdvancePlayerIndex();
        StartTurn();
    }


    // --- Card Playing Methods (UC-2, UC-3) ---

    public void PlayCard(Player player, CardController cardToPlay)
    {
        if (isPaused) return;

        if (isProcessingMove) return;
        
        // Ensure it's this player's turn
        if (player != players[currentPlayerIndex] || !gameIsActive)
        {
            return;
        }

        if (IsValidPlay(cardToPlay.cardData))
        {
            isProcessingMove = true;

            if (SoundManager.Instance != null) SoundManager.Instance.PlayPlayCard();

            // 1. Remove from hand (doesn't destroy it)
            player.RemoveCardFromHand(cardToPlay);
            
            // 2. Update logic
            discardPile.Add(cardToPlay.cardData);
            topCard = cardToPlay.cardData;

            // 3. Update 2D UI (instantly)
            uiManager.UpdateDiscardPile(topCard);

            // 4. Reset wild color
            currentWildColor = null; 

            // Tell the UI to hide the indicator make it null
            if (uiManager != null) uiManager.UpdateWildColorIndicator(null);
            // Check if player forgot to call Uno
            if (player.hand.Count == 1 && !player.calledUno)
            {
                // Penalty! Draw 2 cards.
                uiManager.UpdateStatusText($"{ (player.isAI ? "AI" : "You") } forgot to call UNO! Draw 2.");
                player.DrawCard(deck.Draw());
                player.DrawCard(deck.Draw());
            }
            player.calledUno = false; // Reset Uno status for next turn
            
            // Get the world position of the 2D discard pile
            Vector3 discardTargetPos = uiManager.discardPileImage.transform.position;
            
            cardToPlay.AnimateToDiscard(discardTargetPos, () => {
                // This code runs *after* the animation is complete
                // Check if this was the player's last card
                if(player.hand.Count == 0)
                {
                    CheckWinCondition(player);
                    return; // Game Over
                }
                else
                {
                    HandleCardEffect(topCard);
                }
            });
        }
        else
        {
            if (!player.isAI)
            {
                uiManager.UpdateStatusText("Invalid move. Try again.");
            }
        }
    }

    public bool IsValidPlay(CardData card)
    {
        // Wild cards can always be played
        if (card.cardType == CardType.Wild || card.cardType == CardType.WildDrawFour)
        {
            return true;
        }

        // Check for a Wild card color match
        if (currentWildColor.HasValue)
        {
            return card.cardColor == currentWildColor.Value;
        }

        // Standard match (Color or Type)
        return card.cardColor == topCard.cardColor || card.cardType == topCard.cardType;
    }

    // Called by the Player when they click the draw pile. UC-3.
    public void OnDrawPileClicked()
    {
        if (isPaused) return;
        
        Player currentPlayer = players[currentPlayerIndex];

        // Don't let AI click the draw pile
        if (currentPlayer.isAI || !gameIsActive) return;
        
        // Don't let player draw if it's not their turn
        if (currentPlayer != players[currentPlayerIndex]) return;
        
        // Don't let player draw more than once
        if (hasDrawnCardThisTurn) return;
        hasDrawnCardThisTurn = true;

        if (SoundManager.Instance != null) SoundManager.Instance.PlayDrawCard();

        uiManager.UpdateStatusText("You drew a card.");

        CardData drawnCardData = deck.Draw();
        CardController drawnCardObject = currentPlayer.DrawCard(drawnCardData); // This now updates the spline

        drawnCardObject.SetHandActive(true);

        humanPlayer.UpdatePlayableCardVisuals();

        UpdateUnoButtonVisibility();
        
        // Show the "Pass" button and hide the "Draw" button
        uiManager.ShowDrawButton(false);
        uiManager.ShowPassButton(true);
    }
    
    // Called by the UIManager when the player clicks the "Pass Turn" button.
    public void OnPassTurnClicked()
    {
        if (isPaused) return;

        if (isProcessingMove) return;
        
        Player currentPlayer = players[currentPlayerIndex];

        // Don't let AI pass or pass if it's not your turn
        if (currentPlayer.isAI || !gameIsActive) return;
        if (currentPlayer != players[currentPlayerIndex]) return;

        // Only allow passing *after* drawing a card
        if (hasDrawnCardThisTurn)
        {
            isProcessingMove = true;
            uiManager.UpdateStatusText("You passed your turn.");
            StartCoroutine(EndTurnAfterDelay(0.5f));
        }
    }

    public void OnUnoButtonClicked()
    {
        if (isPaused) return;
        
        Player currentPlayer = players[currentPlayerIndex];
        
        // Can only call on your turn
        if (currentPlayer.isAI || currentPlayer != players[currentPlayerIndex]) return;

        // Can only call if you have 2 cards (and are about to play one)
        if (currentPlayer.hand.Count == 2)
        {
            currentPlayer.calledUno = true;
            uiManager.UpdateStatusText("You called UNO!");

            if (SoundManager.Instance != null) SoundManager.Instance.PlayUno();

            uiManager.ShowUnoButton(false);
        }
    }

    // Handles the special effect of the card that was just played.
    private void HandleCardEffect(CardData card)
    {
        switch (card.cardType)
        {
            case CardType.Skip:
                uiManager.UpdateStatusText("Turn Skipped!");
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySkip();
                StartCoroutine(EndTurnAndSkipAfterDelay(1.0f));
                break;
            
            case CardType.Reverse:
                uiManager.UpdateStatusText("Order Reversed!");
                if (SoundManager.Instance != null) SoundManager.Instance.PlayReverse();
                isGameReversed = !isGameReversed;
                StartCoroutine(EndTurnAfterDelay(0.5f));
                break;
            
            case CardType.DrawTwo:
                uiManager.UpdateStatusText("Draw 2!");
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDrawTwo();
                Player nextPlayer = GetNextPlayer();
                nextPlayer.DrawCard(deck.Draw());
                nextPlayer.DrawCard(deck.Draw());

                StartCoroutine(EndTurnAndSkipAfterDelay(1.0f));
                break;
            
            case CardType.Wild:
                PromptForColorChoice();
                // Note: Turn does NOT end here. It ends in SetWildColor.
                break;
            
            case CardType.WildDrawFour:
                uiManager.UpdateStatusText("Wild Draw 4!");
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDrawFour();
                Player victim = GetNextPlayer();
                victim.DrawCard(deck.Draw());
                victim.DrawCard(deck.Draw());
                victim.DrawCard(deck.Draw());
                victim.DrawCard(deck.Draw());

                PromptForColorChoice();
                // Note: Turn does NOT end here. It ends in SetWildColor.
                break;
            
            default:
                // Regular number card
                StartCoroutine(EndTurnAfterDelay(0.5f));
                break;
        }
    }
    
    // Handles special card rules if the *first* card is not a number.
    private void HandleSpecialFirstCard(CardData card)
    {
        // This function is slightly different from HandleCardEffect
        switch (card.cardType)
        {
            case CardType.Skip:
                uiManager.UpdateStatusText("First card is a Skip. Player 2's turn.");
                currentPlayerIndex = 1; // Skip the first player
                break;
            
            case CardType.Reverse:
                uiManager.UpdateStatusText("First card is Reverse. Order reversed!");
                isGameReversed = !isGameReversed;
                // Player 0 still goes first, but the *next* player is now AI Right (index 3)
                break;
            
            case CardType.DrawTwo:
                uiManager.UpdateStatusText("First card is Draw Two. Player 1 draws 2.");
                players[0].DrawCard(deck.Draw());
                players[0].DrawCard(deck.Draw());
                currentPlayerIndex = 1; // Skip the first player
                break;
            
            case CardType.Wild:
                uiManager.UpdateStatusText("First card is Wild.");
                // Player 1 (the first player) gets to choose the color.
                PromptForColorChoice();
                gameIsActive = false; // Pause the game until color is chosen
                break;
            
            // WildDrawFour is handled by the StartGame loop, so no need to check here.
            default:
                // Regular number card, do nothing.
                break;
        }
    }

    // Asks the current player (AI or Human) to choose a color.
    private void PromptForColorChoice()
    {
        Player currentPlayer = players[currentPlayerIndex];
        if (currentPlayer.isAI)
        {
            // AI chooses a color
            CardColor aiChoice = (currentPlayer as AIPlayer).ChooseColor();
            SetWildColor(aiChoice);
        }
        else
        {
            // Human chooses a color
            uiManager.ShowColorPicker(true);
        }
    }
    
    // Called by the UIManager or AI when a wild color is chosen.
    public void SetWildColor(CardColor color)
    {
        currentWildColor = color;
        uiManager.UpdateWildColorIndicator(color);
        uiManager.ShowColorPicker(false);
        
        // If the game was paused (for a first-card-Wild), unpause it.
        if (!gameIsActive)
        {
            gameIsActive = true;
            StartTurn();
            return;
        }

        // Check if the card was a WildDrawFour, which also includes a skip
        if (topCard.cardType == CardType.WildDrawFour)
        {
            StartCoroutine(EndTurnAndSkipAfterDelay(1.0f));
        }
        else
        {
            // Just a regular Wild, go to next turn
            StartCoroutine(EndTurnAfterDelay(0.5f));
        }
    }

    // Helper function to find out who the next player is,
    // without actually advancing the turn. Used for Draw 2/4.
    private Player GetNextPlayer()
    {
        int nextIndex = currentPlayerIndex;
        if (isGameReversed)
        {
            nextIndex--;
            if (nextIndex < 0)
                nextIndex = players.Count - 1;
        }
        else
        {
            nextIndex++;
            if (nextIndex >= players.Count)
                nextIndex = 0;
        }
        return players[nextIndex];
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;
        
        if (uiManager != null)
        {
            uiManager.ShowPausePanel(isPaused);
        }

        if (isPaused)
        {
            // Use to have animations for pause panel
            /*if (pauseManager != null)
            {
                pauseManager.PanelFadeIn();
            }*/
            
            DOTween.PauseAll();
        }
        else
        {
            // Use to have animations for pause panel
            /*if (pauseManager != null)
            {
                pauseManager.PanelFadeOut();
            }*/
            
            DOTween.PlayAll();
        }
    }
    
    // Helper coroutine to add a small delay before ending a turn.
    IEnumerator EndTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextTurn();
    }
    
    // Helper coroutine to end the turn and skip the next player after a delay.
    IEnumerator EndTurnAndSkipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AdvancePlayerIndex();
        NextTurn();
    }

    public void SetAllHandsVisibility(bool isVisible)
    {
        if (players == null) return;

        foreach (Player player in players)
        {
            if (player == null || player.hand == null) continue;
            
            foreach (CardController card in player.hand)
            {
                if (card != null)
                {
                    // This calls the new function in CardController
                    card.SetRenderer(isVisible);
                }
            }
        }
    }

    // Updates the visibility of the Uno button based on the human player's hand.
    private void UpdateUnoButtonVisibility()
    {
        bool showUnoButton = false;

        if (IsPlayerTurn(humanPlayer) && humanPlayer.hand.Count == 2)
        {
            // Check if the player has at least one playable card
            bool hasPlayableCard = false;
            foreach (CardController card in humanPlayer.hand)
            {
                if (IsValidPlay(card.cardData))
                {
                    hasPlayableCard = true;
                    break; // Break when found playable card
                }
            }

            // Only show the button if they have 2 cards AND one is playable
            if (hasPlayableCard)
            {
                showUnoButton = true;
            }
        }
            uiManager.ShowUnoButton(showUnoButton);

    }
}
