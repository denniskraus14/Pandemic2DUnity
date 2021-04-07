using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    //References 
    public GameObject controller;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

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
}
