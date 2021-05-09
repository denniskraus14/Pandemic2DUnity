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
    public Sprite pawnsprite;
    
    private City location; //you changed this to string to get past an error
    private string role;
    private string name;
    private List<Card> cards;
    private int order;

    public int getOrder()
    {
        return order;
    }
    public void setOrder(int t)
    {
        order=t;
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
    public GameObject getPlayer()
    {
        return player;
    }
    public void setPlayer(GameObject p)
    {
        player = p;
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
    public List<Card> getCards()
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
    public void setCards(List<Card> cs)
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
        this.GetComponent<SpriteRenderer>().sprite = pawnsprite;
    }

    private void OnMouseUp() {
        Card card = null;
        Pawn p = null;
        Pawn current = controller.GetComponent<Game>().getCurrentPlayer().GetComponent<Pawn>();
        bool researchercard = false;
        City loc = null;
        City loc2 = null;
        bool samespace = false; 
        try
        {
            card = controller.GetComponent<Game>().getWhichcard();
            p = card.whoseOwner();
            current = controller.GetComponent<Game>().getCurrentPlayer().GetComponent<Pawn>();
            researchercard = p.getRole().Equals("Researcher");
            loc = p.getLocation();
            loc2 = this.getLocation();
            samespace = loc.Equals(loc2);
        }
        catch {}
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().getCurrentPlayer().Equals(player) && controller.GetComponent<Game>().getAction() != 4)
        {
            DestroyMovePlates();  //remove previous move plates
            InitiateMovePlates(); //initiate new move plates

            if (controller.GetComponent<Game>().getCardclicked())
            {
                if (samespace)      
                {
                    if (card.getName().Equals(loc.getName())) //the user clicked on someone else's card of that city and then clicked on themself
                    {
                        List<Card> hand = p.getCards();
                        List<Card> hand2 = current.getCards();
                        hand.Remove(card);
                        hand2.Add(card);
                        p.setCards(hand);
                        current.setCards(hand2);//exchange it, set cards
                        loc.ActionSpent(); //spend action
                        controller.GetComponent<Game>().display_cards(); //change positions
                    }
                    else if (researchercard) //the user clicked on a researcher card and then clicked on themself
                    {             
                        //exchange it, spend action, change positions, set cards
                    }
                }
                else
                {
                    controller.GetComponent<Game>().setCardclicked(false);
                    controller.GetComponent<Game>().setWhichcard(null);
                }
            }
            else
            {
                controller.GetComponent<Game>().setCardclicked(false);
                controller.GetComponent<Game>().setWhichcard(null);
            }
        }
        else if (!controller.GetComponent<Game>().getCurrentPlayer().Equals(player) && controller.GetComponent<Game>().getAction() != 4 && !controller.GetComponent<Game>().IsGameOver())
        {
            DestroyMovePlates();
            if (researchercard && this.Equals(current)) {
                //exchange it, spend action, change positions, set cards
            }//NOT clicking on a researcher card and then clicking on someone else
            else if(true) {
                //exchange it, spend action, change positions, set cards
            } //the user clicked on their own card and then clicked on someone else
            
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
    public void AllMovePlates()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Dictionary<string,City> cities = controller.GetComponent<Game>().getCities();
        foreach (KeyValuePair<string,City> kvp in cities)
        {
            City nei = kvp.Value;
            Color temp = nei.GetComponent<SpriteRenderer>().color;
            temp.a = 1.0f;
            nei.GetComponent<SpriteRenderer>().color = temp;
        }
    }

    public bool hasCard(Card c)
    {
        foreach(Card card in getCards())
        {
            if (card.getName().Equals(c.getName())) { return true; }
        }
        return false;
    }

}
