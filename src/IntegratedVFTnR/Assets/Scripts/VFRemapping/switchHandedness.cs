using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchHandedness : MonoBehaviour
{
    [SerializeField] GameObject leftHandUI;
    [SerializeField] GameObject rightHandUI;
    [SerializeField] eyeToggle toggleScript;
    [SerializeField] GameObject leftSwitchUI;
    [SerializeField] GameObject rightSwitchUI;
    private int state;
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            switch(state){
                case 0:
                    rightHandUI.SetActive(false);
                    leftHandUI.SetActive(true);
                    toggleScript.SwitchEye = leftSwitchUI;
                    state = 1;
                    break;
                case 1:
                    leftHandUI.SetActive(false);
                    rightHandUI.SetActive(true);
                    toggleScript.SwitchEye = rightSwitchUI;
                    state = 0;
                    break;
            }
        }
        
    }
}
