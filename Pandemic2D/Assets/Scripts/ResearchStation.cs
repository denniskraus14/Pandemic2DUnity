using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchStation : MonoBehaviour
{
    public GameObject controller;
    public City city;

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().getAction() < 4)
        {
            GameObject go = controller.GetComponent<Game>().getCurrentPlayer();
            Pawn p = go.GetComponent<Pawn>();
            City loc = p.getLocation();
            City loc2 = getCity();
            if (loc.getResearchStation()) //you are on the same space as a research station
            {
                if (loc.getName().Equals(loc2.getName()))
                {
                    //what should happen here? maybe curing?
                }
                else
                {
                    p.setLocation(loc2);
                    List<Pawn> ps = loc.getPlayers();
                    ps.Remove(p);
                    loc.setPlayers(ps);
                    ps = loc2.getPlayers();
                    ps.Add(p);
                    loc2.setPlayers(ps);
                    p.transform.position = new Vector3(loc2.getXBoard(), loc2.getYBoard(), -2.0f);//go there, spend an action
                    loc.ActionSpent();
                    controller.GetComponent<Game>().Arrange(loc2);
                }
            }
            else
            {
                //if you are at a neighboring spot you can travel to this spot(it kinda blocks the city)
            }
        }
    }

    public void setCity(City c) { city = c; }
    public City getCity() { return city; }
}
