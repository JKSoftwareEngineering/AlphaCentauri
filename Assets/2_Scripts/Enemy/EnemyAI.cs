using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public Enemy localEnemy;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject LazerBurn;
    [SerializeField] GameObject Lazer;
    [SerializeField] GameObject LazerGameObject;
    Ship MothershipTarget;
    private float newTargetTimer = 0f;
    float distance;
    RaycastHit lazerHit;
    void Start()
    {
        // if we are not in a defend mission dont look for a mothership
        if(DestroyTheEnemyFleetGE.motherShip != null)
        {
            MothershipTarget = DestroyTheEnemyFleetGE.motherShip;
        }
        localEnemy.explosionPrefab = explosionPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        // if we are moving out of the map go toward wherever the player is
        if (Vector3.Distance(localEnemy.t.position, DestroyTheEnemyFleetGE.p.t.position) > 75)
        {
            localEnemy.t.LookAt(DestroyTheEnemyFleetGE.p.t.position);
            transform.Translate(Vector3.forward);
        }
        // if we dont have a target
        if (localEnemy.target == null)
        {
            try
            {
                // set a maximum distance
                distance = Mathf.Infinity;
                // find the closest allied ship and set it as the target
                foreach (Allied allied in DestroyTheEnemyFleetGE.allied)
                {
                        if (Vector3.Distance(localEnemy.t.position, allied.t.position) < distance)
                        {
                            distance = Vector3.Distance(localEnemy.t.position, allied.t.position);
                            localEnemy.target = allied;
                        }   
                }
                // if the player is closer than the allied selected
                if (Vector3.Distance(localEnemy.t.position, DestroyTheEnemyFleetGE.p.t.position) < distance)
                {
                    localEnemy.target = DestroyTheEnemyFleetGE.p;
                }
            }
            catch
            {
                localEnemy.target = null;
            }
        }
        // we have a target
        else
        {
            newTargetTimer += Time.deltaTime;
            // every 5 seconds search for a new target
            if (newTargetTimer >= 5f)
            {
                // 30% chance that this enemy will ignore everything else and go straight for the mothership
                if(MothershipTarget != null)
                {
                    if((int)Random.Range(0f,3f) <= 1)
                    {
                        localEnemy.target = MothershipTarget;
                    }
                    else
                    {
                        localEnemy.target = null;
                    }
                }
                else
                {
                    localEnemy.target = null;
                }
                newTargetTimer = 0f;
            }
            // double chect to make sure that someone else didnt kill our target before we could
            try
            {
                // face the target
                localEnemy.TurnTwordATarget();
                // if we are outside weapons range
                if(Vector3.Distance(localEnemy.t.position, localEnemy.target.t.position) > localEnemy.WeaponsRange)
                {
                    LazerGameObject.SetActive(false);
                    localEnemy.MoveForward();
                }
                // if we are in weapons range
                if (Vector3.Distance(localEnemy.t.position, localEnemy.target.t.position) <= localEnemy.WeaponsRange)
                {
                    // if its not the player look for the allied AI component
                    if (localEnemy.target != DestroyTheEnemyFleetGE.p)
                    {
                        // the not is since TakeDamage() is a bool that indicates if the target is dead... so if not dead then take damage and remove if nessisary
                        if (!localEnemy.target.g.GetComponent<AlliedAI>().localAllied.TakeDamage(localEnemy.damageMain * Time.deltaTime))
                        {
                            newTargetTimer = 0;
                            DestroyTheEnemyFleetGE.allied.Remove((Allied)localEnemy.target);
                        }
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
                    // if it is the player damage the player, the player has an inbuild allied AI so attack the player directally
                    else
                    {
                        Ray ray = new Ray(Lazer.transform.position, Lazer.transform.TransformDirection(Vector3.forward));
                        if (Physics.Raycast(ray, out lazerHit))
                        {
                            LazerGameObject.SetActive(true);
                            Debug.Log(lazerHit.transform.gameObject.name);
                            GameObject g = Instantiate(LazerBurn, lazerHit.point, Quaternion.LookRotation(lazerHit.normal));
                            DestroyTheEnemyFleetGE.p.TakeDamage(localEnemy.damageMain * Time.deltaTime * 5);
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
                // attacking the mothership needs extra distance since the ship is large enough that they will colide with it before getting in the weapons range of the transform
                if (Vector3.Distance(localEnemy.t.position, localEnemy.target.t.position) <= localEnemy.WeaponsRange * 2)
                {
                    if (localEnemy.target == DestroyTheEnemyFleetGE.motherShip)
                    {
                        LazerGameObject.SetActive(true);
                        localEnemy.target.g.GetComponent<MothershipAI>().localMothership.TakeDamage(localEnemy.damageMain * Time.deltaTime);
                    }
                }
            }
            catch
            {
                localEnemy.target = null;
                // if there are no allies to target and we didnt target the player then target the player
                if(DestroyTheEnemyFleetGE.allied.Count == 0)
                {
                    localEnemy.target = DestroyTheEnemyFleetGE.p;
                }
            }
        }
    }
}
