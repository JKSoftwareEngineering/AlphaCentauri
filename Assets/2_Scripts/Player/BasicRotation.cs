using UnityEngine;

public class BasicRotation : MonoBehaviour
{
    [SerializeField] Vector2 sensitivity;
    [SerializeField] Vector2 acceleration;
    [SerializeField] float lagCatch;
    Vector2 rotation;
    Vector2 velocity;
    Vector2 lagEvent;
    float lagTimer;

    // Update is called once per frame
    void Update()
    {
        // check for the game to lag
        lagTimer += Time.deltaTime;
        // get the mouse input
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        // once the lage timer expires we either have or dont have input
        // this is a minnor delay just in case we didnt get the player input
        if(!(Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y))||lagTimer >= lagCatch)
        {
            lagEvent = input;
            lagTimer = 0;
        }
        // proceed with input
        // in the event that we missed the player input and it was cought by the lag event then that will be the player input
        input = lagEvent;
        // turn the cammera based on the sensitivity set to the mouse
        Vector2 turnSpeed = input * sensitivity;
        // add in the floaty aspect through the acceleration to give the feel of a low friction atmosphere
        velocity = new Vector2(Mathf.MoveTowards(velocity.x, turnSpeed.x, acceleration.x * Time.deltaTime), Mathf.MoveTowards(velocity.y, turnSpeed.y, acceleration.y * Time.deltaTime));
        //rotate slowly untill the mouse input is centered in the screen again
        rotation += velocity * Time.deltaTime;

        {
            // because touchpads suck and my mouse is dead
            // negates all floatyness, removes lag Timer, and restricts rotation when in motion 
            // rotation += input * new Vector2(-50,50) * Time.deltaTime;
        }
        // set the rotation of the player to what we enter
        transform.eulerAngles = new Vector3(rotation.x, rotation.y, 0);
    }
}
