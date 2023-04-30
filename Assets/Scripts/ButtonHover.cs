using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SongManager.Instance?.PlaySFX("hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}