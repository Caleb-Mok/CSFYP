using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;

public class ExperimentHandler : MonoBehaviour
{

    [System.Serializable]
    public class ParticipantData {
        public string participantName;
        public int partCount;
        public float taskTime;
        public Vector3 remappedPositionL;
        public Vector3 remappedPositionR;
        public Vector3 remappedScaleL;
        public Vector3 remappedScaleR;

    }

    #region Unity stuff
    /// <summary>
    /// Vars below
    /// </summary>
    public GameObject timerParam;
    // public GameObject testPanel;
    public GameObject menuPanel;
    public GameObject conditionsPanel;
    public GameObject endPanel;
    public GameObject experimentRemap;
    private string countFilePath;
    private string participantCountFilePath;
    private float startTime;
    public bool isTiming = false;
    private float trackedTime = 0f;
    public bool isAuto = false;
    public GameObject leftCamera;
    public GameObject rightCamera;


    /// <summary>
    /// Core script
    /// </summary>
    void Start()
    {
        string saveFolderPath = Application.persistentDataPath + "/Saves";
        string manualFolderPath = Application.persistentDataPath + "/Saves/Manual";
        string autoFolderPath = Application.persistentDataPath + "/Saves/Auto";

        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);
        
        countFilePath = Path.Combine(saveFolderPath, "lastFileNumber.txt");
        participantCountFilePath = Path.Combine(saveFolderPath, "lastParticipantNumber.txt");
    }


    void Update()
    {
        if (isTiming) {
            trackedTime += Time.deltaTime;
        }
    }

    #endregion



    #region SaveData
    /// <summary>
    /// below is the functionalities, above is the core start/update funcs
    /// </summary>

    public void SaveData() {
        ParticipantData data = new ParticipantData();
        int participantNumber = GetNextParticipantNumber();
        data.participantName = "Participant " + participantNumber;
        data.taskTime = trackedTime;
        data.remappedPositionL = leftCamera.transform.localPosition;
        data.remappedPositionR = rightCamera.transform.localPosition;
        data.remappedScaleL = leftCamera.transform.localScale;
        data.remappedScaleR = rightCamera.transform.localScale;


        // Convert to JSON
        string json = JsonUtility.ToJson(data, true);

        int count = GetNextFileNumber();

        string filename = "";

        if (!isAuto) {
            filename = Path.Combine(Application.persistentDataPath + "/Saves/Manual", "Participant_" + count + "_Manual.json");
        }
        else if (isAuto) {
            filename = Path.Combine(Application.persistentDataPath + "/Saves/Auto", "Participant_" + count + "_Auto.json");
        }

        File.WriteAllText(filename, json);

        SaveFileNumber(count + 1);
        SaveParticipantNumber(participantNumber + 1);

        Debug.Log("Saved: " + filename + " | Participant : " + data.participantName);
        // Application.OpenURL(filename); // Open file location
    }

    int GetNextFileNumber()
    {
        if (File.Exists(countFilePath))
        {
            string fileContent = File.ReadAllText(countFilePath);
            if (int.TryParse(fileContent, out int count))
                return count;
        }
        return 1; // Default start value
    }

    void SaveFileNumber(int count) {
        File.WriteAllText(countFilePath, count.ToString());
    }

    int GetNextParticipantNumber()
    {
        if (File.Exists(participantCountFilePath))
        {
            string fileContent = File.ReadAllText(participantCountFilePath);
            if (int.TryParse(fileContent, out int count))
                return count;
        }
        return 1;
    }

    void SaveParticipantNumber(int count) {
        File.WriteAllText(participantCountFilePath, count.ToString());
    }

#endregion

#region Time tracking
    public void StartTimeTrackingManual() {
        trackedTime = 0f; //resets time each session
        Debug.Log("ManualTime started");
        if (!isTiming && !isAuto) {
            isTiming = true;
        }
        Debug.Log("Is it timing?: " + isTiming);
    }
    public void StartTimeTrackingAuto() {
        trackedTime = 0f;
        Debug.Log("AutoTime started");
        isAuto = true;
        if (!isTiming) {
            isTiming = true;
        }
    }

    public void ResumeTimeTracking() {
        if(!isTiming) {
            isTiming = true;
        }
    }
    public void StopTimeTracking() {
        if (isTiming) {
            isTiming = false; // stops timer
            Debug.Log("Tracked time: " + trackedTime);
        }
        // else if (!isTiming && )
        // else if (!isTiming) {
        //     Debug.Log("No time recorded!");
        // }

        // HideEndPanel();
        // menuPanel.SetActive(true);
    }

    #endregion

    #region Panel Controls
    // public void ShowTestPanel() {
    //     testPanel.SetActive(true);
    // }
    public void ShowEndPanel() {
        endPanel.SetActive(true);
    }

    public void HideEndPanel() {
        endPanel.SetActive(false);
    }
    
    public void ShowConditionsPanel() {
        conditionsPanel.SetActive(true);
    }

    public void HideConditionsPanel() {
        conditionsPanel.SetActive(false);
    }

    public void ShowCondsHideMenu() {
        menuPanel.SetActive(false);
        conditionsPanel.SetActive(true);
    }
    public void HideCondsShowMenu() {
        conditionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    #endregion
}
