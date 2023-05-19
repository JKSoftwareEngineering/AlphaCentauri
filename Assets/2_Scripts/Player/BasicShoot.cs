using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InfoDump;

public class BasicShoot : MonoBehaviour
{
    [SerializeField] GameObject Rocket;
    [SerializeField] GameObject Lazer;
    [SerializeField] GameObject RocketLaunchPoint;
    [SerializeField] TextMeshProUGUI BasicInfo;
    [SerializeField] TextMeshProUGUI GeneralInfo;
    [SerializeField] TextMeshProUGUI rocketCount;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI lazerChargeText;
    [SerializeField] Image lazerFireImage;
    [SerializeField] Image healthImage;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioSource audioS;
    [SerializeField] GameObject GE;
    [SerializeField] Image CrackedGlass;
    private bool overHeatTrigger = false;
    RaycastHit InfoHit;
    RaycastHit lazerHit;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // the display of the amount of rockets the player has
        rocketCount.text = DestroyTheEnemyFleetGE.p.SpecialAmmo + "/10";
        // display the health of the player ship
        healthImage.fillAmount = DestroyTheEnemyFleetGE.p.HealthCur/DestroyTheEnemyFleetGE.p.HealthMax;
        // aulternate mehtod of showing the health of the player ship
        CrackedGlass.color = new Color(CrackedGlass.color.r, CrackedGlass.color.g, CrackedGlass.color.b, (1-healthImage.fillAmount));
        // show the charge of the player lazer
        lazerChargeText.text = (int)(lazerFireImage.fillAmount * 100) + "%";
        // show the health of the player ship
        healthText.text = (int)(healthImage.fillAmount * 100) + "%";
        // constant beam to show what the player is looking at
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out InfoHit))
        {
            // if we detect an enemy
            if (InfoHit.transform.gameObject.GetComponent<EnemyAI>() != null)
            {
                Enemy e = InfoHit.transform.gameObject.GetComponent<EnemyAI>().localEnemy;
                // if the enemy is critically damaged
                if(e.HealthCur/e.HealthMax <.25f)
                {
                    BasicInfo.text = "The enemy is critically damaged\nNow is the time to strike.";
                }
                // else
                else
                {
                    BasicInfo.text = InfoHit.transform.gameObject.GetComponent<EnemyAI>().localEnemy.Description;
                }
                // set this enemy as the seeking target
                DestroyTheEnemyFleetGE.p.target = e;
                // if we are not looking at anything the dont display anything
                if (DestroyTheEnemyFleetGE.p.target == null)
                {
                    GeneralInfo.text = "";
                }
                // otherwise show the health of the ship
                else
                {
                    GeneralInfo.text = "" + Mathf.Round(e.HealthCur) + " / " + e.HealthMax;
                }
            }
            // if we detect a falgship
            else if(InfoHit.transform.gameObject.GetComponent<FlagshipAI>() != null)
            {
                // tell us the condition of the flagship and set it to the target
                Flagship e = InfoHit.transform.gameObject.GetComponent<FlagshipAI>().localFlagship;
                BasicInfo.text = InfoHit.transform.gameObject.GetComponent<FlagshipAI>().localFlagship.Description;       
                DestroyTheEnemyFleetGE.p.target = e;
                GeneralInfo.text = "" + Mathf.Round(e.HealthCur) + " / " + e.HealthMax;
            }
            // if we detect an ally
            else if(InfoHit.transform.gameObject.GetComponent<AlliedAI>() != null)
            {
                // technically the player has a inbeded Allied AI that is disabled so if i want friendly fire then we have to check if it is enabled too otherwise we will just shoot ourselves from time to time
                if (InfoHit.transform.gameObject.GetComponent<AlliedAI>().enabled)
                {
                    // tell us the condition of the ally but dont target it
                    Allied a = InfoHit.transform.gameObject.GetComponent<AlliedAI>().localAllied;
                    BasicInfo.text = InfoHit.transform.gameObject.GetComponent<AlliedAI>().localAllied.Description;
                    GeneralInfo.text = "" + Mathf.Round(a.HealthCur) + " / " + a.HealthMax;
                }
            }
            // if we detected a mothership
            // you are way more likely to do damage to your mother ship than to the enemy
            else if (InfoHit.transform.gameObject.GetComponent<MothershipAI>() != null)
            {
                // tell us the condition of the mothership but dont target it
                MotherShip a = InfoHit.transform.gameObject.GetComponent<MothershipAI>().localMothership;
                BasicInfo.text = InfoHit.transform.gameObject.GetComponent<MothershipAI>().localMothership.Description;
                GeneralInfo.text = "" + Mathf.Round(a.HealthCur) + " / " + a.HealthMax;
            }
            // we didnt detect anything
            else
            {
                GeneralInfo.text = "";
            }
        }
        // primary wrapon
        if(Input.GetMouseButton(0) && !overHeatTrigger)
        {
            // play the lazer audio
            if (!audioS.isPlaying)
            {
                audioS.Play();
            }
            // activate the lazer to glow
            Lazer.SetActive(true);
            // reduce the value of the slider
            lazerFireImage.fillAmount -= Time.deltaTime * .8f;
            // if the lazer overheated
            if(lazerFireImage.fillAmount <= 0)
            {
                overHeatTrigger = true;
            }
            // cast damage to the point straight ahead
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out lazerHit))
            {
                // create a fireball on the normal direction
                GameObject g = Instantiate(explosion, lazerHit.point, Quaternion.LookRotation(lazerHit.normal));
                // fireball from lazer is destroyed after 1 second
                Destroy(g, 1);
                // if we hit an enemy
                if (lazerHit.transform.gameObject.GetComponent<EnemyAI>() != null)
                {
                    Ship e = InfoHit.transform.gameObject.GetComponent<EnemyAI>().localEnemy;
                    if(!e.TakeDamage(DestroyTheEnemyFleetGE.p.damageMain * Time.deltaTime))
                    {
                        DestroyTheEnemyFleetGE.enemy.Remove((Enemy)e);
                    }
                }
                // if we hit a flagship
                if (lazerHit.transform.gameObject.GetComponent<FlagshipAI>() != null)
                {
                    Ship e = InfoHit.transform.gameObject.GetComponent<FlagshipAI>().localFlagship;
                    e.TakeDamage(DestroyTheEnemyFleetGE.p.damageMain * Time.deltaTime);
                    if (!e.Alive)
                    {}
                }
                // you are way more likely to do damage to your own mother ship than the enemy
                //if (lazerHit.transform.gameObject.GetComponent<MothershipAI>() != null)
                //{
                //    Ship a = InfoHit.transform.gameObject.GetComponent<MothershipAI>().localMothership;
                //    a.TakeDamage(DestroyTheEnemyFleetGE.p.damageMain * Time.deltaTime);
                //    if (!a.Alive)
                //    { }
                //}
                // if we hit a ally
                if (lazerHit.transform.gameObject.GetComponent<AlliedAI>() != null)
                {
                    // technically the player has a inbeded Allied AI that is disabled so if i want friendly fire then we have to check if it is enabled too
                    if (lazerHit.transform.gameObject.GetComponent<AlliedAI>().enabled)
                    {
                        Ship a = InfoHit.transform.gameObject.GetComponent<AlliedAI>().localAllied;
                        // there is friendly fire
                        a.TakeDamage(DestroyTheEnemyFleetGE.p.damageMain * Time.deltaTime);
                        if (!a.Alive)
                        { }
                    }
                }
            }
        }
        // if you are not firing the lazer
        else
        {
            // increase the lazer slider
            if(lazerFireImage.fillAmount < 1)
            {
                lazerFireImage.fillAmount += Time.deltaTime;
            }
            // turn off over heat after 50%
            if(lazerFireImage.fillAmount > .5f)
            {
                overHeatTrigger = false;
            }
            // turn off the audio
            audioS.Stop();
            // turn off the lazer glow
            Lazer.SetActive(false);
        }
        // fire a rocket
        if(Input.GetMouseButtonDown(1))
        {
            // make a rocket prefab
            if (DestroyTheEnemyFleetGE.p.SpecialAmmo > 0)
            {
                // if the player has not targeted a viable target
                if(DestroyTheEnemyFleetGE.p.target == null)
                {
                    GameObject g = Instantiate(Rocket, RocketLaunchPoint.transform.position, transform.GetChild(0).rotation);
                    g.GetComponent<RocketForward>().rocket = DestroyTheEnemyFleetGE.p.LaunchRocket(g, RocketLaunchPoint);
                }
                // if the player has targeted a viable target
                else
                {
                    GameObject g = Instantiate(Rocket, RocketLaunchPoint.transform.position, transform.GetChild(0).rotation);
                    g.GetComponent<RocketForward>().rocket = DestroyTheEnemyFleetGE.p.LaunchRocket(g, RocketLaunchPoint, DestroyTheEnemyFleetGE.p.target);
                }
            }
        }
    }
}
