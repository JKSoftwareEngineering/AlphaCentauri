using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class MothershipAI : MonoBehaviour
{
    [HideInInspector] public MotherShip localMothership;
    [SerializeField] GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // this is only nessisary so the mothership can take damage
        localMothership = (MotherShip)DestroyTheEnemyFleetGE.motherShip;
        localMothership.explosionPrefab = explosionPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
