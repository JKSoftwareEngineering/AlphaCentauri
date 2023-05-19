using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InfoDump;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DestroyTheEnemyFleetGE : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MissionType;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject alliedSpawner;
    [SerializeField] private GameObject Canvase;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject alliedDeralict;
    [SerializeField] private GameObject crabDeralict;
    [SerializeField] private GameObject commet;
    [SerializeField] private TextMeshProUGUI defendTimerDisplay;

    // 0 = destroy the enemy Fleet
    // 1 = destroy the flagship
    // 2 = protect the mothership
    int gameMode = 0;

    float DefendTimer = 0;
    float DefendTimerMax = 120;
    // Note: this is what other scripts reference to know what level you are on
    public int storyIndex = -1;

    public static Player p;
    public static Ship motherShip;
    public static Ship flagShip;
    public static bool gameLost = false;
    public static bool gameWon = false;
    public static string StoryLevel = "";

    [SerializeField] private GameObject arrowIcon;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject flagshipPrefab;
    [SerializeField] private GameObject mothershipPrefab;
    [SerializeField] private GameObject alliedPrefab;

    [HideInInspector] public static List<Enemy> enemy = new List<Enemy>();
    [HideInInspector] public static List<Allied> allied = new List<Allied>();
    [HideInInspector] public static List<GameObject> arrows = new List<GameObject>();

    int enemyCount;
    int alliedCount;

    float cometTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // load the main file
        LoadLevel();
        // save updated data back to the file
        AutoSave();
        // create a player
        p = new Player(playerPrefab);
        // create all of the deralicts
        for (int i = 0; i < 100; i++)
        {
            Instantiate(alliedDeralict, new Vector3(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150)), Quaternion.Euler(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150)));
            Instantiate(crabDeralict, new Vector3(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150)), Quaternion.Euler(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150)));
        }
        // 0 = destroy the enemy Fleet
        if (gameMode == 0)
        {
            // no special spawns for mode 0
            MissionType.text = "Destroy the\nenemy fleat";
        }
        // 1 = destroy the flagship
        if (gameMode == 1)
        {
            // special spawn a flagship
            CreateFlagship();
            MissionType.text = "Destroy the/nenemy flagship";
        }
        // 2 = protect the mothership
        if (gameMode == 2)
        {
            // special spawn a mothership
            CreateMothership();
            MissionType.text = "Defend the/nmothership";
        }
        // create all of the enemys
        for(int i = 0; i < enemyCount; i++)
        {
            CreateEnemy();
        }
        // create all of the allies
        for (int i = 0; i < alliedCount; i++)
        {
            CreateAllied();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // create new comets in intervals of 5 seconds
        cometTimer += Time.deltaTime;
        if(cometTimer >= 5)
        {
            cometTimer = 0;
            for (int i = 0; i < 15; i++)
            {
                // destroy after 5 seconds
                Destroy(Instantiate(commet, new Vector3(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150)), Quaternion.Euler(Random.Range(150, -150), Random.Range(150, -150), Random.Range(150, -150))), 5);
            }
        }
        // if we are in game mode 0 display the number of enemys left
        if (gameMode == 0)
        {
            defendTimerDisplay.text = "" + enemy.Count;
            // if there are no more enemys then you win
            if (enemy.Count == 0)
            {
                gameWon = true;
            }
            // if you die you lose
            if(!p.Alive)
            {
                gameLost = true;
            }
        }
        // if we are in game mode 1
        if (gameMode == 1)
        {
            // keep the number of allies at the allied count
            if(allied.Count < alliedCount)
            {
                CreateAllied();
            }
            // keep the number of enemys at the enemy count
            if(enemy.Count < enemyCount)
            {
                CreateEnemy();
            }
            // if the flagship is destroyed you win
            if(flagShip.HealthCur <= 0)
            {
                gameWon = true;
            }
            // if you die for any reason you lose
            if (!p.Alive)
            {
                gameLost = true;
            }
        }
        // we are in game mode 2
        if (gameMode == 2)
        {
            // count down how long the player has to aid in defending the mothership
            defendTimerDisplay.text = "" + (DefendTimerMax - (int)DefendTimer);
            DefendTimer += Time.deltaTime;
            // if the timer reaches 0 you win
            if(DefendTimer >= DefendTimerMax)
            {
                gameWon = true;
            }
            // keep the number of allies at the allied count
            if (allied.Count < alliedCount)
            {
                CreateAllied();
            }
            // keep the number of enemys at the enemy count
            if (enemy.Count < enemyCount)
            {
                CreateEnemy();
            }
            // if the mothership is destroyed you lose
            if(motherShip.HealthCur <= 0)
            {
                gameLost = true;
            }
            // if you die for any reason you lose
            if (!p.Alive)
            {
                gameLost = true;
            }
        }
        // win lose conditions
        // return to the main menu
        if(gameLost)
        {
            gameLost = false;
            SceneManager.LoadScene(1);
        }
        // go to the next mission
        if(gameWon)
        {
            gameWon = false;
            SceneManager.LoadScene(2);
            // incrament the index
            storyIndex++;
            // save the updated mission
            AutoSave();
        }
    }
    /* Method: CreateEnemy
     * Purpose: Generate a enemy and assign the data
     * Restrictions: None
     */
    void CreateEnemy()
    {
        // spawn position
        Vector3 randomVector = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), enemySpawner.transform.position.z);
        // create an arrow on the canvase
        GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
        // create the prefab at the spawn position
        GameObject localEnemyPrefab = Instantiate(enemyPrefab, randomVector, Quaternion.identity);
        // assign the arrow to the the ships position in world space
        localEnemyPrefab.GetComponent<ScreenPos>().thisShipArrow = arrow;
        // have the arrow track the ship
        localEnemyPrefab.GetComponent<ScreenPos>().enabled = true;
        // create enemy data 
        Enemy localEnemy = new Enemy(localEnemyPrefab);
        // set this enemy data to the AI that will govern it
        localEnemyPrefab.GetComponent<EnemyAI>().localEnemy = localEnemy;
        // activate the AI
        localEnemyPrefab.GetComponent<EnemyAI>().enabled = true;
        // Assign the arrow to the enemy data
        localEnemy.arrow = arrow;
        // color the arrow to red                       r  g  b  a
        arrow.GetComponent<Image>().color = new Vector4(1, 0, 0, 1);
        // set the arrow on the canvase
        arrow.transform.SetParent(Canvase.transform);
        // set the arrow behind the viewport on the canvase
        arrow.transform.SetAsFirstSibling();
        // name the arrow (mostly for testing)
        arrow.name = "EnemyArrow";
        // bind this arrow to the data arrow reference
        localEnemy.arrow = arrow;
        // add the arrow to the arrow list for easy storage
        arrows.Add(arrow);
        // add the enemy to the list of avalable enemys
        enemy.Add(localEnemy);
    }
    /* Method: CreateFlagship
     * Purpose: Generate a Flagship and assign the data
     * Restrictions: None
     */
    void CreateFlagship()
    {
        Vector3 randomVector = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), enemySpawner.transform.position.z);
        GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
        GameObject localEnemyPrefab = Instantiate(flagshipPrefab, randomVector, Quaternion.identity);
        localEnemyPrefab.GetComponent<ScreenPos>().thisShipArrow = arrow;
        localEnemyPrefab.GetComponent<ScreenPos>().enabled = true;
        Flagship localEnemy = new Flagship(localEnemyPrefab);
        localEnemyPrefab.GetComponent<FlagshipAI>().localFlagship = localEnemy;
        localEnemyPrefab.GetComponent<FlagshipAI>().enabled = true;
        localEnemy.arrow = arrow;
        //                                              r  g  b  a
        arrow.GetComponent<Image>().color = new Vector4(1, 0, 1, 1);
        arrow.transform.SetParent(Canvase.transform);
        arrow.transform.SetAsFirstSibling();
        arrow.name = "EnemyArrow";
        localEnemy.arrow = arrow;
        arrows.Add(arrow);
        flagShip = localEnemy;
    }
    /* Method: CreateMothership
     * Purpose: Generate a Mothership and assign the data
     * Restrictions: None
     */
    void CreateMothership()
    {
        Vector3 randomVector = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100,100));
        GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
        GameObject localMothershipPrefab = Instantiate(mothershipPrefab, randomVector, Quaternion.identity);
        localMothershipPrefab.GetComponent<ScreenPos>().thisShipArrow = arrow;
        localMothershipPrefab.GetComponent<ScreenPos>().enabled = true;
        MotherShip localAllied = new MotherShip(localMothershipPrefab);
        localMothershipPrefab.GetComponent<MothershipAI>().localMothership = localAllied;
        localMothershipPrefab.GetComponent<MothershipAI>().enabled = true;
        localAllied.arrow = arrow;
        //                                              r  g  b  a
        arrow.GetComponent<Image>().color = new Vector4(0, 1, 1, 1);
        arrow.transform.SetParent(Canvase.transform);
        arrow.transform.SetAsFirstSibling();
        arrow.name = "MothershipArrow";
        localAllied.arrow = arrow;
        arrows.Add(arrow);
        motherShip = localAllied;
    }
    /* Method: CreateAllied
     * Purpose: Generate a ally and assign the data
     * Restrictions: None
     */
    void CreateAllied()
    {
        Vector3 randomVector = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), alliedSpawner.transform.position.z);
        GameObject arrow = Instantiate(arrowIcon, Canvase.transform);
        GameObject localAlliedPrefab = Instantiate(alliedPrefab, randomVector, Quaternion.identity);
        localAlliedPrefab.GetComponent<ScreenPos>().thisShipArrow = arrow;
        localAlliedPrefab.GetComponent<ScreenPos>().enabled = true;
        Allied localAllied = new Allied(localAlliedPrefab);
        localAlliedPrefab.GetComponent<AlliedAI>().localAllied = localAllied;
        localAlliedPrefab.GetComponent<AlliedAI>().enabled = true;
        localAllied.arrow = arrow;

        //                                              r, g, b, a
        arrow.GetComponent<Image>().color = new Vector4(0, 1, 0, 1);
        arrow.transform.SetParent(Canvase.transform);
        arrow.transform.SetAsFirstSibling();
        arrow.name = "AlliedArrow";
        localAllied.arrow = arrow;
        arrows.Add(arrow);
        allied.Add(localAllied);
    }
    /* Method: AutoSave
     * Purpose: Save the current mission to the save file
     * Restrictions: None
     */
    public void AutoSave()
    {
        // storyProgession
        // this is what will be saved in the save file
        if (storyIndex == (int)List.story.tutorial)
        {
            StoryLevel = "Tutorial";
        }
        else if (storyIndex == (int)List.story.firstContact)
        {
            StoryLevel = "First Contact";
        }
        else if (storyIndex == (int)List.story.crabsAttackSol)
        {
            StoryLevel = "Crabs Attack Sol";
        }
        else if (storyIndex == (int)List.story.fightAroundSaturn)
        {
            StoryLevel = "Fight Around Saturn";
        }
        else if (storyIndex == (int)List.story.fightAtTheAsteroidBelt)
        {
            StoryLevel = "Fight At The Asteroid Belt";
        }
        else if (storyIndex == (int)List.story.fightBetweenEarthAndMars)
        {
            StoryLevel = "Fight Between Earth And Mars";
        }
        else if (storyIndex == (int)List.story.secondFightAtTheAsteroidBelt)
        {
            StoryLevel = "Second Fight At The Asteroid Belt";
        }
        else if (storyIndex == (int)List.story.fightAtPluto)
        {
            StoryLevel = "Fight At Pluto";
        }
        else if (storyIndex == (int)List.story.fightAtLxion)
        {
            StoryLevel = "Fight At Lxion";
        }
        else if (storyIndex == (int)List.story.fightAtChrion)
        {
            StoryLevel = "Fight At Chrion";
        }
        // file location
        string path = Application.dataPath + "AlphaCentauriSave.txt";
        // if no file is found
        if (!File.Exists(path))
        {
            string s = StoryLevel;
            File.AppendAllText(path, s);
        }
        // if there is a file clear it and save the new data
        if (File.Exists(path))
        {
            File.WriteAllText(path, "");
            string s = StoryLevel;
            File.AppendAllText(path, s);
        }
    }
    /* Method: LoadLevel
     * Purpose: Load the current mission from the save file
     * Restrictions: None
     */
    public void LoadLevel()
    {
        // make sure all of the lists are empty
        enemy.Clear();
        allied.Clear();
        arrows.Clear();
        // file location
        string path = Application.dataPath + "AlphaCentauriSave.txt";
        // if the file exists
        if (File.Exists(path))
        {
            FileInfo f = new FileInfo(path);
            StreamReader read = f.OpenText();
            
            string sIn = read.ReadLine();
            StoryLevel = sIn;
            read.Close();

        }
        // if the file dosnt exist set it to the tutorial
        if (!File.Exists(path))
        {
            string s = "Tutorial";
            File.AppendAllText(path, s);
        }
        // load the correct level
        if (StoryLevel.Equals("Tutorial"))
        {
            // set the story index
            storyIndex = 0;
            // set the game Mode
            gameMode = 0;
            // set the enemy count
            enemyCount = 1;
            // set the allied count
            alliedCount = 0;
        }
        else if (StoryLevel.Equals("First Contact"))
        {
            storyIndex = 1;
            gameMode = 0;
            enemyCount = 20;
            alliedCount = 20;
        }
        else if (StoryLevel.Equals("Crabs Attack Sol"))
        {
            storyIndex = 2;
            gameMode = 1;
            enemyCount = 40;
            alliedCount = 40;
        }
        else if (StoryLevel.Equals("Fight Around Saturn"))
        {
            storyIndex = 3;
            gameMode = 1;
            enemyCount = 30;
            alliedCount = 20;
        }
        else if (StoryLevel.Equals("Fight At The Asteroid Belt"))
        {
            storyIndex = 4;
            gameMode = 1;
            enemyCount = 20;
            alliedCount = 15;
        }
        else if (StoryLevel.Equals("Fight Between Earth And Mars"))
        {
            storyIndex = 5;
            gameMode = 2;
            enemyCount = 20;
            alliedCount = 25;
        }
        else if (StoryLevel.Equals("Second Fight At The Asteroid Belt"))
        {
            storyIndex = 6;
            gameMode = 2;
            enemyCount = 30;
            alliedCount = 30;
        }
        else if (StoryLevel.Equals("Fight At Pluto"))
        {
            storyIndex = 7;
            gameMode = 0;
            enemyCount = 40;
            alliedCount = 40;
        }
        else if (StoryLevel.Equals("Fight At Lxion"))
        {
            storyIndex = 8;
            gameMode = 0;
            enemyCount = 50;
            alliedCount = 50;
        }
        else if (StoryLevel.Equals("Fight At Chrion"))
        {
            storyIndex = 9;
            gameMode = 0;
            enemyCount = 70;
            alliedCount = 70;
        }
    }
}
