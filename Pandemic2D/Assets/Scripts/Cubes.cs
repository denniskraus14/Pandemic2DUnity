using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    public GameObject controller;
    private City location;

    //the current implementation ignores the possibility of a city having more than one kind of cubes
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Pawn p = controller.GetComponent<Game>().getCurrentPlayer().GetComponent<Pawn>();
        string role = p.getRole();
        City c = p.getLocation();
        Dictionary<string, int> cubes = c.getCubes();

        //if the city the current player is in matches the city of the cube
        if (c.getName().Equals(getLocation().getName())) {
            if (role.Equals("Medic")) //add in the bit about the disease being cured already
            {
                cubes[c.getColor()] = 0; //pick them all up
                c.setCubes(cubes);
                controller.GetComponent<Game>().display_cubes(c); //remove sprite
            }
            else {
                cubes[c.getColor()] = cubes[c.getColor()] - 1;//pick up 1
                c.setCubes(cubes);
                controller.GetComponent<Game>().display_cubes(c);//change sprite
            }
            c.ActionSpent();
        }

    }

    public void setLocation(City c) { location = c; }
    public City getLocation() { return location; }
}
