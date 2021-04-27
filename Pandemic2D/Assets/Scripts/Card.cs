using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string name;
    private bool stored;
    
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
}
