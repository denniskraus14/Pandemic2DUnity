using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDeck : MonoBehaviour
{
    public GameObject controller;
    private bool drawn;

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game g = controller.GetComponent<Game>();
        CityDeck cd = GameObject.Find("CityDeck").GetComponent<CityDeck>();
        if (cd.getDrawn()) //only if the player has drawn two first
        {
            //GameObject controller = GameObject.Find("GameController");
            controller.GetComponent<Game>().infect_step();
            //controller.GetComponent<Game>().setAction(0);            //reset the actions
            //controller.GetComponent<Game>().NextTurn();//make it the next person's turn
            cd.setDrawn(false);
            setDrawn(true);
            controller.GetComponent<Game>().NextTurn();//make it the next person's turn
            controller.GetComponent<Game>().setAction(0);            //reset the actions
        }
    }
    public bool getDrawn()
    {
        return drawn;
    }
    public void setDrawn(bool b) { drawn = b; }
}
