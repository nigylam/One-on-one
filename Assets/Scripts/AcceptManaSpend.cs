using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptManaSpend : MonoBehaviour
{
    StatManager statManager;

    //public bool cardsChoosed = true;
    public void OnClick()
    {
        statManager.cardsChoosed = true;
    }

    void OnEnable()
    {
        statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        statManager.cardsChoosed = false;
    }

    void OnDisable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
