
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))] // Needs a collider to be clicked
public class CardController : MonoBehaviour
{
    public CardData cardData;
    private Player owner;
    private Renderer cardRenderer; // To set the card face texture

    private Vector3 originalScale;
    private const float HOVER_SCALE_MULTIPLIER = 1.2f;

    // These colors tint the material of the cards
    private static readonly Color PLAYABLE_COLOR = Color.white; // Full brightness
    private static readonly Color DIMMED_COLOR = new Color(0.55f, 0.55f, 0.55f, 1.0f); // A dark gray
    private static readonly Color ACTIVE_BUT_UNPLAYABLE_COLOR = new Color(0.7f, 0.7f, 0.7f, 1.0f);
    
    // Tracks if the card is currently playable
    private bool isCardPlayable = false;
    private bool isHandActive = false;

    void Awake()
    {
        if (cardRenderer == null)
        {
            cardRenderer = GetComponent<Renderer>();
        }
        
        if (cardRenderer == null)
        {
            Debug.LogError("CardController could not find its Renderer component!", this.gameObject);
        }
    }
    
    
    /// Initializes the card with its data and owner.
    public void Initialize(CardData data, Player player)
    {
        cardData = data;
        owner = player;

        // Get the renderer (e.g., on a child Quad) to set its texture
        cardRenderer = GetComponentInChildren<Renderer>();

        if (owner.isAI)
        {
            // If the owner is the AI, apply the universal card back material
            if (GameManager.Instance.cardBackMaterial != null)
            {
                // Makes sure that the Draw Pile doesn't glow
                cardRenderer.material = new Material(GameManager.Instance.cardBackMaterial);
            }
        }
        else if (cardData.cardImage != null)
        {
            // If it's the human player, show the card face
            // We set the texture, not the whole material, to keep the "Unlit" shader
            if (cardData.cardImage.texture != null)
            {
                cardRenderer.material.mainTexture = cardData.cardImage.texture;
            }
        }

        originalScale = transform.localScale;
        
        // Default to a dimmed (non-playable) state on creation
        SetPlayable(false);
    }
    
    public void SetRenderer(bool isVisible)
    {
        if (cardRenderer != null)
        {
            cardRenderer.enabled = isVisible;
        }
    }

    // Sets whether this card is in an active hand or not.
    public void SetHandActive(bool isActive)
    {
        this.isHandActive = isActive;
        UpdateColor();
    }

    // Sets whether this card is currently playable.
    public void SetPlayable(bool playable)
    {
        this.isCardPlayable = playable;

        UpdateColor(); 
    }

    // Updates the card's color based on its playability and hand activity.
    private void UpdateColor()
    {
        if (cardRenderer == null) return;

        if (!isHandActive)
        {
            // CASE 1: The hand is NOT active.
            // Result: Set to the darkest dim color.
            cardRenderer.material.color = DIMMED_COLOR;
        }
        else
        {
            // CASE 2: The hand IS active.
            if (owner.isAI)
            {
                // AI cards are always full bright when it's their turn.
                cardRenderer.material.color = PLAYABLE_COLOR;
            }
            else
            {
                // It's the human player's turn.
                if (isCardPlayable)
                {
                    // The card is playable.
                    cardRenderer.material.color = PLAYABLE_COLOR;
                }
                else
                {
                    // The card is NOT playable, but it's your turn.
                    cardRenderer.material.color = ACTIVE_BUT_UNPLAYABLE_COLOR;
                }
            }
        }
    }

    // Animates the card moving to the discard pile.
    public void AnimateToDiscard(Vector3 targetPosition, System.Action onComplete)
    {
        // Disable collider so it can't be clicked mid-flight
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Create a DOTween sequence
        Sequence seq = DOTween.Sequence();
        
        // Add animations to the sequence
        float duration = 0.4f;
        seq.Append(transform.DOMove(targetPosition, duration).SetEase(Ease.InQuad));
        
        // When the animation is done
        seq.OnComplete(() => {
            onComplete?.Invoke(); // Call the game logic (like HandleCardEffect)
            Destroy(gameObject);  // Now, destroy the card object
        });
    }

    // Called when the player clicks this card's Collider.
    private void OnMouseDown()
    {
        // Ignore clicks on cards if the game is paused
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        
        // Must be your turn AND the card must be playable
        if (owner.isAI || !isCardPlayable || !GameManager.Instance.IsPlayerTurn(owner)) return;
        
        // Tell the GameManager this card was played
        GameManager.Instance.PlayCard(owner, this);
    }

    // --- 3D Hover Animations ---

    private void OnMouseEnter()
    {
        //Must be your turn AND the card must be playable
        if (owner.isAI || !isCardPlayable || !GameManager.Instance.IsPlayerTurn(owner)) return;
        
        // Simple hover animation: scale up
        transform.localScale = originalScale * HOVER_SCALE_MULTIPLIER;
    }

    private void OnMouseExit()
    {
        // No check needed, always reset scale
        if (owner.isAI) return;
        transform.localScale = originalScale;
    }
}