/* Use this script to maintain a fixed aspect ratio.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FixedAspectLetterbox : MonoBehaviour
{
    [SerializeField] float targetAspect = 16f / 9f;

    void Update()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scale = windowAspect / targetAspect;

        if (scale < 1f)
        {
            // Pillarbox (left/right bars)
            float w = scale;
            float x = (1f - w) * 0.5f;
            Camera.main.rect = new Rect(x, 0f, w, 1f);
        }
        else
        {
            // Letterbox (top/bottom bars)
            float h = 1f / scale;
            float y = (1f - h) * 0.5f;
            Camera.main.rect = new Rect(0f, y, 1f, h);
        }
    }
}
*/