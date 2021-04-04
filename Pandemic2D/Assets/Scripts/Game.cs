using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using System.Threading;



public class Game : MonoBehaviour
{
    public GameObject pawn;
    public GameObject testcity;


    public static int turn = 1;
    public List<GameObject> pawns; //replacing players
    public List<GameObject> diseases; //replacing diseases - make a disease class
    public Dictionary<string,City> cities; //replacing cities

    private GameObject currentPlayer;

    private bool gameOver = false;


    // Start is called before the first frame update
    public void Start()
    {
        //scene that asks for how many players
        //scene that asks to choose roles/ randomize roles
        //InitializeCities();
        int n = 4; //assume 4 for now
        List<string> names = new List<string>() { "Dennis","Dad","Gram","Oreo","Dhhyey","Jessie","Eric","Claus","Claus's Wife"};
        List<string> roles = new List<string>() { "Dispatcher", "Medic", "Researcher", "Scientist", "QuarantineSpecialist", "ContingencyPlanner", "OperationsExpert "};
        //set all piece positions on the board (Atlanta)
        var random = new System.Random();
        for (int i = 1; i <=n; i++)
        {
            //create the pawn object
            int random1 = random.Next(names.Count);
            int random2 = random.Next(roles.Count);
            GameObject temp = Create(names[random1], roles[random2],0,0); //figure out the location of atlanta
            names.Remove(names[random1]);
            roles.Remove(roles[random2]);
            string role = temp.GetComponent<Pawn>().getRole();
            switch(role){
                case "Dispatcher":temp.GetComponent<SpriteRenderer>().color = new Color(.62f,.196f,.745f,1.0f);break;
                case "Medic":temp.GetComponent<SpriteRenderer>().color = new Color(1.0f,.556f,0.0f,1.0f);break;
                case "Scientist": break;
                case "Researcher": temp.GetComponent<SpriteRenderer>().color = new Color(.38f,.227f,.035f,1.0f); break;
                case "QuarantineSpecialist": temp.GetComponent<SpriteRenderer>().color = new Color(.067f,.341f,.035f,1.0f); break;
                case "ContingencyPlanner": temp.GetComponent<SpriteRenderer>().color = new Color(.13f,.52f,.79f,1.0f); break;
                case "OperationsExpert": temp.GetComponent<SpriteRenderer>().color = new Color(.286f,.87f,.35f,1.0f); break;
            }
            pawns.Add(temp);
            SetPosition(temp); //make the pawn show up
        }
        currentPlayer = pawns[turn - 1];
}
    /*
    public static void InitializeCities()
    {
        List<string> citynames = new List<string>() {
        "Atlanta","Chicago","Montreal","San Francisco","New York","Madrid","London","Essen","Paris","Milan","St. Petersburg","Washington",
        "Miami", "Los Angeles","Mexico City","Bogota","Sao Paolo","Lima","Santiago","Buenos Aires","Lagos","Kinshasa","Khartoum","Johannesburg",
        "Algiers","Istanbul","Moscow","Tehran","Baghdad","Riyadh","Karachi","Delhi","Mumbai","Cairo","Chennai","Kolkatta",
        "Sydney","Manilla","Jakarta","Ho Chi Minh City","Hong Kong","Bangkok","Taipei","Osaka","Beijing","Seoul","Tokyo","Shanghai"
        };

        List<int> populations = new List<int>(){
            2946000,8865000,6204000,17718000,15512000,16910000,5037000,
            7419000,20711000,14374000,22242000,13576000,
            4715000,5232000,3429000,20464000,5864000,4879000,5427000,10755000,9121000,575000,8586000,
            4679000,
            17311000,22547000,3785000,8338000,8314000,7106000,20767000,2871000,26063000,13189000,
            8702000,13639000,4887000,9046000,6015000,20186000,11547000,9121000,19463000,
            5582000,3888000, 14900000
        };
        string[] colors = new string[] {"black","blue","red","yellow" };
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 12; j++)
            {
                Dictionary<string, int> acc = new Dictionary<string, int>();
                acc.Add("black", 0);
                acc.Add("blue", 0);
                acc.Add("red", 0);
                acc.Add("yellow", 0);
                GameObject temp = CreateCity(citynames[i * 12 + j], colors[i], acc,false,false, populations[i * 12 + j],0,0);//fix xboard yboard
                cities.Add(citynames[i*12+j],temp.GetComponent<City>());
            }
        }
    }*/

    public GameObject Create(string name, string role, int x, int y)
    {
        Thread.Sleep(1);//ensures at least 1 millisecond has passed
        var random = new System.Random(DateTime.Now.Millisecond);
        GameObject obj = Instantiate(pawn, new Vector3(random.Next(-442,-412), random.Next(85,115), -2), Quaternion.identity);
        Pawn p = obj.GetComponent<Pawn>();
        p.setName(name);
        p.setRole(role);
        p.setLocation("Atlanta");
        p.setCards(null); //fix this later
        p.SetXBoard(x);
        p.SetYBoard(y);
        p.Activate();
        return obj;
    }
    /*
    public static GameObject CreateCity(string name, string color, Dictionary<string,int> cubes, bool q, bool rs, int pop, int x, int y)
    {
        GameObject obj = Instantiate(testcity, new Vector3(0, 0, -2), Quaternion.identity);
        City c = obj.GetComponent<City>();
        c.setName(name);
        c.setColor(color);
        c.setCubes(cubes);
        c.setQuarantined(q); //fix this later
        c.setResearchStation(rs);
        c.setPopulation(pop);
        c.SetXBoard(x);
        c.SetYBoard(y);
        c.Activate();
        return obj;
    }*/

    public void SetPosition(GameObject obj)
    {
        Pawn p = obj.GetComponent<Pawn>();
        pawns.Add(obj);
    }

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (turn == 4)
        {
            turn = 1;
        }
        else
        {
            turn += 1;
        }
    }

    public void Update()
    {
        if (gameOver)
        {
            gameOver = false; 
            SceneManager.LoadScene("Game");//start it over
        }
    }
}
