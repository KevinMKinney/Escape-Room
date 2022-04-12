using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryControlButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData data)
    {
        this.gameObject.GetComponent<TMP_Text>().color = new Color32(214, 48, 49, 255);
    }

    public void OnPointerExit(PointerEventData data)
    {
        this.gameObject.GetComponent<TMP_Text>().color = new Color32(0, 0, 0, 255);
    }
}
