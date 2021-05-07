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
    public GameObject city;

    public int turn = 1;
    public int action = 0;
    public List<GameObject> pawns; //replacing players dict
    public List<GameObject> diseases; //replacing diseases - make a disease class
    public Dictionary<string, City> cities = new Dictionary<string, City>(); //replacing cities
    public GameObject initial_station, outbreak_counter, infect_counter, disease1,disease2,disease3,disease4, citydeckobj, infectdeckobj;
    public List<Card> citydeck;
    public List<Card> citydeck_discard;
    public List<Card> infectdeck;
    public List<Card> infectdeck_discard;
    public List<Card> eventcards;
    private GameObject currentPlayer;
    private bool gameOver = false;
    private int outbreaks;
    private int infectrate;
    public GameObject Card;
    public GameObject cube;
    private bool cardclicked;


    // Start is called before the first frame update
    public void Start()
    {
        //scene that asks for how many players
        //scene that asks to choose roles/ randomize roles
        InitializeCities();
        int n = 4; //assume 4 for now
        List<string> names = new List<string>() { "Dennis", "Dad", "Gram", "Oreo", "Dhhyey", "Zach", "Kathleen", "Eric", "Claus", "Claus's Wife" };
        List<string> roles = new List<string>() { "Dispatcher", "Medic", "Researcher", "Scientist", "QuarantineSpecialist", "ContingencyPlanner", "OperationsExpert" };
        setOutbreaks(0);
        
        //set all piece positions on the board (Atlanta)
        var random = new System.Random();
        for (int i = 1; i <= n; i++)
        {
            //create the pawn object
            int random1 = random.Next(names.Count);
            int random2 = random.Next(roles.Count);
            GameObject temp = Create(names[random1], roles[random2]);
            names.Remove(names[random1]);
            roles.Remove(roles[random2]);
            string role = temp.GetComponent<Pawn>().getRole();
            switch (role)
            {
                case "Dispatcher": temp.GetComponent<SpriteRenderer>().color = new Color(.62f, .196f, .745f, 1.0f); break;
                case "Medic": temp.GetComponent<SpriteRenderer>().color = new Color(1.0f, .556f, 0.0f, 1.0f); break;
                case "Scientist": break;
                case "Researcher": temp.GetComponent<SpriteRenderer>().color = new Color(.38f, .227f, .035f, 1.0f); break;
                case "QuarantineSpecialist": temp.GetComponent<SpriteRenderer>().color = new Color(.067f, .341f, .035f, 1.0f); break;
                case "ContingencyPlanner": temp.GetComponent<SpriteRenderer>().color = new Color(.05f, .52f, .79f, 1.0f); break;
                case "OperationsExpert": temp.GetComponent<SpriteRenderer>().color = new Color(.286f, .87f, .35f, 1.0f); break;
            }
            temp.GetComponent<Pawn>().setOrder(i); //why is this assigning them all the same turn
            pawns.Add(temp);
            SetPosition(temp); //make the pawn show up
        }
        //pawns[getTurn() - 1].GetComponent<Pawn>().setTurn(getTurn()-1);
        setCurrentPlayer(pawns[getTurn() - 1]);
        GameObject obj = Instantiate(initial_station, new Vector3(-445, 110, -2), Quaternion.identity); //research station
        GameObject obj1 = Instantiate(infect_counter, new Vector3(85, 230, -2), Quaternion.identity);
        obj1.name = "InfectCounter";
        obj1.GetComponent<InfectCounter>().setIndex(0);
        GameObject obj2 = Instantiate(outbreak_counter, new Vector3(-590, -45, -2), Quaternion.identity);
        obj2.name = "OutbreakCounter";
        GameObject obj3 = Instantiate(disease1, new Vector3(-280, -355, -2), Quaternion.identity);
        obj3.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,0.0f,1.0f);
        obj3.name = "Yellow Disease";
        obj3.GetComponent<Disease>().setColor("yellow");
        GameObject obj4 = Instantiate(disease2, new Vector3(-220, -355, -2), Quaternion.identity);
        obj4.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        obj4.name = "Red Disease";
        obj4.GetComponent<Disease>().setColor("red");
        GameObject obj5 = Instantiate(disease3, new Vector3(-170, -355, -2), Quaternion.identity);
        obj5.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        obj5.name = "Blue Disease";
        obj5.GetComponent<Disease>().setColor("blue");
        GameObject obj6 = Instantiate(disease4, new Vector3(-120, -355, -2), Quaternion.identity);
        obj6.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1.0f);
        obj6.name = "Black Disease";
        obj6.GetComponent<Disease>().setColor("black");
        GameObject obj7 = Instantiate(citydeckobj, new Vector3(100,-280,-2), Quaternion.identity);
        obj7.name = "CityDeck"; //make scale 15x15
        obj7.transform.localScale = new Vector3(15,15,1);
        GameObject obj8 = Instantiate(infectdeckobj, new Vector3(123,318,-2), Quaternion.identity);
        obj8.name = "InfectDeck";
        obj8.transform.localScale = new Vector3(15, 15, 1);
        obj8.transform.Rotate(.867f, .025f, 90.0f);
        obj8.GetComponent<InfectDeck>().setDrawn(true);
        List<GameObject> acc = new List<GameObject>() { obj3,obj4,obj5,obj6};
        setDiseases(acc);
        InitializeDecks();//initialize decks
        pregame_dealing();
        display_cards();
        pregame_city_deck_prep(6); //assume 6 for now
        //next infect the cities and properly prepare the cdeck w epidemics
    }

    public void pregame_city_deck_prep(int piles)
    {
        System.Random random = new System.Random();
        int p = getPawns().Count; //4
        List<Card> cdeck = getCitydeck();
        setCitydeck(cdeck.OrderBy(c => random.Next()).ToList());
        List<int> sublist_lengths = null;
        if (p==4 || p==2){
            if (piles == 4) { sublist_lengths = new List<int> { 11, 11, 11, 12 }; }
            else if(piles == 5){ sublist_lengths = new List<int> {9,9,9,9,9}; }
            else { sublist_lengths = new List<int> { 8, 8, 8, 7, 7, 7 }; }
        }
        else
        {
            if (piles == 4) { sublist_lengths = new List<int> { 10,10,11,11 }; }
            else if (piles == 5) { sublist_lengths = new List<int> { 8,8,8, 9, 9 }; }
            else { sublist_lengths = new List<int> { 7,7,7, 7, 7, 7 }; }
        }
        List<List<Card>> subdecks = new List<List<Card>>();
        int i = 0;
        int start = 0;
        while (i < piles-1) //shoud these be -1?
        {
            List<Card> subdeck = cdeck.GetRange(start, sublist_lengths[i]);
            subdecks.Add(subdeck);
            start = start + sublist_lengths[i]; //is this right
            //i += 1;
            subdecks[i].Add(new Card("Epidemic"));
            subdecks[i] = subdecks[i].OrderBy(c => random.Next()).ToList();
            i += 1;
        }
        cdeck = new List<Card>(); //empty it
        subdecks = subdecks.OrderBy(s =>random.Next()).ToList();
        while(subdecks.Count>0)
        {
            foreach (Card card in subdecks[0])
            {
                cdeck.Add(card);
            }
            subdecks.Remove(subdecks[0]);
        }
        setCitydeck(cdeck);
    }

    public void display_cards()
    {
        int n = getPawns().Count;
        if (n == 4){
            foreach(GameObject go in getPawns())
            {
                Pawn p = go.GetComponent<Pawn>();
                foreach(Card c in p.getCards())
                {
                    String name = c.getName();
                    String name2 = name.Replace(" ", String.Empty);
                    var sprite = Resources.Load<Sprite>(name2); //ensure all cities are spelled the same. also check caps
                    GameObject temp = Instantiate(Card, new Vector3(0, 0, -3), Quaternion.identity);
                    temp.GetComponent<SpriteRenderer>().sprite = sprite; 
                    temp.name = name2+"Card";
                    temp.GetComponent<Card>().setName(name);
                }
            }
            
    }else if(n==3){
    }else{
    }
    ArrangeCards();
    }


    public void InitializeCities()
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
            5582000,3888000, 14900000,0,0
        };
        List<Tuple<int, int>> locations = new List<Tuple<int, int>>() {
        new Tuple<int,int>(-445,100),new Tuple<int,int>(-477,171), new Tuple<int,int>(-395,168),new Tuple<int,int>(-582,136),new Tuple<int,int>(-334,160),new Tuple<int,int>(-187,125),
        new Tuple<int,int>(-177,208),new Tuple<int,int>(-97,222),new Tuple<int,int>(-114,164),new Tuple<int,int>(-60,183),new Tuple<int,int>(-3,238),new Tuple<int,int>(-360,107),
        new Tuple<int,int>(-397,32),new Tuple<int,int>(-562,50), new Tuple<int,int>(-487,20),new Tuple<int,int>(-397,-50),new Tuple<int,int>(-288,-150),new Tuple<int,int>(-435,-140),
        new Tuple<int,int>(-420,-224),new Tuple<int,int>(-330,-207),new Tuple<int,int>(-123,-36),new Tuple<int,int>(-69,-97),new Tuple<int,int>(-17,-20),new Tuple<int,int>(-20,-175),
        new Tuple<int,int>(-100,78),new Tuple<int,int>(-17,135),new Tuple<int,int>(40,183),new Tuple<int,int>(100,147),new Tuple<int,int>(31,90),new Tuple<int,int>(40,15),
        new Tuple<int,int>(114,70),new Tuple<int,int>(174,90),new Tuple<int,int>(117,2),new Tuple<int,int>(-33,69),new Tuple<int,int>(187,-38),new Tuple<int,int>(231,70),
        new Tuple<int,int>(423,-222),new Tuple<int,int>(375,-60),new Tuple<int,int>(239,-107),new Tuple<int,int>(292,-60),new Tuple<int,int>(292,40),new Tuple<int,int>(242,0),
        new Tuple<int,int>(359,50),new Tuple<int,int>(415,70),new Tuple<int,int>(282,162),new Tuple<int,int>(355,160),new Tuple<int,int>(410,128),new Tuple<int,int>(286,102) };

        List<string> colors = new List<string> { "blue", "yellow", "black", "red" };
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 11; j++)
            {
                Dictionary<string, int> acc = new Dictionary<string, int>();
                acc.Add("black", 0); 
                acc.Add("blue", 0);
                acc.Add("red", 0);
                acc.Add("yellow", 0);
                if (citynames[i * 12 + j].Equals("Atlanta"))
                {
                    GameObject temp = CreateCity(citynames[i * 12 + j], colors[i], acc, false, false, populations[i * 12 + j], locations[i * 12 + j].Item1, locations[i * 12 + j].Item2);
                    temp.GetComponent<SpriteRenderer>().color = new Color(.067f, .341f, .035f, 0.0f); //this would make the city sprites clear/not appear
                    cities.Add(citynames[i * 12 + j], temp.GetComponent<City>());
                }
                else
                {
                    GameObject temp = CreateCity(citynames[i * 12 + j], colors[i], acc, false, true, populations[i * 12 + j], locations[i * 12 + j].Item1, locations[i * 12 + j].Item2);
                    temp.GetComponent<SpriteRenderer>().color = new Color(.067f, .341f, .035f, 1.0f); //this would make the city sprites clear/not appear
                    cities.Add(citynames[i * 12 + j], temp.GetComponent<City>());
                }
            }
        }
        List<List<City>> tempconnect = new List<List<City>>()
        {
            new List<City>(){cities["Miami"], cities["Washington"], cities["Chicago"] },new List<City>(){ cities["Atlanta"], cities["San Francisco"], cities["Mexico City"], cities["Montreal"], cities["Los Angeles"] },new List<City>(){ cities["New York"], cities["Washington"], cities["Chicago"]},new List<City>(){ cities["Tokyo"], cities["Manilla"], cities["Los Angeles"], cities["Chicago"]},new List<City>(){ cities["London"], cities["Madrid"], cities["Washington"], cities["Montreal" ]},new List<City>(){ cities["Sao Paolo"], cities["London"], cities["New York"], cities["Paris"], cities["Algiers"] },
            new List<City>(){ cities["Essen"], cities["Madrid"], cities["New York"], cities["Paris" ]},new List<City>(){ cities["Milan"], cities["London"], cities["St. Petersburg"], cities["Paris" ]},new List<City>(){ cities["Madrid"], cities["Milan"], cities["London"], cities["Essen"], cities["Algiers"] },new List<City>(){ cities["Essen"], cities["Paris"], cities["Istanbul"]},new List<City>(){ cities["Istanbul"], cities["Moscow"], cities["Essen"] },new List<City>(){ cities["Atlanta"], cities["New York"], cities["Montreal"], cities["Miami"] },
            new List<City>(){ cities["Atlanta"], cities["Bogota"], cities["Washington"], cities["Mexico City"] },new List<City>(){ cities["San Francisco"], cities["Mexico City"], cities["Sydney"], cities["Chicago"] },new List<City>(){ cities["Los Angeles"], cities["Bogota"], cities["Lima"], cities["Miami"], cities["Chicago"] },new List<City>(){ cities["Mexico City"], cities["Buenos Aires"], cities["Sao Paolo"], cities["Miami"], cities["Lima"] },new List<City>(){ cities["Bogota"], cities["Madrid"], cities["Buenos Aires"], cities["Lagos" ]},new List<City>(){ cities["Bogota"], cities["Mexico City"], cities["Santiago"]},
            new List<City>(){ cities["Lima"]},new List<City>(){ cities["Sao Paolo"], cities["Bogota"]},new List<City>(){ cities["Sao Paolo"], cities["Khartoum"], cities["Kinshasa"] },new List<City>(){ cities["Lagos"], cities["Johannesburg"], cities["Khartoum"]},new List<City>(){ cities["Cairo"], cities["Lagos"], cities["Kinshasa"], cities["Johannesburg"] },new List<City>(){ cities["Khartoum"], cities["Kinshasa"] },
            new List<City>(){ cities["Istanbul"], cities["Cairo"], cities["Madrid"], cities["Paris"] },new List<City>(){ cities["Baghdad"], cities["Cairo"], cities["Moscow"], cities["St. Petersburg"], cities["Algiers"], cities["Milan"] },new List<City>(){ cities["Istanbul"], cities["St. Petersburg"], cities["Tehran"] },new List<City>(){ cities["Delhi"], cities["Moscow"], cities["Karachi"], cities["Baghdad"] },new List<City>(){ cities["Istanbul"], cities["Karachi"], cities["Cairo"], cities["Riyadh"], cities["Tehran"]},new List<City>(){ cities["Cairo"], cities["Karachi"], cities["Baghdad"]},
            new List<City>(){ cities["Mumbai"], cities["Tehran"], cities["Delhi"], cities["Riyadh"], cities["Baghdad"]},new List<City>(){ cities["Mumbai"], cities["Tehran"], cities["Kolkatta"], cities["Chennai"], cities["Karachi"]},new List<City>(){ cities["Delhi"], cities["Chennai"], cities["Karachi"]},new List<City>(){ cities["Baghdad"], cities["Istanbul"], cities["Khartoum"], cities["Riyadh"], cities["Algiers"]},new List<City>(){ cities["Mumbai"], cities["Delhi"], cities["Jakarta"], cities["Kolkatta"], cities["Bangkok"]},new List<City>(){ cities["Delhi"], cities["Bangkok"], cities["Hong Kong"], cities["Chennai"]},
            new List<City>(){ cities["Jakarta"], cities["Manilla"], cities["Los Angeles"]},new List<City>(){ cities["Taipei"], cities["Ho Chi Minh City"], cities["Hong Kong"], cities["Sydney"], cities["San Francisco"] },new List<City>(){ cities["Bangkok"], cities["Sydney"], cities["Chennai"], cities["Ho Chi Minh City"]},new List<City>(){ cities["Jakarta"], cities["Manilla"], cities["Bangkok"], cities["Hong Kong"] },new List<City>(){ cities["Bangkok"], cities["Manilla"], cities["Kolkatta"], cities["Shanghai"], cities["Ho Chi Minh City"], cities["Taipei"] },new List<City>(){ cities["Kolkatta"], cities["Jakarta"], cities["Ho Chi Minh City"], cities["Chennai"], cities["Hong Kong"]},
            new List<City>(){ cities["Osaka"], cities["Shanghai"], cities["Manilla"], cities["Hong Kong"] },new List<City>(){ cities["Tokyo"], cities["Taipei"] },new List<City>(){ cities["Seoul"], cities["Shanghai"] },new List<City>(){ cities["Tokyo"], cities["Shanghai"], cities["Beijing"]},new List<City>(){ cities["Osaka"], cities["Seoul"], cities["Shanghai"], cities["San Francisco"]},new List<City>(){ cities["Seoul"], cities["Tokyo"], cities["Beijing"], cities["Hong Kong"], cities["Taipei"]},
        };
        //test  this to make sure it is working
        for (int k = 0; k < 48; k++)
        {
            cities[citynames[k]].setConnections(tempconnect[k]); //this is assigning connections to each of the cities
        }
    }
    public void InitializeDecks()
    {
        setCitydeckDiscard(new List<Card>());
        setCitydeck(new List<Card>());
        setInfectdeckDiscard(new List<Card>());
        setInfectdeck(new List<Card>());
        List<Card> temp = getCitydeck();
        List<Card> temp2 = getInfectdeck();
        Dictionary<string, City> cities = getCities();
        foreach (KeyValuePair<string,City> kvp in cities)
        {
            temp.Add(new Card(kvp.Key));
            temp2.Add(new Card(kvp.Key));
        }
        setEventcards(new List<Card>() {new Card("Airlift", false),new Card("Government Grant", false),new Card("Resilient Population", false),new Card("One Quiet Night", false),new Card("Forecast", false)});
        foreach(Card c in getEventcards())
        {
            temp.Add(c);
        }
        System.Random random = new System.Random();
        setInfectdeck(temp2.OrderBy(c => random.Next()).ToList());
        Thread.Sleep(1);
        setCitydeck(temp.OrderBy(c => random.Next()).ToList());
    }

    public GameObject Create(string name, string role)
    {
        Thread.Sleep(1);//ensures at least 1 millisecond has passed
        var random = new System.Random(DateTime.Now.Millisecond);
        int xBoard = random.Next(-442, -412);
        int yBoard = random.Next(85, 115);
        GameObject obj = Instantiate(pawn, new Vector3(xBoard, yBoard, -2), Quaternion.identity);
        Pawn p = obj.GetComponent<Pawn>();
        p.setName(name);
        p.setRole(role);
        p.setLocation(cities["Atlanta"]);
        cities["Atlanta"].getPlayers().Add(p);
        p.setCards(new List<Card>()); //fix this
        p.setXBoard(xBoard);
        p.setYBoard(yBoard);
        p.setPlayer(obj);
        p.Activate();
        obj.name = role;
        return obj;
    }

    public GameObject CreateCity(string name, string color, Dictionary<string, int> cubes, bool q, bool rs, int pop, int x, int y)
    {
        GameObject obj = Instantiate(city, new Vector3(x, y, -2), Quaternion.identity);
        City c = obj.GetComponent<City>();
        c.setName(name);
        c.setColor(color);
        cubes["red"] = 0; //this will initialize every city to have 0 cubes
        cubes["black"] = 0; //this will initialize every city to have 0 cubes
        cubes["blue"] = 0; //this will initialize every city to have 0 cubes
        cubes["yellow"] = 0; //this will initialize every city to have 0 cubes
        c.setCubes(cubes);
        c.setQuarantined(q); //fix this later
        c.setResearchStation(rs);
        c.setPopulation(pop);
        c.setXBoard(x);
        c.setYBoard(y);
        c.setPlayers(new List<Pawn>()); //set city to have no players in it initially
        c.Activate(name);
        obj.name = name;
        return obj;
    }

    //this should be called after every action?
    public void SetPosition(GameObject obj)
    {
        Pawn p = obj.GetComponent<Pawn>();
        Arrange(p.getLocation());//arrange the pawns around a city nicely
    }


    public void Arrange(City c)
    {
        List<Pawn> ps = c.getPlayers();
        float x = c.getXBoard();
        float y = c.getYBoard();
        int n = ps.Count;
        switch (n)
        {
            case 1:
                ps[0].transform.position = new Vector3(x+10, y, -2.0f);
                break;
            case 2:
                ps[0].transform.position = new Vector3(x - 20, y, -2.0f);
                ps[1].transform.position = new Vector3(x + 20, y, -2.0f); //change xboard and yboard for pawns?
                break;
            case 3:
                ps[0].transform.position = new Vector3(x - 20, y, -2.0f);
                ps[1].transform.position = new Vector3(x + 20, y, -2.0f);
                ps[2].transform.position = new Vector3(x + 20, y + 35, -2.0f);
                break;
            case 4:
                ps[0].transform.position = new Vector3(x - 20, y, -2.0f);
                ps[1].transform.position = new Vector3(x + 20, y, -2.0f);
                ps[2].transform.position = new Vector3(x + 20, y + 35, -2.0f);
                ps[3].transform.position = new Vector3(x - 20, y + 35, -2.0f);
                break;
        }
    }

    //clean this up
    public void ArrangeCards()
    {
        foreach(GameObject go in getPawns())
        {
            Pawn p = go.GetComponent<Pawn>();
            int turn = p.getOrder();
            List<Card> cs = p.getCards();
            float hspace = 80;
            float vspace = 100;

            if (turn==1)
            {
                //top left
                float x = -1000.0f;
                float y = 300.0f;
                int i = 1;
            
                foreach(Card c in cs)
                {
                    String name = c.getName().Replace(" ", String.Empty);
                    Card card = GameObject.Find(name+"Card").GetComponent<Card>();
                    card.transform.position = new Vector3(x, y, -2);
                    x = x + hspace;
                    if (i%3==0)
                    {
                        y = y - vspace;
                        x = -1000.0f;
                    }
                    i = i + 1;
                }
            }else if (turn == 2)
            {
                //top right
                float x = 600.0f;
                float y = 300.0f;
                int i = 1;

                foreach (Card c in cs)
                {
                    String name = c.getName().Replace(" ", String.Empty);

                    Card card = GameObject.Find(name + "Card").GetComponent<Card>();
                    card.transform.position = new Vector3(x, y, -2);
                    x = x + hspace;
                    if (i % 3 == 0)
                    {
                        y = y - vspace;
                        x = 600.0f;
                    }
                    i = i + 1;
                }
            }
            else if (turn == 3)
            {
                //bottom right
                float x = 600.0f;
                float y = -200.0f;
                int i = 1;

                foreach (Card c in cs)
                {
                    String name = c.getName().Replace(" ", String.Empty);
                    Card card = GameObject.Find(name+ "Card").GetComponent<Card>();
                    card.transform.position = new Vector3(x, y, -2);
                    x = x + hspace;
                    if (i%3 == 0)
                    {
                        y = y - vspace;
                        x = 600.0f;

                    }
                    i = i + 1;
                }
            }
            else
            {
                //bottom left
                float x = -1000.0f;
                float y = -200.0f;
                int i = 1;

                foreach (Card c in cs)
                {
                    String name = c.getName().Replace(" ", String.Empty);
                    Card card = GameObject.Find(name + "Card").GetComponent<Card>();
                    card.transform.position = new Vector3(x, y, -2);
                    x = x + hspace;
                    if (i %3== 0)
                    {
                        y = y - vspace;
                        x = -1000.0f;
                    }
                    i = i + 1;
                }
            }
        }
    }

    //this function deals the players' hands
    public void pregame_dealing()
    {
        InitializeDecks();
        List<Card> cdeck = getCitydeck(); //shuffle this
        System.Random random = new System.Random();
        setCitydeck(cdeck.OrderBy(c => random.Next()).ToList());
        cdeck = getCitydeck(); //shuffled
        List<GameObject> players = getPawns();
        int n = players.Count;
        if (n == 4)
        {
            foreach(GameObject player in players)
            {
                Pawn p = player.GetComponent<Pawn>();
                Card card1 = cdeck[0]; 
                cdeck.RemoveAt(0);
                Card card2 = cdeck[0]; 
                cdeck.RemoveAt(0);
                p.setCards(new List<Card>() { card1, card2 });
            }
        }
        else if (n == 3)
        {
            foreach (GameObject player in players)
            {
                Pawn p = player.GetComponent<Pawn>();
                Card card1 = cdeck[0];
                cdeck.RemoveAt(0);
                Card card2 = cdeck[0];
                cdeck.RemoveAt(0);
                Card card3 = cdeck[0];
                cdeck.RemoveAt(0);
                p.setCards(new List<Card>() { card1, card2, card3 });
            }
        }
        else
        {
            foreach (GameObject player in players)
            {
                Pawn p = player.GetComponent<Pawn>();
                Card card1 = cdeck[0];
                cdeck.RemoveAt(0);
                Card card2 = cdeck[0];
                cdeck.RemoveAt(0);
                Card card3 = cdeck[0];
                cdeck.RemoveAt(0);
                Card card4 = cdeck[0];
                cdeck.RemoveAt(0);
                p.setCards(new List<Card>() { card1, card2, card3, card4 });
            }
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn() //change the current player using setter and pawns object in here
    {
        if (turn == 4)
        {
            turn = 1;
        }
        else
        {
            turn += 1;
        }
        setCurrentPlayer(pawns[turn - 1]);
    }

    public void Update()
    {
        GameObject.Find("OutbreakCounter").GetComponent<OutbreakCounter>().setCount(getOutbreaks());
        if (gameOver)
        {
            gameOver = false;
            SceneManager.LoadScene("Game");//start it over
        }
    }

    public void draw_two()
    {
        System.Random random = new System.Random();
        GameObject go = getCurrentPlayer();
        Pawn p = go.GetComponent<Pawn>();
        List<Card> hand = p.getCards();
        List<Card> cdeck = getCitydeck();
        List<Card> cdeck_discard = getCitydeckDiscard();
        Card c1 = cdeck[0];
        cdeck.Remove(cdeck[0]);
        Card c2 = cdeck[0];
        cdeck.Remove(cdeck[0]);
        if(c1.getName().Equals("Epidemic") && c2.getName().Equals("Epidemic"))
        {
            cdeck_discard.Add(c1);
            cdeck_discard.Add(c2);
            setCitydeck(cdeck);
            setCitydeckDiscard(cdeck_discard);
            var sprite = Resources.Load<Sprite>("Epidemic"); //ensure all cities are spelled the same. also check caps
            GameObject temp = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            GameObject temp2 = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);

            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp2.GetComponent<SpriteRenderer>().sprite = sprite;

            temp.name = "EpidemicCard";
            temp2.name = "EpidemicCard";
            //do you want to instantiate them both in the discard pile?

            temp.GetComponent<Card>().transform.position = new Vector3(1,-280,-3);
            temp2.GetComponent<Card>().transform.position = new Vector3(100, -280, -3);
            epidemic();
            epidemic();//infect is then called normally
        }
        else if(!c1.getName().Equals("Epidemic") && c2.getName().Equals("Epidemic")) 
        {
            hand.Add(c1);
            cdeck_discard.Add(c2);
            setCitydeckDiscard(cdeck_discard);
            p.setCards(hand);
            String name = c1.getName();
            name = name.Replace(" ", String.Empty);
            var sprite = Resources.Load<Sprite>(name); //ensure all cities are spelled the same. also check caps
            GameObject temp = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp.name = name + "Card";
            temp.GetComponent<Card>().setName(name);
            var sprite2 = Resources.Load<Sprite>("Epidemic"); //ensure all cities are spelled the same. also check caps
            GameObject temp2 = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp2.GetComponent<SpriteRenderer>().sprite = sprite2;
            temp2.name = "EpidemicCard";
            temp2.GetComponent<Card>().transform.position = new Vector3(250, -280, -3);
            epidemic();       //call single epidemic func
        }
        else if (c1.getName().Equals("Epidemic") && !c2.getName().Equals("Epidemic"))
        {
            hand.Add(c2);
            cdeck_discard.Add(c1);
            setCitydeckDiscard(cdeck_discard);
            p.setCards(hand);
            String name = c2.getName();
            name = name.Replace(" ", String.Empty);
            var sprite = Resources.Load<Sprite>(name); //ensure all cities are spelled the same. also check caps
            GameObject temp = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp.name = name+"Card";
            temp.GetComponent<Card>().setName(name);
            var sprite2 = Resources.Load<Sprite>("Epidemic"); //ensure all cities are spelled the same. also check caps
            GameObject temp2 = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp2.GetComponent<SpriteRenderer>().sprite = sprite2;
            temp2.name = "EpidemicCard";
            temp2.GetComponent<Card>().transform.position = new Vector3(250, -280, -3);
            //call single epidemic func
            epidemic();
        }
        else
        {
            hand.Add(c1);
            hand.Add(c2);
            p.setCards(hand);
            String name = c1.getName();
            String name2 = c2.getName();
            //name.Replace(@"\s+", "");
            name = name.Replace(" ", String.Empty);
            name2 = name2.Replace(" ", String.Empty);
            var sprite = Resources.Load<Sprite>(name); //ensure all cities are spelled the same. also check caps
            var sprite2 = Resources.Load<Sprite>(name2); //ensure all cities are spelled the same. also check caps
            GameObject temp = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp.name = name+"Card";
            temp.GetComponent<Card>().setName(name);
            GameObject temp2 = Instantiate(Card, new Vector3(random.Next(-400, 400), random.Next(-300, 300), -3), Quaternion.identity);
            temp2.GetComponent<SpriteRenderer>().sprite = sprite2;
            temp2.name = name2+"Card";
            temp2.GetComponent<Card>().setName(name2);

        }
        ArrangeCards();
    }

    public void infect_step()
    {
        int num = GameObject.Find("InfectCounter").GetComponent<InfectCounter>().getRate();
        List<Card> ideck = getInfectdeck();
        List<Card> ideck_discard = getInfectdeckDiscard();
        int i = 0;
        while (i <num){
            Card c = ideck[0]; //take the top card
            ideck.Remove(c); //remove it from one deck
            ideck_discard.Add(c); //add it to the discard deck
            String name = c.getName();
            String name2 = name.Replace(" ", String.Empty);
            var sprite = Resources.Load<Sprite>(name2+"EC"); //ensure all cities are spelled the same. also check caps
            GameObject temp = Instantiate(Card, new Vector3(320.0f, 315.0f, -3), Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite; //check if this works
            temp.name = name2 + "InfectCard";
            //how to get city color from card? link card to city?
            City c2 = getCities()[name]; 
            place_cube(c2, 1, c2.getColor(),new List<City>());//place one cube
            display_cubes(c2);//display the number of cubes at that city, color it
            i++;
        }
    }

    public void place_cube(City c, int cubes, string color, List<City> outbreakchain)
    {
        Dictionary<string, int> temp = c.getCubes();
        int current = temp[c.getColor()];

        //temp[c.getColor()] = current + 1;////do this for now
        //c.setCubes(temp);

        List<GameObject> ds = getDiseases();
        //find the correct disease to subtract cubes from
        Disease disease = null; 
        foreach(GameObject d in ds){
            if (d.GetComponent<Disease>().getColor().Equals(color)){
                disease = d.GetComponent<Disease>();}
        }
        if(current+cubes > 3 && c.getQuarantined()==false && !outbreakchain.Contains(c))
        {
            disease.setCubes(disease.getCubes() - 3 - temp[color]);//subtract the amount of cubes to get to 3
            setOutbreaks(getOutbreaks()+1); //add one to the counter
            //move the outbreak counter sprite
            temp[color] = 3;
            c.setCubes(temp);
            outbreakchain.Add(c);
            resolve_outbreak(c, outbreakchain, color);
        }
        else if (outbreakchain.Contains(c)){;} //avoid outbreak cycles
        else if (c.getQuarantined()){;} //avoid quarantined cities
        else
        {
            disease.setCubes(disease.getCubes()-cubes);
            temp[color] = temp[color] + cubes;
            c.setCubes(temp);
        }
    }

    public void resolve_outbreak(City c, List<City> outbreakchain, string color){
        move_outbreakcounter();
        List<City> connections = c.getConnections();
        List<City> acc = new List<City>();
        foreach(City connection in connections){acc.Add(connection);}
        foreach(City c2 in acc) { place_cube(c2,1,color,outbreakchain); display_cubes(c2); }
    }
    public void epidemic() {
        InfectCounter ic = GameObject.Find("InfectCounter").GetComponent<InfectCounter>();
        ic.setIndex(ic.getIndex()+1); //increase the rate sprite
        move_infectcounter(); //get infectcounter object and change its position
        List<Card> ideck = getInfectdeck();
        List<Card> ideckdiscard = getInfectdeckDiscard();
        Card last = ideck[ideck.Count - 1]; //last card
        ideck.Remove(last);
        ideckdiscard.Add(last);
        City city = getCities()[last.getName()];
        place_cube(city,3,city.getColor(),new List<City>());//put 3 cubes on the bottom card
        display_cubes(city);
        System.Random random = new System.Random();
        ideckdiscard.OrderBy(c=>random.Next()).ToList();
        setInfectdeck(infectdeck_discard.Concat(ideck).ToList());//place infeckdeckdiscard on top of infectdeck
        setInfectdeckDiscard(new List<Card>());//empty infectdeckdiscard
        //(infect is then called normally)
    }

    public void move_outbreakcounter()
    {
        Update(); //manually call it?
        GameObject go = GameObject.Find("OutbreakCounter");
        OutbreakCounter oc = go.GetComponent<OutbreakCounter>();
        switch (oc.getCount()){
            case 0:
                break;
            case 1: 
                oc.transform.position = new Vector3(-545,-80,-2.0f); //decrement by 45,35
                break;
            case 2:
                oc.transform.position = new Vector3(-590, -115, -2.0f); //decrement by 45,35
                break;
            case 3:
                oc.transform.position = new Vector3(-545, -150, -2.0f); //decrement by 45,35
                break;
            case 4:
                oc.transform.position = new Vector3(-590, -185, -2.0f); //decrement by 45,35
                break;
            case 5:
                oc.transform.position = new Vector3(-545, -220, -2.0f); //decrement by 45,35
                break;
            case 6:
                oc.transform.position = new Vector3(-590, -255, -2.0f); //decrement by 45,35
                break;
            case 7:
                oc.transform.position = new Vector3(-545, -290, -2.0f); //decrement by 45,35
                break;
            case 8:
                oc.transform.position = new Vector3(-590, -325, -2.0f); //decrement by 45,35
                break;
        }
    }
    public void move_infectcounter() {
        InfectCounter ic = GameObject.Find("InfectCounter").GetComponent<InfectCounter>();
        int count = ic.getIndex();
        switch (count)
        {
            case 0:
                break; //starts at 85,230
            case 1:
                ic.transform.position = new Vector3(125,230,-2.0f);
                break;
            case 2:
                ic.transform.position = new Vector3(165, 230, -2.0f);
                break;
            case 3:
                ic.transform.position = new Vector3(205, 230, -2.0f);
                break;
            case 4:
                ic.transform.position = new Vector3(245, 230, -2.0f);
                break;
            case 5:
                ic.transform.position = new Vector3(285, 230, -2.0f);
                break;
            case 6:
                ic.transform.position = new Vector3(325, 230, -2.0f);
                break;
        }
        
    }
    //clean this up
    public void display_cubes(City c)
    {
        int current = c.getCubes()[c.getColor()];
        try
        {
            c.getCube().GetComponent<SpriteRenderer>().sprite = null;
        }
        catch { }
        GameObject cs = Instantiate(cube, new Vector3(c.getXBoard(), c.getYBoard() - 18, -3), Quaternion.identity);
        SpriteRenderer sr = cs.GetComponent<SpriteRenderer>();
        if (current == 3)
        {
            var s = Resources.Load<Sprite>("3");
            sr.sprite = s;
            sr.color = Color.red;
            cs.transform.localScale = new Vector3(2, 2, 1);
        }
        else if (current == 2)
        {
            var s2 = Resources.Load<Sprite>("2");
            sr.sprite = s2;
            sr.color = Color.yellow;
            cs.transform.localScale = new Vector3(3, 3, 1);

        }
        else if (current == 1)
        {
            var s3 = Resources.Load<Sprite>("1");
            sr.sprite = s3;
            sr.color = Color.green;
            cs.transform.localScale = new Vector3(10, 10, 1);

        }
        else
        {
            var s4 = Resources.Load<Sprite>("x");
            cs.GetComponent<SpriteRenderer>().sprite = s4;
            cs.transform.localScale = new Vector3(5, 5, 1);
        }
        c.setCube(cs);
        cs.GetComponent<Cubes>().setLocation(c);
    }
    public void quarantine_passive() {
        GameObject go = GameObject.Find("QuarantineSpecialist");
        Pawn p = go.GetComponent<Pawn>();
        City c = p.getLocation();
        foreach (KeyValuePair<string, City> kvp in cities)
        {
            kvp.Value.setQuarantined(false);//reset them all in case quarantined was moved
        }
        foreach (City con in c.getConnections())
        {
            con.setQuarantined(true);
        }
        c.setQuarantined(true); 
    }

    public GameObject getCurrentPlayer()
    {
        return currentPlayer;
    }
    public void setCurrentPlayer(GameObject p)
    {
        currentPlayer = p;
    }

    public List<GameObject> getPawns()
    {
        return pawns;
    }
    public void setPawns(List<GameObject> ps)
    {
        pawns = ps;
    }
    public List<GameObject> getDiseases()
    {
        return diseases;
    }
    public void setDiseases(List<GameObject> ds)
    {
        diseases = ds;
    }
    public Dictionary<string, City> getCities()
    {
        return cities;
    }
    public void setCities(Dictionary<string, City> cs)
    {
        cities = cs;
    }
    public void setTurn(int i)
    {
        turn = i;
    }
    public int getTurn()
    {
        return turn;
    }
    public void setAction(int i)
    {
        action = i;
    }
    public int getAction()
    {
        return action;
    }
    public void setCitydeck(List<Card> cd)
    {
        citydeck = cd;
    }
    public List<Card> getCitydeck()
    {
        return citydeck;
    }
    public void setCitydeckDiscard(List<Card> cdd)
    {
        citydeck_discard = cdd;
    }
    public List<Card> getCitydeckDiscard()
    {
        return citydeck_discard;
    }
    public void setInfectdeck(List<Card> id)
    {
        infectdeck = id;
    }
    public List<Card> getInfectdeck()
    {
        return infectdeck;
    }
    public void setInfectdeckDiscard(List<Card> idd)
    {
        infectdeck_discard = idd;
    }
    public List<Card> getInfectdeckDiscard()
    {
        return infectdeck_discard;

    }
    public void setEventcards(List<Card> ec)
    {
        eventcards = ec;
    }
    public List<Card> getEventcards()
    {
        return eventcards;
    }
    public void setOutbreaks(int c)
    {
        outbreaks = c;
    }
    public int getOutbreaks()
    {
        return outbreaks;
    }
    public int getInfectRate() { 
        return infectrate; }
    
    public void setInfectRate(int i) { 
        infectrate = i; }
    public void setCarcdclicked(bool b)
    {
        cardclicked = b;
    }
    public void getCardclicked()
    {
        return cardclicked;
    }
}
