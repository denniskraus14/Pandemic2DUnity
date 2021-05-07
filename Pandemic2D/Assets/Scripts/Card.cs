using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string name;
    private bool stored;
    public GameObject controller;
    
    public Card(string n, bool s = false)
    {
        name = n;
        stored = s;
    }
    public string getName()
    {
        return name;
    }
    public void setName(string s) { name = s; }
    public bool getStored()
    {
        return stored;
    }
    public void setStored(bool s)
    {
        stored = s;
    }

    //onmouseup - no menu needed
    /*
     trade : click on card, click on pawn
     research station : click on card, click on current city you are in that matches the card (moveplate)
     fly : click on card, moveplate to that city only, click on city
     teleport : click on card of city you are in, move plates to all cities, click on any city
     */
    public void OnMouseUp()
    {
        
        controller = GameObject.FindGameObjectWithTag("GameController");
        string name = getName();
        Dictionary<string, City> cities = controller.GetComponent<Game>().getCities();
        City c2 = cities[name];
        Pawn p = controller.GetComponent<Game>().getCurrentPlayer().GetComponent<Pawn>();
        p.DestroyMovePlates();
        City c = p.getLocation();
        Pawn other = whoseOwner();
        //first check if the current player is able to interact with this card 
        // -if it is your card or
        // -if it is some other player's card of the shared space
        // -if it is the researchers cards
        bool inhand = false;
        foreach(Card k in p.getCards())
        {
            if (k.Equals(this)) { inhand = true; }
        }
        bool researcher = other.getRole().Equals("Researcher") && other.getLocation().getName().Equals(c.getName());
        bool samespace = other.getLocation().getName().Equals(name) && other.getLocation().getName().Equals(c.getName());
        if (inhand || researcher || samespace)  {
            controller.GetComponent<Game>().setCardclicked(true);
            //if you are in the same city as the card you clicked
            if (c.getName().Equals(getName()))
            {
                p.AllMovePlates();//display all moveplates
            }
            else
            {
                Color temp = c2.GetComponent<SpriteRenderer>().color;
                temp.a = 1.0f;
                c2.GetComponent<SpriteRenderer>().color = temp;//show a moveplate to that one specific city
            }
        }
    }
    

    public Pawn whoseOwner()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        List<GameObject> ps = controller.GetComponent<Game>().getPawns();
        foreach(GameObject p in ps)
        {
            foreach (Card c in p.GetComponent<Pawn>().getCards()) {
                if (c.Equals(this))
                {
                    return p.GetComponent<Pawn>();
                }
            }
        }
        return null; //this case should not/ cannot happen
    }

    public virtual bool Equals(Card c) {
        return c.getName().Equals(this.getName());
    }

}
