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

    //var to keep track of turn
    private string player;

    //refs for all the sprites of chess pieces
    public Sprite Dispatcher, Medic, Researcher, Scientist, QuarantineSpecialist, ContingencyPlanner, OperationsExpert;


    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //take the instantiated location and adjust the transform
        SetCoords(); //everyone would start in atlanta

        switch (this.name)
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
    {
        /*
        float x = xBoard;
        float y = yBoard;

        x *= .66f;
        y *= .66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -2.0f);*/
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {//once the valid pawn is clicked, what should happen? 
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
