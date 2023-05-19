using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfoDump;

public class BasicMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject GE;
    Player player;
    Transform playerTransform;
    [SerializeField] GameObject RadiationWarning;
    void Start()
    {
        // lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // make it invisable so its not anoying
        Cursor.visible = false;
        player = DestroyTheEnemyFleetGE.p;
        if (player != null)
        {
            playerTransform = DestroyTheEnemyFleetGE.p.t;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make sure there is a player
        if (player == null || playerTransform == null)
        {
            player = DestroyTheEnemyFleetGE.p;
        }
        // this is reversed from the norm becouse the player is spawned in "backward" so they can face the battlefield before they are just in the battlefield
        if (Input.GetKey(KeyCode.W))
        {
            player.MoveBackward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.MoveForward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.MoveRight();
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.MoveLeft();
        }
        if(Input.GetKey(KeyCode.Space))
        {
            player.RemoveForces();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // if the player collided with a rocket cube
        // max of 10
        if(other.collider.gameObject.tag == "RocketCube")
        {
            if(DestroyTheEnemyFleetGE.p.AddSpecialAmmo(2))
            {
                Destroy(other.gameObject);
            }
        }
        // if the player collieded with a lazer cube
        if (other.collider.gameObject.tag == "LazerCube")
        {
            DestroyTheEnemyFleetGE.p.damageMain += 50;
            Destroy(other.gameObject);
        }
        // if the player collided with a armor cube
        if (other.collider.gameObject.tag == "ArmorCube")
        {
            DestroyTheEnemyFleetGE.p.AddArmor(20);
            Destroy(other.gameObject);
        }
        // if the player collieded with a health cube
        if (other.collider.gameObject.tag == "HealthCube")
        {
            if (DestroyTheEnemyFleetGE.p.AddHealth(40))
            {
                Destroy(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // if the player is leaving the battle field
        if(other.tag == "InnerRadiation")
        {
            RadiationWarning.SetActive(true);
        }
        // if the player has left the battle field
        if(other.tag == "OuterRadiation")
        {
            DestroyTheEnemyFleetGE.gameLost = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // if the player is in the battle field
        if (other.tag == "InnerRadiation")
        {
            RadiationWarning.SetActive(false);
        }
    }
}