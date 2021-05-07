using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityDeck : MonoBehaviour
{
    public GameObject controller;
    private bool drawn;

    public void OnMouseUp()
    {
        if (getDrawn() == false)
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            //GameObject controller = GameObject.Find("GameController");
            controller.GetComponent<Game>().draw_two();
            setDrawn(true);
            controller.GetComponent<Game>().setAction(4); //don't allow them to make any moves in between drawing and infecting
        }
    }

    public void setDrawn(bool s)
    {
        drawn = s;
    }
    public bool getDrawn() { return drawn; }
}
