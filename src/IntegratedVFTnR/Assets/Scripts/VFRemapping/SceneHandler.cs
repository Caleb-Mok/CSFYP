using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    private Button btn;
    private GameObject srFramework;

    void Start(){
        srFramework = GameObject.Find("[SRwork_FrameWork]");
        // if (srFramework != null){
        //     // srFramework.SetActive(false);
        //     srFramework.transform.localScale = new Vector3(0,0,0);
        // }
    }

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }
    

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null){
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            Debug.Log("click");
            }
        // if (e.target.name == "Cube")
        // {
        //     Debug.Log("Cube was clicked");
        // } else if (e.target.name == "Button")
        // {
        //     Debug.Log("Button was clicked");
        // }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        btn = e.target.GetComponent<Button>();
        if (btn != null){
            btn.Select();
            Debug.Log("enter");
        }
        // if (e.target.name == "Cube")
        // {
        //     Debug.Log("Cube was entered");
        // }
        // else if (e.target.name == "Button")
        // {
        //     Debug.Log("Button was entered");
        // }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        btn = e.target.GetComponent<Button>();
        if (btn != null){
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("exit");

        }
        // if (e.target.name == "Cube")
        // {
        //     Debug.Log("Cube was exited");
        // }
        // else if (e.target.name == "Button")
        // {
        //     Debug.Log("Button was exited");
        // }
    }
}