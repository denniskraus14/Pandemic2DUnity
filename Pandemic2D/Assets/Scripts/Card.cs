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
    public void Start() { }
    public void Update() { }
}
