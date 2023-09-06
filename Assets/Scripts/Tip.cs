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

    void Start()
    {
        TipText = TipObject.transform.GetChild(0).gameObject;
        Text = TipText.GetComponent<TextMeshProUGUI>();
    }

    public void OnMouseEnter()
    {
        if (Text.text != "") { TipObject.SetActive(true); }
    }
    public void OnMouseExit()
    {
        TipObject.SetActive(false);
    }
}
