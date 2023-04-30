using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinMaxRange
{
    public float min;

    public float max;

    public float Size => max - min;

    public float Center => min + (Size / 2);

    public bool IsInRange(float val)
    {
        return val >= min && val <= max;
    }

    public MinMaxRange(float size)
    {
        var halfSize = size / 2f;
        var randomPosition = Random.Range(halfSize, 1f - halfSize);
        min = randomPosition - halfSize;
        max = randomPosition + halfSize;
    }
}

public class MedicalFormBox : MonoBehaviour
{
    // The rect transform that defines the outer box
    [SerializeField] RectTransform outerRect;
    [SerializeField] RectTransform highlightRect;
    [SerializeField] RectTransform underlineRect;

    [field: SerializeField] public MinMaxRange HighlightRange { get; set; }
    [field: SerializeField] public MinMaxRange UnderlineRange { get; set; }


    bool logged;

    void Update()
    {
        var totalWidth = outerRect.rect.width;
        var halfWidth = totalWidth / 2;

        var highlightWidth = Mathf.Round(HighlightRange.Size * totalWidth);

        if (!logged)
            Debug.Log($"totalWidth {totalWidth}");

        if (highlightWidth > 0)
        {
            if (!logged)
            {
                Debug.Log($"%highlight {HighlightRange.Center * totalWidth}");
                Debug.Log(outerRect.localPosition);
            }

            var position = (HighlightRange.Center * totalWidth) - halfWidth;
            highlightRect.localPosition = new Vector3(outerRect.localPosition.x + position, highlightRect.localPosition.y, 0);
            highlightRect.sizeDelta = new Vector2(highlightWidth, highlightRect.sizeDelta.y);
        }

        var underlineWidth = Mathf.Round(UnderlineRange.Size * totalWidth);

        if (underlineWidth > 0)
        {
            if (!logged)
                Debug.Log($"%highlight {UnderlineRange.Center * totalWidth}");

            var position = (UnderlineRange.Center * totalWidth) - halfWidth;
            underlineRect.localPosition = new Vector3(outerRect.localPosition.x + position, underlineRect.localPosition.y, 0);
            underlineRect.sizeDelta = new Vector2(underlineWidth, underlineRect.sizeDelta.y);
        }

        logged = true;
    }
}
