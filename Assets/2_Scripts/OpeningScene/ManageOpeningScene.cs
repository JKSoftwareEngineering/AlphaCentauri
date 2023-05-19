using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ManageOpeningScene : MonoBehaviour
{
    bool infoActive;
    bool loadActive;
    bool mainActive;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject infoMenu;
    [SerializeField] private GameObject loadMenu;

    void Start()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // set the menu to the defult state
        ResetMenu();
        mainActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // manage what menu is open at the time
        if (infoActive)
        {
            infoMenu.SetActive(true);
        }
        else if (loadActive)
        {
            loadMenu.SetActive(true);
        }
        else if (mainActive)
        {
            mainMenu.SetActive(true);
        }
    }
    /* Method: ResetMenu
     * Purpose: Set the menu to the defult state
     * Restrictions: None
     */
    private void ResetMenu()
    {
        infoActive = false;
        loadActive = false;
        mainActive = false;
        mainMenu.SetActive(false);
        infoMenu.SetActive(false);
        loadMenu.SetActive(false);
    }
    /* Method: Back
     * Purpose: Set the menu to the defult state
     * Restrictions: None
     */
    public void Back()
    {
        ResetMenu();
        mainActive = true;
    }
    /* Method: Load
     * Purpose: Set the menu to the load game state
     * Restrictions: None
     */
    public void Load()
    {
        ResetMenu();
        loadActive = true;
    }
    /* Method: LoadOldGame
     * Purpose: load the next scene
     * Restrictions: None
     */
    public void LoadOldGame()
    {
        SceneManager.LoadScene(2);
    }
    /* Method: Info
     * Purpose: Set the menu to the info state
     * Restrictions: None
     */
    public void Info()
    {
        ResetMenu();
        infoActive = true;
    }
    /* Method: Play
     * Purpose: Set the save file to a new game state and load the next scene
     * Restrictions: None
     */
    public void Play()
    {
        NewGame();
        SceneManager.LoadScene(2);
    }
    /* Method: Exit
     * Purpose: Exit the game
     * Restrictions: None
     */
    public void Exit()
    {
        Application.Quit();
    }
    /* Method: Exit
     * Purpose: Set the save file to a new game state
     * Restrictions: None
     */
    void NewGame ()
    {
        string path = Application.dataPath + "AlphaCentauriSave.txt";
        // if we dont have a save file
        if (!File.Exists(path))
        {
            string s = "Tutorial";
            File.AppendAllText(path, s);
        }
        // if we do have a save file
        if (File.Exists(path))
        {
            File.WriteAllText(path, "");//this removes all saved data
            string s = "Tutorial" + "\n";
            File.AppendAllText(path, s);
        }
    }

}
