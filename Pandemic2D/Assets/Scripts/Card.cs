using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string name;
    private bool stored;
    private bool discarded;
    public GameObject controller;
    
    public Card(string n, bool s = false, bool d = false)
    {
        name = n;
        stored = s;
        discarded = d;
    }

    public string getName(){return name;}
    public void setName(string s) { name = s; }
    public bool getStored(){return stored;}
    public void setStored(bool s){stored = s;}
    public bool getDiscarded() { return discarded; }
    public void setDiscarded(bool b) { discarded = b; }

    //onmouseup - no menu needed
    /*
     trade : click on card, click on pawn
     research station : click on card, click on current city you are in that matches the card (moveplate)
     fly : click on card, moveplate to that city only, click on city
     teleport : click on card of city you are in, move plates to all cities, click on any city
     */
    public void OnMouseUp()
    {
        //if it has been discarded
        if (getDiscarded()) { } //maybe do an action that lets you inspect the discard pile
        else
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            string name = getName();
            Dictionary<string, City> cities = controller.GetComponent<Game>().getCities();
            City c2;
            try{ c2 = cities[name];}
            catch { c2 = null; }

            Pawn p = controller.GetComponent<Game>().getCurrentPlayer().GetComponent<Pawn>();
            p.DestroyMovePlates();
            City c = p.getLocation(); //the location of the current player
            Pawn other = whoseOwner(); //is this working?
            City otherloc = other.getLocation(); //the location of the clicked pawn
            //first check if the current player is able to interact with this card 
            // -if it is your card or
            // -if it is some other player's card of the shared space
            // -if it is the researchers cards
            bool inhand = p.hasCard(this);
            bool researcher = other.getRole().Equals("Researcher"); //&& other.getLocation().getName().Equals(c.getName());
            bool samespace = otherloc.getName().Equals(name) && otherloc.getName().Equals(c.getName());
            if ((inhand || researcher || samespace) && controller.GetComponent<Game>().getAction() != 4)
            {
                controller.GetComponent<Game>().setCardclicked(true);
                controller.GetComponent<Game>().setWhichcard(this);
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
    }
    

    public Pawn whoseOwner()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        List<GameObject> ps = controller.GetComponent<Game>().getPawns();
        foreach(GameObject p in ps)
        {
            if (p.GetComponent<Pawn>().hasCard(this))
            {
                return p.GetComponent<Pawn>();
            }
        }
        return null; //this case should not/cannot happen
    }
    
    public virtual bool Equals(Card c) {
        return c.getName().Equals(this.getName());
    }

}
