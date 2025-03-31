using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RemapRight : MonoBehaviour
{
    private Vector3 positionIncrement;
    private Vector3 scaleIncrement;
    // Start is called before the first frame update
    void Start()
    {
        scaleIncrement = new Vector3(0.001f, 0f, 0f);
        positionIncrement = new Vector3(0.005f, 0f, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.RightArrow)){
            // transform.position += positionIncrement;
            // transform.Translate(0.01f,0f,0f);
            transform.localPosition += positionIncrement;
        }
        if (Input.GetKey (KeyCode.LeftArrow)){
            // transform.position -= positionIncrement;
            // transform.Translate(-0.01f,0f,0f);
            transform.localPosition -= positionIncrement;
        }
        if (Input.GetKey (KeyCode.UpArrow)){
            transform.localScale += scaleIncrement;
        }
        if (Input.GetKey (KeyCode.DownArrow)){
            transform.localScale -= scaleIncrement;
        }

        if (SteamVR_Actions._default.TouchpadNorth.GetState(SteamVR_Input_Sources.Any)){
            transform.localScale += scaleIncrement;
        }
        if (SteamVR_Actions._default.TouchpadSouth.GetState(SteamVR_Input_Sources.Any)){
            transform.localScale -= scaleIncrement;
        }
        if (SteamVR_Actions._default.TouchpadEast.GetState(SteamVR_Input_Sources.Any)){
            transform.localPosition += positionIncrement;
        }
        if (SteamVR_Actions._default.TouchpadWest.GetState(SteamVR_Input_Sources.Any)){
            transform.localPosition -= positionIncrement;
        }
    }
}
