using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //References 
    public GameObject controller;
    
    //Positions
    private float xBoard = -1;
    private float yBoard = -1;
    public GameObject movePlate;

    //var to keep track of turn
    private GameObject player;

    //refs for all the sprites of chess pieces
    public Sprite Dispatcher, Medic, Researcher, Scientist, QuarantineSpecialist, ContingencyPlanner, OperationsExpert;
    
    private City location; //you changed this to string to get past an error
    private string role;
    private string name;
    private City[] cards;

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
    public GameObject getPlayer()
    {
        return player;
    }
    public void setPlayer(GameObject p)
    {
        player = p;
    }

    private void OnMouseUp() {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().getCurrentPlayer().Equals(player))
        {
            DestroyMovePlates();  //remove previous move plates
            InitiateMovePlates(); //initiate new move plates
        }
        else if(!controller.GetComponent<Game>().getCurrentPlayer().Equals(player))
        {
            DestroyMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        Dictionary<string, City> cs = controller.GetComponent<Game>().getCities();
        foreach(KeyValuePair<string,City> kvp in cs)
        {
            //just make all city sprites translucent again. don't destroy!
            Color temp = kvp.Value.GetComponent<SpriteRenderer>().color;
            temp.a = 0.0f;
            kvp.Value.GetComponent<SpriteRenderer>().color = temp;
        }
    }
    private void InitiateMovePlates()
    {
        ConnectionMovePlates();
    }

    private void ConnectionMovePlates()
    {
        City loc = this.location;
        List<City> neis = loc.getConnections();
        foreach(City nei in neis)
        {
            //make the neighboring city sprites visible
            Color temp = nei.GetComponent<SpriteRenderer>().color;
            temp.a = 1.0f;
            nei.GetComponent<SpriteRenderer>().color = temp;
        }
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

    
}
