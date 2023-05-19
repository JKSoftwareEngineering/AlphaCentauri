using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class FlagshipAI : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public Flagship localFlagship;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject LazerBurn;
    [SerializeField] GameObject Lazer;
    private float newTargetTimer = 0f;
    float distance;
    RaycastHit lazerHit;
    [SerializeField] GameObject LazerGameObject;
    void Start()
    {
        localFlagship.explosionPrefab = explosionPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        // if we dont have a target
        if (localFlagship.target == null)
        {
            try
            {
                // set a maximum distance
                distance = Mathf.Infinity;
                // find the closest allied ship and set it as the target
                foreach (Allied allied in DestroyTheEnemyFleetGE.allied)
                {
                    if (Vector3.Distance(localFlagship.t.position, allied.t.position) < distance)
                    {
                        distance = Vector3.Distance(localFlagship.t.position, allied.t.position);
                        localFlagship.target = allied;
                    }
                }
                if (Vector3.Distance(localFlagship.t.position, DestroyTheEnemyFleetGE.p.t.position) < distance)
                {
                    localFlagship.target = DestroyTheEnemyFleetGE.p;
                }
            }
            catch
            {
                localFlagship.target = null;
            }
        }
        // we have a target
        else
        {
            newTargetTimer += Time.deltaTime;
            // every 5 seconds search for a new target
            if (newTargetTimer >= 5f)
            {
                localFlagship.target = null;
                newTargetTimer = 0f;
            }
            // double chect to make sure that someone else didnt kill our target before we could
            try
            {
                // if we are moving out of the map go toward wherever the player is
                if (Vector3.Distance(localFlagship.t.position, DestroyTheEnemyFleetGE.p.t.position) > 75)
                {
                    localFlagship.target = DestroyTheEnemyFleetGE.p;
                    transform.LookAt(DestroyTheEnemyFleetGE.p.t);
                }
                // face the target
                transform.LookAt(localFlagship.target.t);
                // if we are outside weapons range
                if (Vector3.Distance(localFlagship.t.position, localFlagship.target.t.position) >= localFlagship.WeaponsRange)
                {
                    LazerGameObject.SetActive(false);
                    localFlagship.MoveForward();
                }
                if (Vector3.Distance(localFlagship.t.position, localFlagship.target.t.position) <= 50)// localFlagship.WeaponsRange)
                {// if we are in weapons range
                    if (localFlagship.target != DestroyTheEnemyFleetGE.p)
                    {
                        // if the target is destroyed
                        if (!localFlagship.target.g.GetComponent<AlliedAI>().localAllied.TakeDamage(localFlagship.damageMain * Time.deltaTime))
                        {
                            newTargetTimer = 0;
                            DestroyTheEnemyFleetGE.allied.Remove((Allied)localFlagship.target);
                        }
                        // if the target is not destroyed
                        else
                        {
                            Ray ray = new Ray(Lazer.transform.position, Lazer.transform.TransformDirection(Vector3.forward));
                            if (Physics.Raycast(ray, out lazerHit))
                            {
                                LazerGameObject.SetActive(true);
                                GameObject g = Instantiate(LazerBurn, lazerHit.point, Quaternion.LookRotation(lazerHit.normal));
                                Destroy(g, .2f);
                            }
                        }
                    }
                    // if we are targeting the player
                    else
                    {
                        Ray ray = new Ray(Lazer.transform.position, Lazer.transform.TransformDirection(Vector3.forward));
                        if (Physics.Raycast(ray, out lazerHit))
                        {
                            LazerGameObject.SetActive(true);
                            GameObject g = Instantiate(LazerBurn, lazerHit.point, Quaternion.LookRotation(lazerHit.normal));
                            DestroyTheEnemyFleetGE.p.TakeDamage(localFlagship.damageMain * Time.deltaTime);
                            Destroy(g, .2f);
                        }
                    }
                }
                {// if we are under attack

                }
                {// if we are attacking

                    {// if we are alone
                        {// do we have rockets

                        }
                    }
                    {// if we are not alone
                        {// do we have rockets

                        }

                    }
                }
            }
            catch
            {
                localFlagship.target = null;
                // if the player is the only target left
                if (DestroyTheEnemyFleetGE.allied.Count == 0)
                {
                    localFlagship.target = DestroyTheEnemyFleetGE.p;
                }
            }
        }
    }
}
