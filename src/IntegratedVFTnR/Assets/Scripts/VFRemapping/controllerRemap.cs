using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using static System.Math;

public class controllerRemap : MonoBehaviour
{
    public bool scaleXAndY;
    [Header("UI Elements")]
    [SerializeField] GameObject scaleXUI;
    [SerializeField] GameObject scaleXYUI;
    [SerializeField] SpriteRenderer leftArrow;
    [SerializeField] SpriteRenderer rightArrow;
    [SerializeField] SpriteRenderer scaleUpArrow;
    [SerializeField] SpriteRenderer scaleDownArrow;
    [SerializeField] SpriteRenderer scaleDownXY;
    [SerializeField] SpriteRenderer scaleUpXY;
    [SerializeField] Color selectColour;
    [SerializeField] bool rightEye;
    public float scaleFactor;
    private Vector3 positionIncrement;
    private Vector3 scaleIncrement;
    private Vector3 defaultPosition;
    private Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        scaleIncrement = scaleXAndY ? new Vector3(0.001f,0.001f,0f) : new Vector3(0.001f, 0f, 0f);
        positionIncrement = new Vector3(0.005f, 0f, 0f);
    }

    void Awake()
    {
        defaultPosition = transform.localPosition;
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        scaleIncrement = scaleXAndY ? new Vector3(0.001f,0.001f,0f) : new Vector3(0.001f, 0f, 0f);
        updateUI();
        experimentPresets();
        newExperimentPresets();

        // updateTransformInformation();
        //Keyboard Input
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

        // CONTROLLER INPUT

        // Reset the image transform of the camera feed
        if ((SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)) || (Input.GetKeyDown(KeyCode.R))){
            resetTransform();
        }

        //REMAPPING/TRANSFORMING CONTROLLER INPUT MANAGEMENT

        //if UP is pressed on controller: increase the scale of the camera feed
        if (SteamVR_Actions._default.TouchpadNorth.GetState(SteamVR_Input_Sources.Any)){
            transform.localScale += scaleIncrement;

            if (scaleXAndY){
                scaleUpXY.color = selectColour;
            }
            else{
                scaleUpArrow.color = selectColour;
            }
        }
        else{
            scaleUpArrow.color = Color.white;
            scaleUpXY.color = Color.white;
        }
        
        //if DOWN is pressed on controller: decrease the scale of the camera feed
        if (SteamVR_Actions._default.TouchpadSouth.GetState(SteamVR_Input_Sources.Any)){
            transform.localScale -= scaleIncrement;

            if (scaleXAndY){
                scaleDownXY.color = selectColour;
            }
            else{
                scaleDownArrow.color = selectColour;
            }
        }
        else{
            scaleDownArrow.color = Color.white;
            scaleDownXY.color = Color.white;
        }

        //if RIGHT is pressed on controller: increase the X-position of the camera feed
        if (SteamVR_Actions._default.TouchpadEast.GetState(SteamVR_Input_Sources.Any)){
            transform.localPosition += positionIncrement;
            rightArrow.color = selectColour;
        }

        else{
            rightArrow.color = Color.white;
        }

        //if LEFT is pressed on controller: decrease the X-position of the camera feed
        if (SteamVR_Actions._default.TouchpadWest.GetState(SteamVR_Input_Sources.Any)){
            transform.localPosition -= positionIncrement;
            leftArrow.color = selectColour;
        }

        else{
            leftArrow.color = Color.white;
        }
    }

    public void resetTransform(){
        transform.localPosition = defaultPosition;
        transform.localScale = defaultScale;
    }

    //alters the UI depending on the current scaling method to improve understandability
    public void updateUI(){
        if (scaleXAndY){
            scaleXUI.SetActive(false);
            scaleXYUI.SetActive(true);
        }
        else{
            scaleXUI.SetActive(true);
            scaleXYUI.SetActive(false);
        }
    }


    private void OnGUI(){
        float defaultPositionX = defaultPosition.x;
        float defaultScaleX = defaultScale.x;
        float currentPositionX = transform.localPosition.x;
        float currentScaleX = transform.localScale.x;
        string scaleString = "Scale Factor 1";

        float positionDifference = Abs(defaultPositionX - currentPositionX);
        // float positionPercentageChange = (positionDifference / defaultPositionX) * 100;
        string eyeType = rightEye ? "RIGHT" : "LEFT";
        if (scaleXAndY){
            if (defaultScaleX > currentScaleX){
                scaleFactor = currentScaleX / defaultScaleX;
                scaleString = eyeType + " Scale Down Factor " +  scaleFactor;
                print(eyeType + " Scale Down Factor " +  scaleFactor);
            }
            else{
                scaleFactor = currentScaleX / defaultScaleX;
                scaleString = eyeType + " Scale Up Factor " +  scaleFactor;
                print(eyeType + " Scale Up Factor " +  scaleFactor);
            }
        }


        // float scaleDifference
        print(positionDifference);

        GUI.color = Color.white;
        GUIStyle StyleBold = new GUIStyle { fontStyle = FontStyle.Bold };
        StyleBold.normal.textColor = Color.white;
        if (!rightEye){
            GUILayout.Label(new GUIContent("\n"), StyleBold);
            GUILayout.Label(new GUIContent(scaleString), StyleBold);
        }
        else{
            GUILayout.Label(new GUIContent(scaleString), StyleBold);
        }
    }

    
    private void experimentPresets(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            Vector3 threeQuarterScale = defaultScale*0.75f;
            Vector3 xAdjust = defaultPosition - new Vector3 (1.4f, 0f, 0f);
            transform.localPosition = xAdjust;
            transform.localScale = threeQuarterScale;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            Vector3 halfScale = defaultScale*0.5f;
            Vector3 xAdjust = defaultPosition - new Vector3 (1.2f, 0f, 0f);
            transform.localPosition = xAdjust;
            transform.localScale = halfScale;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            Vector3 oneQuarterScale = defaultScale*0.25f;
            Vector3 xAdjust = defaultPosition - new Vector3 (0.8f, 0f, 0f);
            transform.localPosition = xAdjust;
            transform.localScale = oneQuarterScale;
        }
    }

    private void newExperimentPresets() {
        // Homonymous Hemianopia
        // HH-Right
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            Debug.Log("HH-Right set");
            Vector3 hhRightRemap = defaultScale * 0.5f;
            Vector3 xAdjust = defaultPosition - new Vector3(1.2f, 0f, 0f);
            transform.localPosition = xAdjust;
            transform.localScale = hhRightRemap;
        }
        // HH-Left
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            Vector3 hhLeftRemap = defaultScale * 0.5f;
            Vector3 xAdjust = defaultPosition + new Vector3(0.5f, 0f, 0f);
            transform.localPosition = xAdjust;
            transform.localScale = hhLeftRemap;
        }
    }
}
