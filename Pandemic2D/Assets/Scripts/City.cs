using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    //References 
    public GameObject controller;

    //Positions
    private float xBoard = -1;
    private float yBoard = -1;

    private string name;
    private string color;
    private Dictionary<string, int> cubes;
    private bool quarantined = false;
    private bool research_station = false;
    private int population;
    private List<Pawn> players;
    private List<City> connections;

    public Sprite red_city, blue_city, yellow_city, black_city;

    public string getName()
    {
        return name;
    }
    public string getColor()
    {
        return color;
    }
    public Dictionary<string, int> getCubes()
    {
        return cubes;
    }
    public bool getQuarantined()
    {
        return quarantined;
    }
    public bool getResearchStation()
    {
        return research_station;
    }
    public int getPopulation()
    {
        return population;
    }
    public void setName(string n)
    {
        name = n;
    }
    public void setColor(string c)
    {
        color = c;
    }
    public void setCubes(Dictionary<string, int> cs)
    {
        cubes = cs;
    }
    public void setQuarantined(bool q)
    {
        quarantined = q;
    }
    public void setResearchStation(bool rs)
    {
        research_station = rs;
    }
    public void setPopulation(int p)
    {
        population = p;
    }
    public void setPlayers(List<Pawn> ps)
    {
        players = ps;
    }

    public List<Pawn> getPlayers()
    {
        return players;
    }
    public void setConnections(List<City> cs)
    {
        connections = cs;
    }
    public List<City> getConnections()
    {
        return connections;
    }
    public float getXBoard()
    {
        return xBoard;
    }

    public float getYBoard()
    {
        return yBoard;
    }

    public void setXBoard(float x)
    {
        xBoard = x;
    }

    public void setYBoard(float y)
    {
        yBoard = y;
    }

    public void Activate(string name)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //take the instantiated location and adjust the transform
        SetCoords(); //everyone would start in atlanta
        this.name = name;
        switch (this.color)
        {
            case "black": this.GetComponent<SpriteRenderer>().sprite = black_city; break;
            case "blue": this.GetComponent<SpriteRenderer>().sprite = blue_city; break;
            case "red": this.GetComponent<SpriteRenderer>().sprite = red_city; break;
            case "yellow": this.GetComponent<SpriteRenderer>().sprite = yellow_city; break;
        }
    }

    public void SetCoords()
    {//get cities to their positions?
        /*
        float x = xBoard;
        float y = yBoard;

        x *= .66f;
        y *= .66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -2.0f);*/
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //make sure this city is a valid one to go to
        GameObject obj = controller.GetComponent<Game>().getCurrentPlayer();
        Pawn player = obj.GetComponent<Pawn>();
        City loc = player.getLocation();
        List<City> neis = loc.connections;
        if (neis.Contains(this))
        {
            loc.getPlayers().Remove(player);
            player.DestroyMovePlates();//destroy the moveplates
            //move the person
            float x = this.getXBoard();
            float y = this.getYBoard();
            player.setXBoard(x);
            player.setYBoard(y); //is this enough to move the sprite as well?
            player.transform.position = new Vector3(player.getXBoard(),player.getYBoard(),-2.0f);
            player.setLocation(this);
            this.getPlayers().Add(player);
            controller.GetComponent<Game>().Arrange(this);
            ActionSpent();
        }
        else
        {
            player.DestroyMovePlates();//destroy the moveplates
            //player.setXBoard(0.0f);
            //player.setYBoard(0.0f); //this will tell you when a city move is invalid
            //player.transform.position = new Vector3(player.getXBoard(), player.getYBoard(), -2.0f);
        }
    }

    //this checks if 4 actions have happened and sets off a chain of events
    //this could be changed later to give the user more control. (when to play event cards for example)
    public void ActionSpent()
    {
        //increase the action counter
        controller.GetComponent<Game>().setAction(controller.GetComponent<Game>().getAction() + 1);
        if (controller.GetComponent<Game>().getAction()== 4)
        {
            controller.GetComponent<Game>().draw_two(); //draw two
            //discard to 7 and resolve epidemics (later lol)
            controller.GetComponent<Game>().infect_step();
            controller.GetComponent<Game>().setAction(0);            //reset the actions

            controller.GetComponent<Game>().NextTurn();//make it the next person's turn
        }
        else ;//the turn just keeps going
        
    }
}
