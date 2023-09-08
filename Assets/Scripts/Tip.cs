using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class Tip : MonoBehaviour
{
    public GameObject TipObject;
    public GameObject TipText;
    TextMeshProUGUI Text;
    bool isMouseOver;

    void Start()
    {
        TipText = TipObject.transform.GetChild(0).gameObject;
        Text = TipText.GetComponent<TextMeshProUGUI>();
        TipObject.SetActive(false);
    }
    public void OnMouseOver()
    {
        if (Text.text != "" && !Input.GetMouseButton(0))
        {
            TipObject.SetActive(true);
            isMouseOver = true;
        }
    }

    public void OnMouseExit()
    {
        TipObject.SetActive(false);
        isMouseOver = false;
    }

    public void OnMouseDrag()
    {
        if (isMouseOver)
        {
            TipObject.SetActive(false);
            isMouseOver = false;
        }
    }
}
