//This script has timer functions that control when certain events should happen

//Unity coroutines reference - https://youtu.be/k8St8iQkpxg?si=NzH_1b6Zj1lH64ai

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timeValueRockLaunch = 5; //the ~base~ number of seconds until the rock is launched again
                                          //(a random value is also added to this value later to add randomness/make things feel more natural)
    public float timeValueHideUI = 10; //the number of seconds the UI will be visible for before it's hidden

    //objects
    public RockController rock;
    public UIOverlayController UIcontroller;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RockLaunchTimer"); //start countdown for the first time the rock will be launched
    }

    // Update is called once per frame
    void Update()
    {
        //nothing in Update()
    }

    public IEnumerator RockLaunchTimer() //counts down to when the rock will be launched
    {
        
        yield return new WaitForSeconds(timeValueRockLaunch + Random.Range(0, 7)); //wait this many seconds before the rock is launched,
                                                                                   //a random # is added to the base interval to make the
                                                                                   //system feel more natural and less scripted
        Debug.Log("RockLaunchTimer done");
        rock.resetRockPosition(); //make sure the rock is at an appropriate position to be launched from
        rock.launchRock(); //launch the rock

    }

    public IEnumerator HideUITimer() //counts down to when the UI will be hidden again
    {
        yield return new WaitForSeconds(timeValueHideUI); //wait this many seconds before hiding the UI
        Debug.Log("HideUITimer done");
        UIcontroller.hideWindowOverlay(); //hide the UI
    }

}
