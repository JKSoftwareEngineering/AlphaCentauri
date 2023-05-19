//---------------------------------------
//  This script is depreciated but is kept as a reference
//---------------------------------------

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using InfoDump;
//using UnityEngine.UI;
//
//public class ShipCreator : MonoBehaviour
//{
//    // Start is called before the first frame update
//    [SerializeField] GameObject GE;
//    [SerializeField] GameObject explosion;
//    GameObject arrow;
//    public Ship localShip;
//    private bool firstCycle = true;
//    bool shipActive;
//    public int shipIndex;
//
//    //void Start()
//    //{}
//
//    // Update is called once per frame
//    void Update()
//    {
//        if(firstCycle)
//        {
//            if (shipIndex == (int)List.index.enemyBasic)
//            {
//                localShip = new Enemy(gameObject);
//                GameEngine.enemy.Add((Enemy)localShip);
//                shipActive = true;
//                for(int i = 0; i < GameEngine.enemy.Count; i++)
//                {
//                    if (!GameEngine.enemyArrow[i].activeInHierarchy)
//                    {
//                        GameEngine.enemyArrow[i].SetActive(true);
//                        arrow = GameEngine.enemyArrow[i];
//                        break;
//                    }
//                }
//                arrow.GetComponent<Image>().color = new Vector4(1, 0, 0, 1);
//            }
//            if (shipIndex == (int)List.index.alliedShip)
//            {
//                localShip = new Allied(gameObject);
//                GameEngine.allied.Add((Allied)localShip);
//                shipActive = true;
//                for (int i = 0; i < GameEngine.allied.Count; i++)
//                {
//                    if (!GameEngine.alliedArrow[i].activeInHierarchy)
//                    {
//                        GameEngine.alliedArrow[i].SetActive(true);
//                        arrow = GameEngine.alliedArrow[i];
//                        break;
//                    }
//                }
//                arrow.GetComponent<Image>().color = new Vector4(0, 1, 0, 1);
//            }
//            if (shipIndex == (int)List.index.enemyFlagship)
//            {
//                localShip = new Flagship(gameObject);
//                shipActive = true;
//            }
//            if (shipIndex == (int)List.index.motherShip)
//            {
//                localShip = new MotherShip(gameObject);
//                shipActive = true;
//            }
//            if (shipIndex == (int)List.index.enemyDeralict)
//            {
//                localShip = new Enemy(gameObject);
//                shipActive = false;
//            }
//            if (shipIndex == (int)List.index.alliedDeralict)
//            {
//                localShip = new Allied(gameObject);
//                shipActive = false;
//            }
//            localShip.arrow = arrow;
//            localShip.explosionPrefab = explosion;
//            ///
//            /// target is assigned via random selection of remaining ships and will be realitive to the ship type themselves
//            ///
//            firstCycle = false;
//        }
//    }
//}
//