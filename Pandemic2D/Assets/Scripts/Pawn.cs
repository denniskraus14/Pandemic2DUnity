using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //References 
    public GameObject controller;
    
    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //var to keep track of turn
    private string player;

    //refs for all the sprites of chess pieces
    public Sprite Dispatcher, Medic, Researcher, Scientist, QuarantineSpecialist, ContingencyPlanner, OperationsExpert;
    
    private City location; //you changed this to string to get past an error
    private string role;
    private string name;
    private City[] cards;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //take the instantiated location and adjust the transform
        SetCoords(); //everyone would start in atlanta

        switch (this.role)
        {
            case "Dispatcher": this.GetComponent<SpriteRenderer>().sprite = Dispatcher; break;
            case "Medic": this.GetComponent<SpriteRenderer>().sprite = Medic; break;
            case "Researcher": this.GetComponent<SpriteRenderer>().sprite = Researcher; break;
            case "Scientist": this.GetComponent<SpriteRenderer>().sprite = Scientist; break;
            case "QuarantineSpecialist": this.GetComponent<SpriteRenderer>().sprite = QuarantineSpecialist; break;
            case "ContingencyPlanner": this.GetComponent<SpriteRenderer>().sprite = ContingencyPlanner;break;
            case "OperationsExpert": this.GetComponent<SpriteRenderer>().sprite = OperationsExpert; break;
        }
    }

    public void SetCoords()
    {//get everyone to start in atlanta
        /*
        float x = xBoard;
        float y = yBoard;

        x *= .66f;
        y *= .66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -2.0f);*/
    }

    public int getXBoard()
    {
        return xBoard;
    }

    public int getYBoard()
    {
        return yBoard;
    }

    public void setXBoard(int x)
    {
        xBoard = x;
    }

    public void setYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {//once the valid pawn is clicked, what should happen? 
    
    }

    public City getLocation()
    {
        return location;
    }
    public string getName()
    {
        return name;
    }
    public string getRole()
    {
        return role;
    }
    public City[] getCards()
    {
        return cards;
    }
    public void setName(string n)
    {
        name = n;
    }
    public void setLocation(City c)
    {
        location = c;
    }
    public void setCards(City[] cs)
    {
        cards = cs;
    }
    public void setRole(string r)
    {
        role = r;
    }
}
