using UnityEngine;
namespace InfoDump
{
    /* Class: Item
     * Purpose: All things in the game will have this data for reference
     * Author: Jonathan Karcher
     */
    // everything in the scene should have
    public abstract class Item
    {
        // number used to reference what i have
        // dont realy need this but it cant hurt to have as the project grows
        protected int index;
        // description that is what is displayed in general information
        protected string description;
        // name is included in basic information
        protected string name;
        // this gameObject ref
        public GameObject g;
        // this transform ref
        public Transform t;

        public int Index { get { return index; } }
        public string Description { get { return description; } }
        public string Name { get { return name; } }

    }
    /* Class: Health
     * Purpose: All things in the game that can be destroyed will need health
     * Author: Jonathan Karcher
     */
    // every destructable thing should have
    public abstract class Health : Item
    {
        // current health
        protected float healthCur;
        // max health
        protected float healthMax;
        // death condition
        protected float healthMin;

        public float HealthCur { get { return healthCur; } }
        public float HealthMax { get { return healthMax; } }
        public float HealthMin { get { return healthMin; } }
    }
    /* Class: Ship
     * Purpose: All ships will have these options
     * Author: Jonathan Karcher
     */
    // every combative agent should have
    public abstract class Ship : Health
    {
        // how fast does the ship move
        protected float speed;
        // how fast does the ship turn
        protected float turnSpeed;
        // how much drift is added when they move
        // drift should always be in the direction that the player is moving
        protected float drift;
        // rockets count
        protected int specialAmmo;
        // indicates the live state
        protected bool alive;
        // what do i want to kill
        public Ship target;
        // primary weapon damage
        public float damageMain;
        // rocket damage
        public float damageSpecial;
        // movement, sound, death particle effect, and position indication references
        // note this is assigned later in the constructor
        public Rigidbody rig;
        public AudioSource audioS;
        public GameObject explosionPrefab;
        public GameObject arrow;

        // flagship is a special enemy and will get its own weapons range in the constructor
        public virtual float WeaponsRange { get { return 10; } }
        public float Speed { get { return speed; } }
        public float Drift { get { return drift; } }
        public int SpecialAmmo { get { return specialAmmo; } }
        public bool Alive { get { return alive; } }

        /* Method: TakeDamage
         * Purpose: add damage to the ship and return if the ship is still alive after
         * Restrictions: None
         */
        // if the combative agent takes damage
        // note this destroys this gameObject so remember to set the other target to null before destruction
        public bool TakeDamage(float DamageIn)
        {
            // reduce the health by the amount in
            healthCur -= DamageIn;
            // if the health is gone
            if (healthCur <= healthMin)
            {
                // reset the health to 0
                healthCur = healthMin;
                // remove data the arrow
                GameObject.Destroy(arrow);
                // set it to false
                alive = false;
                // generate an explosion
                GameObject explosion = GameObject.Instantiate(explosionPrefab, t.position, Quaternion.identity);
                // remove the explosion after 2 seconds
                GameObject.Destroy(explosion, 2);
                // remove the scene arrow
                arrow.SetActive(false);
                // destroy the ship
                GameObject.Destroy(g);
            }
            // the current status of the ship
            return alive;
        }
        /* Method: AddHealth
         * Purpose: add health to the ship
         * Restrictions: None
         */
        public bool AddHealth(float healthIn)
        {
            // if we are not already at full health
            if(healthCur < healthMax)
            {
                // add the health in
                healthCur += healthIn;
                // return that the ship has collected the health
                return true;
            }
            // the ship did not use the health
            return false;
        }
        /* Method: AddArmor
         * Purpose: add to the health max of the ship
         * Restrictions: None
         */
        public void AddArmor(float armorIn)
        {
            healthMax += armorIn;
            healthCur += armorIn;
        }
        /* Method: AddSpecialAmmo
         * Purpose: add rocket ammo to the ship
         * Restrictions: None
         */
        public bool AddSpecialAmmo(int amount)
        {
            // we did not use the ammo
            bool toReturn = false;
            // if we are not at full ammo
            if(specialAmmo < 10)// <-- randomly chosen max
            {
                //add the ammo to the ship
                specialAmmo += amount;
                // we used the ammo
                toReturn = true;
            }
            // return if we use the ammo
            return toReturn;
        }
        // turn the ship tword a target
        public void TurnTwordATarget()
        {
            Vector3 targetDirection = target.t.position - t.position;
            // rotation speed
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(t.forward, targetDirection, step, 0.0f);
            t.rotation = Quaternion.LookRotation(newDirection);
        }
        // move the local gameobject in the positive z
        public void MoveForward()
        {
            rig.AddForce((t.forward * Drift) * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.forward * Speed) * Time.deltaTime);
        }
        // move the local gameobject in the negative z
        public void MoveBackward()
        {
            rig.AddForce((t.forward * -1 * Drift) * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.forward * -1 * Speed) * Time.deltaTime);
        }
        // move the local gameobject in the negative x
        public void MoveLeft()
        {
            rig.AddForce((t.right * -1 * Drift) * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.right * -1 * (Speed / 2)) * Time.deltaTime);
        }
        // move the local gameobject in the positive x
        public void MoveRight()
        {
            rig.AddForce((t.right * Drift) * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.right * (Speed / 2)) * Time.deltaTime);
        }
        /* Method: LaunchRocket
         * Purpose: Launch a rocket with a seeking component
         * Restrictions: None
         */
        public Rocket LaunchRocket(GameObject rocketPrefab, GameObject rocketLaunchPoint, Ship seekingTarget)
        {
            // make a rocket object
            Rocket r = new Rocket(seekingTarget);
            // have the object clame this gameobject
            r.g = rocketPrefab;
            // assign the transform
            r.t = rocketPrefab.transform;
            // assign the rigidbody
            r.rig = rocketPrefab.GetComponent<Rigidbody>();
            specialAmmo--;
            return r;
        }
        /* Method: LaunchRocket
         * Purpose: Launch a rocket without a seeking component
         * Restrictions: None
         */
        public Rocket LaunchRocket(GameObject rocketPrefab, GameObject rocketLaunchPoint)
        {
            // make a rocket object
            Rocket r = new Rocket();
            // have the object clame this gameobject
            r.g = rocketPrefab;
            // assign the transform
            r.t = rocketPrefab.transform;
            // assign the rigidbody
            r.rig = rocketPrefab.GetComponent<Rigidbody>();
            specialAmmo--;
            return r;
        }
        // hand break... diftyness is cool but it does get annoying if you need a persision movement
        // note this only removes forces acting out ie you hit it... if it hits you forces of the hit remain
        public void RemoveForces()
        {
            rig.velocity = Vector3.zero;
            rig.angularDrag = 0;
        }
    }
    //-------------------------------------CONSTRUCTORS--------------------------------------------//
    /* Class: Rocket
     * Purpose: Create a rocket
     * Author: Jonathan Karcher
     */
    public class Rocket : Item
    {
        float explosiveDamage = 50;
        Transform seekingTarget;
        float radius = 1; // on contact only
        float speed = 100;
        public Rigidbody rig;
        public AudioSource audioS;
        public GameObject explosionPrefab;
        /* Constructor: Rocket
         * Purpose: Create a rocket with no seaking component
         * Restrictions: None
         */
        public Rocket()
        {
            name = "Rocket";
            index = (int)List.index.rocket;
            description = "BOOM";
        }
        /* Constructor: Rocket
         * Purpose: Create a rocket with a seaking component
         * Restrictions: None
         */
        public Rocket(Ship seekingTarget)
        {
            name = "Rocket";
            index = (int) List.index.rocket;
            description = "BOOM";
            this.seekingTarget = seekingTarget.t;
        }
        /* Method: Rocket
         * Purpose: Move the rocket, the rocket is an item but not a combative so it needs its own movement
         * Restrictions: None
         */
        public void MoveForward()
        {
            rig.AddForce(t.forward * speed * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.forward * speed) * Time.deltaTime);
        }
        /* Method: Rocket
         * Purpose: Main movement type since rockets easily miss
         * Restrictions: None
         */
        public void MoveSeeking()
        {
            t.LookAt(DestroyTheEnemyFleetGE.p.target.t);
            rig.AddForce(t.forward * speed * Time.deltaTime, ForceMode.Acceleration);
            rig.MovePosition(t.position + (t.forward * speed) * Time.deltaTime);
        }
        /* Method: Explode
         * Purpose: Transfer rocket damage
         * Restrictions: None
         */
        public float Explode(ref Collision other)
        {
            // add a random force to the ship if its hit by a rocket
            // verry buggy
            //if (other.gameObject.GetComponent<Rigidbody>() != null)
            //{
            //    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
            //}

            GameObject explosion = GameObject.Instantiate(explosionPrefab, t.position, Quaternion.identity);
            explosion.name = "BOOM LOL";
            GameObject.Destroy(explosion, 2);
            GameObject.Destroy(g);
            return explosiveDamage;
        }
        public float Damage { get { return explosiveDamage; } }
    }
    /* Class: Player
     * Purpose: Create a player
     * Author: Jonathan Karcher
     */
    public class Player : Ship
    {
        public Player(GameObject prefab)
        {
            target = null;
            g = prefab;
            t = prefab.GetComponent<Transform>();
            rig = prefab.GetComponent<Rigidbody>();
            audioS = prefab.GetComponent<AudioSource>();
            // player arrow is not used
            arrow = null;
            // probably dont need this
            name = "Player";
            index = (int)List.index.player;
            healthCur = 100;
            healthMax = 100;
            healthMin = 0;
            speed = 30;
            drift = 8;
            specialAmmo = 0;
            damageMain = 100;
            damageSpecial = 50;
            // note player alive is actually game over condition
            alive = true;
        }
    }
    /* Class: Enemy
     * Purpose: Create an enemy
     * Author: Jonathan Karcher
     */
    public class Enemy : Ship
    {
        
        public Enemy(GameObject prefab)
        {
            target = null;
            turnSpeed = 1f;
            g = prefab;
            t = prefab.GetComponent<Transform>();
            rig = prefab.GetComponent<Rigidbody>();
            audioS = prefab.GetComponent<AudioSource>();
            name = "CrabShip";
            index = (int)List.index.enemyBasic;
            description = "One of the insect ships, its them or us.";
            healthCur = 100;
            healthMax = 100;
            healthMin = 0;
            speed = 8;
            drift = 8;
            specialAmmo = 0;
            damageMain = 10;
            damageSpecial = 50;
            alive = true;
        }
    }
    /* Class: Allied
     * Purpose: Create a ally
     * Author: Jonathan Karcher
     */
    public class Allied : Ship
    {
        public bool isWingman;

        public Allied(GameObject prefab)
        {
            //isWingman = false; I dont think ill have time to implement
            target = null;
            turnSpeed = 1f;
            g = prefab;
            t = prefab.GetComponent<Transform>();
            rig = prefab.GetComponent<Rigidbody>();
            audioS = prefab.GetComponent<AudioSource>();
            name = "Allied";
            index = (int)List.index.alliedShip;
            description = "We are the only line of defence between earth and anialation.";
            healthCur = 100;
            healthMax = 100;
            healthMin = 0;
            speed = 12;
            drift = 8;
            specialAmmo = 0;
            damageMain = 10;
            damageSpecial = 50;
            alive = true;
        }
    }
    /* Class: MotherShip
     * Purpose: Create a mothership
     * Author: Jonathan Karcher
     */
    public class MotherShip : Ship
    {
        public MotherShip(GameObject prefab)
        {
            target = null;
            g = prefab;
            t = prefab.GetComponent<Transform>();
            rig = prefab.GetComponent<Rigidbody>();
            audioS = prefab.GetComponent<AudioSource>();
            name = "MotherShip";
            index = (int)List.index.motherShip;
            description = "Our home base, we can not lose this... we dont have the fuel to fly back home.";
            healthCur = 4000;
            healthMax = 4000;
            healthMin = 0;
            speed = 0;
            drift = 0;
            specialAmmo = 0;
            alive = true;
        }
    }
    /* Class: Flagship
     * Purpose: Create a flagship
     * Author: Jonathan Karcher
     */
    public class Flagship : Ship
    {
        // this is the weapons range of the flagship
        public override float WeaponsRange
        {
            get { return 50; }
        }
        public Flagship(GameObject prefab)
        {
            target = null;
            g = prefab;
            t = prefab.GetComponent<Transform>();
            rig = prefab.GetComponent<Rigidbody>();
            audioS = prefab.GetComponent<AudioSource>();
            name = "Flagship";
            index = (int)List.index.motherShip;
            description = "Carefull the insect ships dont come much bigger than this.";
            healthCur = 1000;
            healthMax = 1000;
            healthMin = 0;
            speed = 16;
            drift = 8;
            specialAmmo = 0;
            damageMain = 10;
            alive = true;
        }
    }
}