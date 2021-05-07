﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityDeck : MonoBehaviour
{
    public GameObject controller;
    private bool drawn;

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //GameObject controller = GameObject.Find("GameController");
        controller.GetComponent<Game>().draw_two();
        setDrawn(true);
    }

    public void setDrawn(bool s)
    {
        drawn = s;
    }
    public bool getDrawn() { return drawn; }
}
