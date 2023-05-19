using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;
using UnityEngine.UI;

public class BasicAliedPersuit : MonoBehaviour
{
    //---------------------------------------
    //  This script is depreciated but is kept as a reference
    //---------------------------------------




    //private bool firstCycle = true;
    //[SerializeField] GameObject GE;
    //[SerializeField] GameObject Explosion;
    //public Allied localAllied;
    //GameObject arrow;
    //
    //bool thrusterOn = false;
    //
    //// Start is called before the first frame update
    //void Start()
    //{
    //    localAllied = new Allied(gameObject);
    //    localAllied.explosionPrefab = Explosion;
    //    GameEngine.allied.Add(localAllied);
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //    if(thrusterOn)
    //    {
    //        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    //        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    //        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    //    }
    //    if (firstCycle)
    //    {
    //        for (int i = 0; i < 20; i++)
    //        {
    //            if (!GameEngine.alliedArrow[i].activeInHierarchy)
    //            {
    //                GameEngine.alliedArrow[i].SetActive(true);
    //                arrow = GameEngine.alliedArrow[i];
    //                break;
    //            }
    //        }
    //        localAllied.arrow = arrow;
    //        arrow.GetComponent<Image>().color = new Vector4(0, 1, 0, 1);
    //        firstCycle = false;
    //        localAllied.t.GetChild(0).gameObject.SetActive(true);
    //        localAllied.t.GetChild(0).gameObject.GetComponent<ScreenPos>().thisShipArrow = arrow;
    //    }
    //    if(localAllied.target == null)
    //    {
    //            foreach (Enemy enemy in GameEngine.enemy)
    //            {
    //                if (enemy.g != null)
    //                {
    //                    if (enemy.g.GetComponent<BasicPersuit>().active)
    //                    {
    //                        localAllied.target = enemy;
    //                    }
    //                }
    //            }
    //        //search for target
    //        thrusterOn = false;
    //    }
    //    else
    //    {
    //        try
    //        {
    //            transform.LookAt(localAllied.target.t.position);
    //            localAllied.rig.MovePosition(localAllied.t.position + (localAllied.t.forward * 12) * Time.deltaTime);
    //        }
    //        catch
    //        {
    //            localAllied.target = null;
    //        }
    //        //move tword target
    //        thrusterOn = true;
    //    }
    //}
}
