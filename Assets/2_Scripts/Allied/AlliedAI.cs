using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class AlliedAI : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public Allied localAllied;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject LazerBurn;
    [SerializeField] GameObject Lazer;
    private float newTargetTimer = 0f;
    public float localshipHealth;
    RaycastHit lazerHit;
    [SerializeField] GameObject LazerGameObject;
    void Start()
    {
        localAllied.explosionPrefab = explosionPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        localshipHealth = localAllied.HealthCur;
        // if we are moving out of the map go toward wherever the player is
        if (Vector3.Distance(localAllied.t.position, DestroyTheEnemyFleetGE.p.t.position) > 75)
        {
            localAllied.t.LookAt(DestroyTheEnemyFleetGE.p.t.position);
            transform.Translate(Vector3.forward);
        }
        // if we dont have a target
        if (localAllied.target == null)
        {
            // set a maximum distance
            float distance = Mathf.Infinity;
            // find the closest enemy ship and set it as the target
            foreach (Enemy enemy in DestroyTheEnemyFleetGE.enemy)
            {
                try
                {
                    if (Vector3.Distance(localAllied.t.position, enemy.t.position) < distance)
                    {
                        distance = Vector3.Distance(localAllied.t.position, enemy.t.position);
                        localAllied.target = enemy;
                    }
                }
                catch
                { }
            }
        }
        // we have a target
        else
        {
            newTargetTimer += Time.deltaTime;
            // every 5 seconds search for the closest target
            if(newTargetTimer >= 5f)
            {
                localAllied.target = null;
                newTargetTimer = 0f;
            }
            // double chect to make sure that someone else didnt kill our target before we could
            try
            {
                // face the target
                localAllied.TurnTwordATarget();
                localAllied.MoveForward();
                // if we are outside weapons range
                if(Vector3.Distance(localAllied.t.position, localAllied.target.t.position) > localAllied.WeaponsRange)
                {
                    LazerGameObject.SetActive(false);
                    localAllied.MoveForward();
                }
                // if we are in weapons range
                if (Vector3.Distance(localAllied.t.position, localAllied.target.t.position) <= localAllied.WeaponsRange)
                {
                    // damage the target and if nesssisary remove it from the enemy list
                    if (!localAllied.target.g.GetComponent<EnemyAI>().localEnemy.TakeDamage(localAllied.damageMain * Time.deltaTime))
                    {
                        LazerGameObject.SetActive(true);
                        newTargetTimer = 0;
                        DestroyTheEnemyFleetGE.enemy.Remove((Enemy)localAllied.target);
                    }
                    // if the target isnt dead then add particle effects to show damage on the normal opposite direction
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
                {// if we are under attack

                }
                {// if we are attacking

                    {// if we are alone
                        {// do we have rockets

                        }
                        //otherwise
                    }
                    {// if we are not alone
                        {// do we have rockets

                        }
                        //otherwise

                    }
                }
            }
            catch
            {
                localAllied.target = null;
            }
        }
    }
}
