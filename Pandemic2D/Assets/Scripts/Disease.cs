using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disease : MonoBehaviour
{
    string color;
    bool cured;
    bool eradicated;
    int cubes;

    public string getColor()
    {
        return color;
    }
    public bool getCured()
    {
        return cured;
    }
    public bool getEradicated()
    {
        return eradicated;
    }
    public int getCubes()
    {
        return cubes;
    }
    public void setColor(string c)
    {
        color = c;
    }
    public void setCured(bool b)
    {
        cured = b;
    }
    public void setEradicated(bool e)
    {
        eradicated = e;
    }
    public void setCubes(int c)
    {
        cubes = c;
    }
}
