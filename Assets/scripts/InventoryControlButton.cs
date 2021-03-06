using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryControlButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// OnPointerEnter() handles the color change of the text when the control button is hovered over.
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        this.gameObject.GetComponent<TMP_Text>().color = new Color32(214, 48, 49, 255);
    }

    /// <summary>
    /// OnPointerExit() handles the color change of the text when the control button is no longer hovered over.
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerExit(PointerEventData data)
    {
        this.gameObject.GetComponent<TMP_Text>().color = new Color32(0, 0, 0, 255);
    }
}
