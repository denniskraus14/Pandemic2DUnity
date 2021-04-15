using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string name;
    private bool stored;
    private GameObject card_go;

    public Card(string n, bool s = false)
    {
        name = n;
        stored = s;
    }
    public string getName()
    {
        return name;
    }
    public bool getStored()
    {
        return stored;
    }
    public void setStored(bool s)
    {
        stored = s;
    }

    public GameObject getCard_Go()
    {
        return card_go;
    }
    public void setCard_Go(GameObject go)
    {
        card_go = go;
    }

    public void Start() { }
    public void Update() { }
}
