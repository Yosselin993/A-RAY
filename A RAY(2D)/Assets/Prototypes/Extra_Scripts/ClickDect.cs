using UnityEngine;
using UnityEngine.EventSystems; //needed to detect click event

public class ClickDect : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InfoShow.instance != null)
        {
            InfoShow.instance.HidePanel();
        }
    }
}
