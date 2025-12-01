using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RulesManager : MonoBehaviour
{
    public float fadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public void PanelFadeIn()

    {
        canvasGroup.alpha = 0f; // Start at invisible
        rectTransform.transform.localPosition = new Vector3 (0f, 0, 0f); // Start off-screen at 0 y
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false)
                     .SetEase(Ease.OutQuint) // Move to start pos
                     .SetUpdate(true);
                     
        canvasGroup.DOFade(1, fadeTime); // Fade in
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f; // Start at visible
        rectTransform.transform.localPosition = new Vector3 (0f, 0f, 0f); // Start on-screen
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false)
                     .SetEase(Ease.InOutQuint) 
                     .SetUpdate(true);

        canvasGroup.DOFade(0, fadeTime)
                   .SetUpdate(true)
                   .OnComplete(() => {
            // Deactivate the panel GameObject after fade completes
            if (canvasGroup != null && canvasGroup.gameObject != null)
            {
                canvasGroup.gameObject.SetActive(false);
            }
        }); // Fade out
    }
}