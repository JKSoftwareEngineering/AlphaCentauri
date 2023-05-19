using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public TextMeshProUGUI MissionName;
    [SerializeField] private Button loadButton;
    // Start is called before the first frame update
    void Start()
    {
        LoadFile();
        // if they are on the tutorial then set the load button to inactive
        if(MissionName.text.Equals("Tutorial"))
        {
            loadButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /* Method: LoadFile
     * Purpose: Load the main save file
     * Restrictions: None
     */
    public void LoadFile()
    {
        string path = Application.dataPath + "AlphaCentauriSave.txt";
        // if we have a file load it
        if (File.Exists(path))
        {
            FileInfo f = new FileInfo(path);
            StreamReader read = f.OpenText();

            string sIn = read.ReadLine();
            MissionName.text = sIn;
            read.Close();

        }
        // if we dont have a file then set the mission to the tutorial
        if (!File.Exists(path))
        {
            string s = "Tutorial";
            File.AppendAllText(path, s);
        }
    }
}
