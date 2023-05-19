using UnityEngine;
using UnityEngine.UI;
using InfoDump;

public class BasicPersuit : MonoBehaviour
{
    //---------------------------------------
    //  This script is depreciated but is kept as a reference
    //---------------------------------------



    // Start is called before the first frame update
    //[SerializeField] Rigidbody rig;
    [SerializeField] public bool active;
    [SerializeField] GameObject GE;
    [SerializeField] GameObject Explosion;
    //[SerializeField] public GameObject EnemyArrow;
    GameObject arrow;
    public Enemy localEnemy;
    private bool firstCycle = true;
    void Start()
    {
        localEnemy = new Enemy(gameObject);
        localEnemy.explosionPrefab = Explosion;
        GameEngine.enemy.Add(localEnemy);
        localEnemy.target = GameEngine.p;
        //localEnemy.t.GetChild(0).gameObject.GetComponent<ScreenPos>().box.gameObject.SetActive(false);
        //localEnemy.arrow = EnemyArrow;
        //localEnemy.CreateArrow();
        //localEnemy.arrow.transform.SetParent(GameObject.Find("Player").transform.GetChild(0).transform);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if(firstCycle)
            {
                // find an unused arrow arrow
                for (int i = 0; i < 20; i++)
                {
                    if (!GameEngine.enemyArrow[i].activeInHierarchy)
                    {
                        GameEngine.enemyArrow[i].SetActive(true);
                        arrow = GameEngine.enemyArrow[i];
                        break;
                    }
                }
                localEnemy.arrow = arrow;
                arrow.GetComponent<Image>().color = new Vector4(1, 0, 0, 1);
                firstCycle = false;
            }
            localEnemy.RemoveForces();
            // AI implemented
            // ... about as simple as it can be
            // non combat just persuit
            if(localEnemy.target == null)
            {
                localEnemy.target = GameEngine.p;
            }
            transform.LookAt(localEnemy.target.t.position);
            localEnemy.rig.MovePosition(localEnemy.t.position + (localEnemy.t.forward * 8) * Time.deltaTime);
            localEnemy.t.GetChild(0).gameObject.SetActive(true);
            localEnemy.t.GetChild(0).gameObject.GetComponent<ScreenPos>().thisShipArrow = arrow;
        }
    }
}
