using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using ViveSR.anipal.Eye;
using System;
using System.Collections;
using System.Collections.Generic;
using static CountDown;

public class EyeTrackingCaleb : MonoBehaviour
{
    public GameObject centerPoint;

    public Material green;
    public Material orange;
    private LineRenderer gazeRayRenderer;
    private readonly float MaxDistance = 20;
    private static EyeData eyeData = new EyeData();
    private bool eye_callback_registered = false;
    // private StreamWriter outputStream;
    private string outputPath;
    private string datetimenow;
    private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
    private bool looking_at_stim = false;
    private float testStartTime;

    private bool testStarted = false;
    public CountDown countDown;
    private saveEyeTrackingData saveETData = new saveEyeTrackingData();

    void Start()
    {
        gazeRayRenderer = GetComponent<LineRenderer>();
        Debug.Log("Eye tracking started!");
        datetimenow = DateTime.Now.ToString("yyyyMMdd_HHmm");
        outputPath = $"Saved Data/run_{datetimenow}/eye_tracking_data.json";
        StartCoroutine(WaitForSpaceToStart());
    }

    // public void SetOutputPath(string path)
    // {
    //     outputPath = Path.Combine(path, "EyeTrackingData.txt");
    //     if (outputStream != null)
    //     {
    //         outputStream.Close();
    //     }
    //     outputStream = new StreamWriter(outputPath, true);
    //     outputStream.WriteLine($"{centerPoint.name}");
    // }


    private void Update() // called each frame, firstly checks eyetracking status is okay before logging any contact of ray gaze to raw eyetrackingdata.txt, also calls centralstimulus script to change colour
    {
        // Debug.Log(SRanipal_Eye_Framework.Status);
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && !eye_callback_registered)
        {
            SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
            // Debug.Log("Runs here");
        }
        else if (!SRanipal_Eye_Framework.Instance.EnableEyeDataCallback && eye_callback_registered)
        {
            SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        Ray gazeRay;
        bool eye_focus = false;
        FocusInfo focusInfo = new FocusInfo();
        int stimulusLayerId = LayerMask.NameToLayer("stimulusToTrack");
        LayerMask layerMask = 1 << stimulusLayerId;

        GazeIndex gazeIndex = GazeIndex.COMBINE;

        if (eye_callback_registered)
        {
            eye_focus = SRanipal_Eye.Focus(gazeIndex, out gazeRay, out focusInfo, 0, MaxDistance, layerMask, eyeData);
        }
        else
        {
            eye_focus = SRanipal_Eye.Focus(gazeIndex, out gazeRay, out focusInfo, 0, MaxDistance, layerMask);
        }
        looking_at_stim = false;
        if (centerPoint != null && focusInfo.transform != null && eye_focus)
        {
            looking_at_stim = (focusInfo.transform.gameObject == centerPoint);
            HighlightObject(eye_focus && focusInfo.transform.gameObject == centerPoint);
        }
        else
        {
            HighlightObject(false);
        }

        if (eye_focus && testStarted)
        {
            Vector3 localFocusPoint = centerPoint.transform.InverseTransformPoint(focusInfo.point);
            float actualTime = Time.time - testStartTime;
            saveETData.timeywimey.Add(actualTime);
            saveETData.coords.Add(new Vector2(localFocusPoint.x, localFocusPoint.y));
            saveETData.lookingAtStim.Add(looking_at_stim);
        }
    }
    IEnumerator WaitForSpaceToStart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                yield return StartCoroutine(countDown.startCountDown());
                Debug.Log("Test started!");
                testStartTime = Time.time;
                testStarted = true;
                yield break; 
            }
            yield return null; 
        }
    }

    void HighlightObject(bool highlight)
    {
        // centralStimulus centralStimulus = centerPoint.GetComponent<centralStimulus>();
        if (centerPoint != null)
        {
            if (highlight){
                centerPoint.GetComponent<MeshRenderer> ().material = green;
            }
            // centralStimulus.Highlight(highlight);
            else{
                centerPoint.GetComponent<MeshRenderer> ().material = orange;
            }
        }
    }

    public bool IsLookingAtStimulus()
    {
        return looking_at_stim;
    }

    public void saveEyeJson(){
        Debug.Log(testStartTime);
        string json = JsonUtility.ToJson(saveETData, true);

        // Save the JSON file
        string filePath = Path.Combine(Application.dataPath, outputPath);
        System.IO.File.WriteAllText(filePath, json);
    }


    private void OnApplicationQuit()
    {
        // Debug.Log(testStartTime);
        // string json = JsonUtility.ToJson(saveETData, true);

        // // Save the JSON file
        // string filePath = Path.Combine(Application.dataPath, outputPath);
        // System.IO.File.WriteAllText(filePath, json);
    }

    private static void EyeCallback(ref EyeData eye_data)
    {
        eyeData = eye_data;
    }

    public void StopTracking()
    {
        // if (outputStream != null)
        // {
        //     outputStream.Close();
        //     outputStream = null;
        // }

        // centralStimulus centralStimulus = centerPoint.GetComponent<centralStimulus>();
        // if (centralStimulus != null)
        // {
        //     centralStimulus.Finished();
        // }
        if (centerPoint != null){
            centerPoint.SetActive(false);
        }

        this.enabled = false;
    }

}

[System.Serializable]
public class saveEyeTrackingData
{
    public List<float> timeywimey = new List<float>();
    public List<Vector2> coords = new List<Vector2>();
    public List<bool> lookingAtStim = new List<bool>();
    
}

