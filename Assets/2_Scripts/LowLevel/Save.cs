using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /* Method: SaveFile
     * Purpose: Create a save file if we dont already have one
     * Restrictions: None
     */
    public void SaveFile()
    {
        string path = Application.dataPath + "AlphaCentauriSave.txt";
        if (!File.Exists(path))
        {
            string s = "Tutorial";
            File.AppendAllText(path, s);
        }
    }
}
