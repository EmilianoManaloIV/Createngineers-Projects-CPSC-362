using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Required: Install 'Splines' from Package Manager
using DG.Tweening;          // Required: Install 'DOTween (HOTween v2)' from Asset Store or website


public class HandManager : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField, Range(0.1f, 1f)] private float tinyestHandSpread = 0.15f; // How much of the spline to use for 2 or fewer cards
    [SerializeField, Range(0.1f, 1f)] private float tinyHandSpread = 0.3f; // How much of the spline to use for 4 or fewer cards
    [SerializeField, Range(0.1f, 1f)] private float smallHandSpread = 0.5f; // How much of the spline to use for 9 or fewer cards
    [SerializeField, Range(0.1f, 1f)] private float largeHandSpread = 0.9f; // How much of the spline to use for larger hands
    [SerializeField] private SplineContainer splineContainer; // Drag your Spline object here
    
    [Header("Card Prefab (3D)")]
    [SerializeField] private GameObject cardPrefab_3D; // Your 3D Card Prefab
    [SerializeField] private Transform spawnPoint; // Where cards appear before moving to hand
    
    [Header("Owner")]
    [SerializeField] private Player owner; // Drag the Player script (on this same object) here

    void Awake()
    {
        if (owner == null)
        {
            owner = GetComponent<Player>(); // Auto-find if not set
        }
    }

    // Sets whether this hand is "active" (current player's turn) or not.
    public void SetHandActive(bool isActive)
    {
        // This will tell every card in this hand whether it's
        // part of the "active" hand or an "inactive" (dimmed) hand.
        if (owner == null || owner.hand == null) return;
        
        foreach (CardController card in owner.hand)
        {
            if (card != null)
            {
                card.SetHandActive(isActive);
            }
        }
    }
  
    // Instantiates a 3D card prefab and initializes it.
    // This is called by the Player.cs script when drawing.
    public CardController AddCardToHand(CardData cardData)
    {
        // Spawns card at spawn point
        GameObject newCardObj = Instantiate(cardPrefab_3D, spawnPoint.position, spawnPoint.rotation); 
        
        CardController cardController = newCardObj.GetComponent<CardController>();
        if (cardController != null)
        {
            cardController.Initialize(cardData, owner);
        }
        else
        {
            Debug.LogError("Card Prefab is missing a CardController script!");
        }
        return cardController;
    }

    // Spline logic now reads directly from the Player's 'hand' list.
    public void UpdateCardPositions()
    {
        int handCount = owner.hand.Count;
        if (handCount == 0) return;
        
        Spline spline = splineContainer.Spline;
        if (spline == null)
        {
            Debug.LogError("No spline found in SplineContainer!");
            return;
        }

        // --- DYNAMIC SPACING LOGIC ---
        
        float firstCardPosition;
        float cardSpacing;
        float currentSpread;

        // Determine spread based on hand size
        if (handCount <= 2)
        {
            currentSpread = tinyestHandSpread;
        }
        else if (handCount <= 4)
        {
            currentSpread = tinyHandSpread;
        }
        // case when there's less than 10 cards
        else if (handCount < 12)
        {
            currentSpread = smallHandSpread;
        }
        else
        {
            // Uses the full, wide spread for large hands
            currentSpread = largeHandSpread;
            // currentSpread = smallHandSpread; 
        }

        if (handCount == 1)
        {
            firstCardPosition = 0.5f; // 0.5 = middle of spline
            cardSpacing = 0f;
        }
        else
        {
            // This math spreads the cards across the 'handSpread' percentage
            cardSpacing = currentSpread / (float)(handCount - 1);
            firstCardPosition = (1.0f - currentSpread) / 2.0f;
        }

        // --- POSITIONING LOOP ---
        for (int i = 0; i < owner.hand.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing; // Calculate the position along the spline
            
            spline.Evaluate(p, out var splinePosition, out _, out _); // Get position on spline
            
            Quaternion rotation = splineContainer.transform.rotation; // Default rotation

            // This moves each card very slightly in front of the one before it.
            float stackingOffset = i * -0.01f; // 1 millimeter offset
            
            Vector3 localPositionWithOffset = (Vector3)splinePosition + new Vector3(0, 0, stackingOffset); // Apply offset

            // Animate the card to its new position and rotation using DOTween
            owner.hand[i].transform.DOMove(splineContainer.transform.TransformPoint(localPositionWithOffset), 0.25f);
            owner.hand[i].transform.DORotateQuaternion(rotation, 0.25f);

        }
    }
}