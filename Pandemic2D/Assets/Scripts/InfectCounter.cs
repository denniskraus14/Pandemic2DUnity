using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectCounter : MonoBehaviour
{
    private int count = 2;

    //this method would be called when epidemic cards are drawn
    public void setCount(int c)
    {
        count = c;
    }

    //this method would be called to in the infect_step() to find out how many cards to draw
    public int getCount()
    {
        return count;
    }
}
