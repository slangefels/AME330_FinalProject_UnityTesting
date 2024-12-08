//A script that controls the UI Overlay. Includes functions for showing and hiding the UI. In Update() the UI is scaled according to
//the dimensions of the game scene window and is positioned vertically using encoderPosition from DataIO

//how to set width/height of an UI element: https://discussions.unity.com/t/how-do-i-set-the-width-height-of-a-ui-element/139029
//how to make UI element invisible: https://discussions.unity.com/t/make-ui-invisible-when-ui-button-is-pressed/831470
//                                  https://discussions.unity.com/t/how-to-make-a-panel-visible-or-invisible-via-script/551923

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlayController : MonoBehaviour //attach me to a SceneManager Object
{
    //objects
    public RectTransform uiElement; // reference to the UI element, drag Overlay gameobject into this field
    public Image overlay; //drag Overlay gameobject into this field
    public DataIO data; //drag the gameobject (DataManager) into the Data field in the inspector panel
    public Timer timer; //drag the SceneManager Object into this field


    //screen dimension values
    public int screenWidth = 0;
    public int screenHeight = 0;
    
    //encoder value
    public int encoderPos = 0; //holds the encoder position data coming over from the DataIO script, stored here as an int (I'm more familiar working with ints)

    //UI values
    public Vector2 newUIPosition; //new screen position to move the UI to
    public int stepAmount = 1; //for using the arrow key input controls; how much the UI would be moved when using the arrow key inputs
    public int xPos = 0; //UI overlay's x position on the screen
    public int yPos = 0; //UI overlay's y position on the screen

    float screenPosition = 0.0f; //0-1.0f value that indicates how far up the UI should be positioned on the screen
                                 //(used to calculate the specific y pixel location) (0.0f is the very bottom, 1.0f is the very top I believe)

    public bool windowCracked = false; //boolean indicating if the cracked UI overlay should be shown

    //public int testInput = 0; //for temporary debug use

    // Start is called before the first frame update
    void Start()
    {
        //get the window's dimensions
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        overlay.gameObject.SetActive(windowCracked); //UI overlay is initially set to not be visible 
    }

    // Update is called once per frame
    void Update()
    {
        //screen width and height can change if the game window dimensions change - so keep track of the current dimensions
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        //scale UI:
        //UI width = width of screen
        //UI height = 1/2 screen height
        // Note: make sure Canvas gameobject > Canvas Scalar > UI Scale Mode is set to Constant Pixel Size
        uiElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) screenHeight / 2); //scale vertical dimension
        uiElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) screenWidth); //scale horizontal dimension

        //get encoder position
        encoderPos = (int)data.encoderPosition; // + testInput; 
                                                //+ testInput is just so I can test the InverseLerp/Lerp code using key inputs instead of using the actual encoder
                                                // testInput won't be included in final system
        

        // use encoder position to control UI overlay vertical position
        //InverseLerp and Lerp found by reading Mathf documentation
        screenPosition = Mathf.InverseLerp(0, 416, (float) encoderPos); //returns 0.0-1.0f value that signifies where the encoderPos falls
                                                                        //in the range between 0 and 416 (max height the physical window pane
                                                                        //can move to)
        yPos = (int) Mathf.Lerp(0.0f, screenHeight * 0.5f, screenPosition); //formerly screenHeight * 1.0f;
                                                                            /* sets min yPos to bottom of the screen and prevents UI from 
                                                                             * moving above the top of the screen, aka confines the vertical 
                                                                             * movement of the UI to within the vertical bounds of the screen 
                                                                             */

        //move UI overlay
        newUIPosition = new Vector2(xPos, yPos); 
        uiElement.position = newUIPosition; //move UI to new position


        /* --- DEBUG CODE --------------------------------------------------
        //toggle show/hide cracked UI - for debug purposes
        if (Input.GetKeyUp(KeyCode.H)) //H arrow key pressed
        {
            windowCracked = !windowCracked; //flip visibility of the UI overlay; ex. visible --> not visible; not visible --> visible
            overlay.gameObject.SetActive(windowCracked); //change visibility of UI
        }

        //arrow key and W,S debug controls -- arrow key controls don't work when the encoder is used but W,S do
        //checkArrowKeyControls();
        ------------------------------------------------------------------------ 
        */
    }

    public void showWindowOverlay() //makes cracked UI visible
    {
        windowCracked = true;
        overlay.gameObject.SetActive(windowCracked); //activate/show UI object
        timer.StartCoroutine("HideUITimer"); //start the countdown to when the UI will be hidden again
    }

    public void hideWindowOverlay() //makes cracked UI invisible
    {
        windowCracked = false;
        overlay.gameObject.SetActive(windowCracked); //deactivate/hide UI
        timer.StartCoroutine("RockLaunchTimer"); //start countdown to when the rock will be launched again
    }


    /* Note to Byron:
     * The following are keyboard input controls used for debugging controlling the UI's position
     * Please ignore this code, it isn't directly used in the final system, but I'm saving the code for posterity 
     * and to keep record of my work flow.      
     */

    /*
    void checkArrowKeyControls()
    {
        //arrow key controls move the UI's position in the vertical and horizontal directions
        //note - arrow key controls don't work when the encoder is being used

        if (Input.GetKey(KeyCode.UpArrow)) //up arrow key pressed
        {
            yPos += stepAmount;
            newUIPosition = new Vector2(xPos, yPos);
            uiElement.position = newUIPosition;
        }

        if (Input.GetKey(KeyCode.DownArrow)) //down arrow key pressed
        {
            yPos -= stepAmount;
            newUIPosition = new Vector2(xPos, yPos);
            uiElement.position = newUIPosition;
        }

        if (Input.GetKey(KeyCode.RightArrow)) //right arrow key pressed
        {
            xPos += stepAmount;
            newUIPosition = new Vector2(xPos, yPos);
            uiElement.position = newUIPosition;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) //left arrow key pressed
        {
            xPos -= stepAmount;
            newUIPosition = new Vector2(xPos, yPos);
            uiElement.position = newUIPosition;
        }

        // W & S key controls inc/dec testInput which is added to encoderPos to move the UI up & down 
        // These are used to test moving the UI vertically using InverseLerp & Lerp when the encoder is not hooked up

        if (Input.GetKey(KeyCode.W)) //W arrow key pressed
        {
            testInput += 1;
        }

        if (Input.GetKey(KeyCode.S)) //S arrow key pressed
        {
            testInput -= 1;
        }

    }
    */

}
