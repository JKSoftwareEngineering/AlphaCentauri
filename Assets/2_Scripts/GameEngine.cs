using System.Collections.Generic;
using UnityEngine;
using InfoDump;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject arrowIcon;
    [SerializeField] private GameObject Canvase;
    [SerializeField] private GameObject motherShipPrefab;
    [SerializeField] private GameObject flagShipPrefab;
    //[SerializeField] private GameObject alliedPrefab;
    //[SerializeField] private GameObject rocketPrefab;
    //[SerializeField] private GameObject flagshipPrefab;
    //[SerializeField] private GameObject mothershipPrefab;
    //int AlliedCount = 0;
    public static Player p;
    public static MotherShip motherShip;
    public static GameObject mothershipAnchor;
    public static Flagship flagShip;
    public static GameObject flagshipBaseAnchor;
    public static GameObject mothershipArrow;
    public static GameObject flagshipArrow;

    // all of our enemies are listed here
    public static List<Enemy> enemy = new List<Enemy>();
    // up to 20 arrows can be active at any one time
    public static GameObject[] enemyArrow = new GameObject[20];
    // all of our allies are listed here
    public static List<Allied> allied = new List<Allied>();
    // up to 20 arrows can be active at any one time
    public static GameObject[] alliedArrow = new GameObject[20];
    //public static GameObject MainEnemy;

    // Start is called before the first frame update
    void Start()
    {
        p = new Player(playerPrefab);
        motherShip = new MotherShip(motherShipPrefab);
        flagShip = new Flagship(flagShipPrefab);
        // generate enemy arrows
        for(int i = 0; i < enemyArrow.Length; i++)
        {
            GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
            arrow.transform.SetParent(Canvase.transform);
            arrow.transform.SetAsFirstSibling();
            arrow.name = "EnemyArrow";
            enemyArrow[i] = arrow;
        }
        // generate allied arows
        for (int i = 0; i < alliedArrow.Length; i++)
        {
            GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
            arrow.transform.SetParent(Canvase.transform);
            arrow.transform.SetAsFirstSibling();
            arrow.name = "AlliedArrow";
            alliedArrow[i] = arrow;
        }
        // set all the arows to be inactive
        foreach (GameObject arrow in enemyArrow)
        {
            arrow.SetActive(false);
        }
        foreach (GameObject arrow in alliedArrow)
        {
            arrow.SetActive(false);
        }
        // create a special arrow for the mothership
        mothershipArrow = Instantiate(arrowIcon, Canvase.transform);
        mothershipArrow.transform.SetParent(Canvase.transform);
        mothershipArrow.transform.SetAsFirstSibling();
        mothershipArrow.name = "MotherShipArrow";
        mothershipArrow.GetComponent<Image>().color = new Vector4(0, 1, 1, 1);
        // create a special arro for the flagship
        flagshipArrow = Instantiate(arrowIcon, Canvase.transform);
        flagshipArrow.transform.SetParent(Canvase.transform);
        flagshipArrow.transform.SetAsFirstSibling();
        flagshipArrow.name = "FlagshipShipArrow";
        flagshipArrow.GetComponent<Image>().color = new Vector4(1, 0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // assign the arrow to follow spacial ship types
        if (flagShip.g != null)
        {
            if (!flagShip.g.GetComponent<BasicPersuit>().active)
            {
                flagShip.t.GetChild(0).gameObject.GetComponent<ScreenPos>().thisShipArrow = flagshipArrow;
                flagShip.t.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                flagshipArrow.SetActive(false);
            }
        }
        motherShip.t.GetChild(0).gameObject.GetComponent<ScreenPos>().thisShipArrow = mothershipArrow;
        motherShip.t.GetChild(0).gameObject.SetActive(true);
    }
}
