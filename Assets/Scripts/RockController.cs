//This script controls the rock object and includes functions to launch the rock at the window and teleport the rock to a
//position to be launched from

// thread: Make an object face current cameras worldpoint target: https://discussions.unity.com/t/make-an-object-face-current-cameras-worldpoint-target/228476 
// collision detection: https://docs.unity3d.com/ScriptReference/Collision-gameObject.html 
// how to play sounds on collision: https://youtu.be/lBTtzqfaNdM?si=Eb341b51sBJjU1L6
// sound clip used: https://pixabay.com/sound-effects/glass-shatter-3-100155/
// rigidbody reference, particularly looking at linear and angular velocity properties: https://docs.unity3d.com/ScriptReference/Rigidbody.html
// rigidbody.AddForce documentation: https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
// shooting projectiles reference: https://youtu.be/qeHoJzL-haM?si=cw3EN0kjhsIWreWT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RockController : MonoBehaviour
{
    //objects
    public Rigidbody body; //the rock's rigidbody component
    public GameObject target; //what the rock will be launched at, drag the window gameobject into this field
    public UIOverlayController UIcontroller; //controls the state of the UI, drag SceneManger into this field

    //audio 
    public AudioSource audioSource;
    public AudioClip audioClip;

    //additional info
    Vector3 initialPosition; //starting position of the rock object
    public Vector3 secondPosition; //a secondary position the rock can be thrown from, added for the sake of variability 

    public float launchForce = 50f; /* the force used to launch the rock, feel free to play around with this value but you 
                                     * might have to refresh the script component in the inspector panel */


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        //initialize position vectors 
        initialPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); //note what the starting position of the rock is
        secondPosition = new Vector3(this.transform.position.x - 10, this.transform.position.y, this.transform.position.z + 5); // choose 2nd position based off the initial
                                                                                                                                // position, might have to play with these values
        
        this.gameObject.transform.LookAt(target.transform); //look at the target object (window)
    }

    // Update is called once per frame
    void Update()
    {
        //nothing actively happening in Update()
        //checkKeyDebugControls();
    }

    public void launchRock()
    {
        body.angularVelocity = Vector3.zero; //make sure angular velocity of rock is 0

        this.gameObject.transform.LookAt(target.transform); //look at target

        body.AddForce(transform.forward * launchForce, ForceMode.Impulse); //launch rock

    }

    public void resetRockPosition()
    {
        //set rock's linear and angular velocity to 0, aka stop the rock's motion
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;

        //randomly choose between two locations to launch the rock from and teleport the rock to that position
        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            this.transform.position = initialPosition; //launch from the rock's initial positon
        }
        else
        {
            this.transform.position = secondPosition; //launch from some secondary position based off the initial position
        }

        this.gameObject.transform.LookAt(target.transform); //look at the target (which should be the window)

    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        //Check to see if the name of the gameobject collided with matches the specified name 
        //Note - put the name of the window object in the " " !
        if (collision.gameObject.name == "FauxWindow")  //if the rock collides with the window, do:
        {
            Debug.Log("Rock hit window");
            UIcontroller.showWindowOverlay(); //show cracked screen overlay on bottom window pane
            audioSource.PlayOneShot(audioClip); //play collision sound effect
        }

    }

    /* Note to Byron:
     * The following are keyboard input controls used for debugging before I automated the event of the rock being launched
     * Please ignore this code, it is messy and not directly used in the final system, but I'm saving the code for posterity 
     * and to keep record of my work flow.      
     */
    
    /*
    void checkKeyDebugControls() 
    {
        //add an impulse force to the rock so it flies at the camera
        if (Input.GetKey(KeyCode.Space)) //space key pressed
        {
            body.angularVelocity = Vector3.zero;
            //body.velocity = Vector3.zero;
            this.gameObject.transform.LookAt(target.transform);
            //body.velocity = transform.forward * launchForce;
            //body.velocity = transform.up * launchForce;

            body.AddForce(transform.forward * 2f, ForceMode.Impulse);
            //body.AddForce(0, 0, 2f, ForceMode.Impulse);
        }


        //reset to initial position by hitting O key, for debug
        if (Input.GetKeyDown(KeyCode.O)) //O key pressed
        {
            //set rock's linear and angular velocity to 0, aka stop rock's motion
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;

            //teleport the rock back to its starting position
            this.transform.position = initialPosition;
            this.gameObject.transform.LookAt(target.transform);

        }

        //hit P key to teleport the rock to a secondary position
        if (Input.GetKey(KeyCode.P)) //P key pressed
        {

            this.transform.position = new Vector3(2, 1, -1);
            this.gameObject.transform.LookAt(target.transform);
            body.velocity = Vector3.zero;

        }

    }
    */

}
