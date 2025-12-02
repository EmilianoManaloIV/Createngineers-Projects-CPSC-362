using UnityEngine;
using DG.Tweening;

// This files only job is to tell DOTween to reserve more memory
// at the start of the game so it doesn't show a warning.
public class DOTweenInit : MonoBehaviour
{
    void Awake()
    {
        // We are setting the capacity to 500 tweens and 50 sequences, which is needed for 4 players
        DOTween.SetTweensCapacity(500, 50);
    }
}