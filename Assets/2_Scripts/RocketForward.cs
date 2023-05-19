using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class RocketForward : MonoBehaviour
{
    [SerializeField] GameObject Explosion;
    public Rocket rocket;
    // Start is called before the first frame update
    void Start()
    {
        rocket.explosionPrefab = Explosion; 
    }

    // Update is called once per frame
    void Update()
    {
        // can still use simple straight forward rocked but they can be dificult to hit anything
        if (DestroyTheEnemyFleetGE.p.target == null)
        {
            rocket.MoveForward();
        }
        else
        {
            rocket.MoveSeeking();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            if(!collision.gameObject.GetComponent<EnemyAI>().localEnemy.TakeDamage(rocket.Damage))
            {
                DestroyTheEnemyFleetGE.enemy.Remove((Enemy)collision.gameObject.GetComponent<EnemyAI>().localEnemy);
            }
        }
        if (collision.gameObject.GetComponent<FlagshipAI>() != null)
        {
            collision.gameObject.GetComponent<FlagshipAI>().localFlagship.TakeDamage(rocket.Damage);
        }
        // Ill allow friendy fire too
        if (collision.gameObject.GetComponent<AlliedAI>() != null)
        {
            collision.gameObject.GetComponent<AlliedAI>().localAllied.TakeDamage(rocket.Damage);
        }
        rocket.Explode(ref collision);
    }
}
