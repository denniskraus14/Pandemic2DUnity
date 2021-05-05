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
    public GameObject cube;
    public Sprite citysprite;

    public Sprite getSprite()
    {
        return citysprite;
    }

    public void setCitysprite(Sprite s)
    {
        citysprite = s;
    }

    public GameObject getCube()
    {
        return cube;
    }
    public void setCube(GameObject c)
    {
        cube = c;
    }

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
        Sprite c;
        switch (this.color)
        {
            case "black":  c = Resources.Load<Sprite>("black_city"); this.GetComponent<SpriteRenderer>().sprite = c; setCitysprite(c);
                break;
            case "blue":  c = Resources.Load<Sprite>("blue_city"); this.GetComponent<SpriteRenderer>().sprite = c; setCitysprite(c);
                break;
            case "red":  c = Resources.Load<Sprite>("red_city"); this.GetComponent<SpriteRenderer>().sprite = c; setCitysprite(c);
                break;
            case "yellow":  c = Resources.Load<Sprite>("yellow_city"); this.GetComponent<SpriteRenderer>().sprite = c; setCitysprite(c);
                break;
        }
    }

    public void SetCoords()
    {
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject obj = controller.GetComponent<Game>().getCurrentPlayer();
        Pawn player = obj.GetComponent<Pawn>();
        City loc = player.getLocation();
        List<City> neis = loc.connections;
        if (neis.Contains(this))         //make sure this city is a valid one to go to
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
            //did you lose? check outbreaks and cube counts
            controller.GetComponent<Game>().setAction(0);            //reset the actions
            controller.GetComponent<Game>().NextTurn();//make it the next person's turn
        }
        else ;//the turn just keeps going
        
    }
}
