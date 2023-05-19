using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    private bool pause = true;
    private bool firstCycle = true;
    GameObject[] inScene;
    public GameObject pauseCam;
    public GameObject pauseCanvase;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject gameContainer;
    [SerializeField] private GameObject pauseText;
    [SerializeField] private TextMeshProUGUI intro;
    [SerializeField] private GameObject introText;

    void Start()
    {
        eventSystem.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(firstCycle)
        {
            Intro();
            firstCycle = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if (pause)
            {
                PauseGame();
            }
            if (!pause)
            {
                UnPause();
            }
        }
    }
    public void UnPause()
    {
        introText.SetActive(false);
        pauseText.SetActive(true);
        // lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // make it invisable so its not anoying
        Cursor.visible = false;
        pause = !pause;
        foreach (GameObject g in inScene)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        pauseCam.SetActive(false);
        pauseCanvase.SetActive(false);
    }
    public void PauseGame()
    {
        // lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.None;
        // make it invisable so its not anoying
        Cursor.visible = true;
        inScene = FindObjectsOfType<GameObject>();
        foreach (GameObject g in inScene)
        {
            if (!g.name.Equals("Pause"))
            {
                g.SetActive(false);
            }
        }
        pauseCam.SetActive(true);
        pauseCanvase.SetActive(true);
        eventSystem.SetActive(true);
        gameManager.SetActive(true);
        //gameContainer.SetActive(true);
    }
    public void MainMenu()
    {
        UnPause();
        DestroyTheEnemyFleetGE.gameLost = true;
    }
    public void Desktop()
    {
        UnPause();
        gameManager.GetComponent<DestroyTheEnemyFleetGE>().AutoSave();
        Application.Quit();
    }
    public void Intro()
    {
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 0)
        {
            intro.text = "Ah my first patrol, I joined the earth space force about 3 years ago hoping to get right out and explore some of the universe and see new worlds with my own eyes.  I didn’t expect it to so long before I finally got out here.  We have been getting reports of mysterious objects shaped like crabs flying around by the locals... Probably just a strangely shaped comet flying around.  But if not that’s why we have lasers.  After some flying, I came across a ship graveyard.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 1)
        {
            intro.text = "After my first skirmish I was reassigned to join the 3rd fleet.  Apparently my first experience with whatever that was wasn’t unique.  But this time there might be a lot of them, but there’s a lot of us too.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 2)
        {
            intro.text = "We won the day, but we have been getting reports of the 2nd fleet being pushed back toward sol when they engaged a new enemy... a big one this time.  We are turning back to give them support.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 3)
        {
            intro.text = "We won the day, but we have received reports of the 1st fleet is having the same troubles.  We are becoming crab slaying machines... I think we have this covered.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 4)
        {
            intro.text = "There still pushing us back It seems like there’s no end to their numbers one of their flagships managed to get through the blockade and 3rd fleet is the closest to deal with it.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 5)
        {
            intro.text = "It’s time to bring the fight to them.  Fleet command has been able to track them back to their home planet Chrion in the Alpha Centauri System.  We are mobilizing a gigantic home base affectionately nicknamed the mothership as a mobile combat platform so our main ground force can reach their home planet safely.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 6)
        {
            intro.text = "We are preparing to leave sol and they have started to target the mothership with greater force.  Defend it or there will be no ground forces once we reach the home planet.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 7)
        {
            intro.text = "Our scout has reported swarms of enemy ships as far as the eye can see.  1st fleet is going to stay back ad protect the mothership as the 2nd and 3rd push forward to try to clear the area ahead of the mothership.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 8)
        {
            intro.text = "It seems that the closer we get to Chrion the more of them there is.  We are passing Lxion, Chrions neighbor.  After a few losses 1st fleet has moved up to support us in the coming fight.";
        }
        if (gameManager.GetComponent<DestroyTheEnemyFleetGE>().storyIndex == 9)
        {
            intro.text = "Here we are Chrion.  All hand on deck and everyone to their ships.  If we can take the sky’s our mothership will be able to land and our combined air and land assault will win the day, if we can’t then we might not get another chance.";
        }
        PauseGame();
        introText.SetActive(true);
        pauseText.SetActive(false);
    }
}
