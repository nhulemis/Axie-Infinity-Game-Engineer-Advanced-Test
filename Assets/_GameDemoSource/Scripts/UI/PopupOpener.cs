using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupOpener : MonoBehaviour
{
    public GameObject popupPrefab;

    protected Canvas m_canvas;

    protected void Start()
    {
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public virtual void OpenPopup()
    {
        var popup = Instantiate(popupPrefab) as GameObject;
        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(m_canvas.transform, false);
        popup.GetComponent<Popup>().OpenPopup();
    }
}
