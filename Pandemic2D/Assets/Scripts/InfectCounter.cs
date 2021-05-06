using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectCounter : MonoBehaviour
{
    private List<int> rates = new List<int>() { 2, 2, 2, 3, 3, 4, 4 };

    private int index = 0;

    //this method would be called when epidemic cards are drawn
    public void setIndex(int c)
    {
        index = c;
    }

    //this method would be called to in the infect_step() to find out how many cards to draw
    public int getIndex()
    {
        return index;
    }
    public int getRate() {
        return rates[getIndex()];
    }
}
