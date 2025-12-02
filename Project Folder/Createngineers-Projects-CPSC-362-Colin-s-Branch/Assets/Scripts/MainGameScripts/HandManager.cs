using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> handCards = new();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) DrawCard(); // Calls DrawCard() when D key is pressed
    }
    
    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return; // Checks size of list
        GameObject g = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation); // Spawns card at spawn point
        handCards.Add(g); // Adds card to hand list
        UpdateCardPositions(); // Updates card positions along spline
    }
    private void UpdateCardPositions()
    {
        if (handCards.Count == 0) return;
        float cardSpacing = 1f / maxHandSize; // One whole spline(sliding point) length = 1f
        float firstCardPosittion = 0.5f - (handCards.Count - 1) * cardSpacing / 2; // 0.5f = middle of spline; makes sure if only one card, it is placed in the middle
        Spline spline = splineContainer.Spline; // Get the spline from the SplineContainer
        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosittion + i * cardSpacing; // Calculate the position along the spline for each card
            Vector3 splinePosition = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculates the rotation
            handCards[i].transform.DOMove(splinePosition, 0.25f); // Moves the card to the calculated posittion
            handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f); // Rotates the card to match spline orientation
        }
    }
}
