using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;
using static CountDown;

public class StimulusUpdater : MonoBehaviour
{
    public GameObject stimulus;
    public GameObject centerPoint;
    public Material green;
    public Material red;
    public GameObject wall;
    // private UserInput controls; 

    private int rows = 10;
    private int columns = 10;

    private float updateInterval = 0.75f; // in seconds.
    private float flashDuration = 0.05f; //how long does the stimulus flash for in seconds
    private int timesPerCoord = 3; // how many times to test per coordinate
    private int numCoords; // num of points to test
    
    private Vector3 wallStart;
    private Vector3 wallSize;
    private List<Vector2> gridPoints = new List<Vector2>();

    private int count = 0;
    private saveData data = new saveData();
    private string outputPath;
    private string datetimenow;
    private float startTime;
    private float testStartTime;
    private float testDuration;
    private bool started;
    public CountDown countDown;
    public SceneTransitionManager changeScene;

    public RunPythonScript runPythonScript; // Used in start to let scripts know where data will be
    public EyeTrackingCaleb eyeTracking; // Used in start to let scripts know where data will be


    void Start()
    {

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);


        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.characteristics.ToString()));
        }

        // controls = new UserInput();
        // controls.Enable();

        if  (stimulus == null || wall == null)
        {
            Debug.LogError("stimulus or Wall is not assigned.");
            return;
        }

        // Get wall bounds
        var wallRenderer = wall.GetComponent<Renderer>();
        if (wallRenderer != null)
        {
            // wallSize = wallRenderer.localBounds.size;
            // wallStart = wallRenderer.localBounds.min;
            wallSize = wall.transform.localScale;
            wallStart = new Vector3(-wallSize.x/2, -wallSize.y/2+wall.transform.localPosition.y, wall.transform.localPosition.z);

        }
        
        else
        {
            Debug.LogError("Wall does not have a Renderer component.");
            return;
        }

        for (int i = -rows/2; i <= rows/2; i++)
        {
            for (int j = -columns/2; j <= columns/2; j++)
            {   
                if ((Math.Sqrt(i*i+j*j))<=rows/2+0.5){  // to make sure the stimuluss are generated within a circle
                    if (!(i==0 & j==0)){   // remove fixation center
                        for (int k = 0; k < timesPerCoord; k++){
                            gridPoints.Add(new Vector2(i, j));  
                        }
                    }
                }
                
            }
        }

        data.centerSize = centerPoint.transform.localScale.x;
        numCoords = gridPoints.Count;

        ShuffleList(gridPoints);
        datetimenow = DateTime.Now.ToString("yyyyMMdd_HHmm");
        // outputPath = $"/Saved Data/run_{datetimenow}/run_data.json";
        outputPath = $"Saved Data/run_{datetimenow}";
        // data.runName = $"run_{datetimenow}";

        runPythonScript.SetRunPath(System.IO.Path.Combine(Application.dataPath, outputPath));

        stimulus.SetActive(false);
        centerPoint.GetComponent<MeshRenderer> ().material = red;
        StartCoroutine(WaitForSpaceToStart());
        
    }

    // void Update(){

    //     if (count < numCoords && gridPoints.Count > 0)
    //     {
    //         StartCoroutine(Updat stimulusPosition());
    //     }

    //     if (count >= numCoords)
    //     {
    //         Debug.Log("Stopping Now");
    //         testDuration = Time.time - testStartTime;
    //         Debug.Log($"Total Test Duration: {testDuration:0.00} seconds");
    //         controls.Disable();
    //         // CancelInvoke();
    //         data.testDuration = testDuration;

    //         // Convert data to JSON format
    //         string json = JsonUtility.ToJson(data, true); // Pretty print format

    //         // Save the JSON file
    //         // string filePath = Application.persistentDataPath + "/detected_data.json";
    //         string filePath = Application.dataPath + "/Saved Data/detected_data.json";

    //         System.IO.File.WriteAllText(filePath, json);

    //         Debug.Log($"JSON saved at: {filePath}");
    //         Application.OpenURL(filePath); // Open file location

    //         // Stop the Unity scene (Only works in Editor)
    //         #if UNITY_EDITOR
    //         UnityEditor.EditorApplication.isPlaying = false;
    //         #else
    //         Application.Quit(); // Closes application if built
    //         #endif
    //     }
    // }
    IEnumerator WaitForSpaceToStart()
    {
        while (true)
        {
            // if (controls.DetectedDot.PressSpace.triggered)
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                yield return StartCoroutine(countDown.startCountDown());
                Debug.Log("Test started!");
                testStartTime = Time.time;
                Debug.Log(testStartTime);
                stimulus.SetActive(true);
                centerPoint.GetComponent<MeshRenderer> ().material = green;
                StartCoroutine(StartStimulusTestSequence()); 
                yield break; 
            }
            yield return null; 
        }
    }

    IEnumerator StartStimulusTestSequence()
    {
        
        while (count < numCoords && gridPoints.Count > 0)
        {
            yield return StartCoroutine(UpdateStimulusPosition());
        }

        Debug.Log("Stopping Now");
        testDuration = Time.time - testStartTime;
        Debug.Log($"Total Test Duration: {testDuration:0.00} seconds");
        // controls.Disable();
        centerPoint.GetComponent<MeshRenderer> ().material = red;
        stimulus.SetActive(false);
        
        data.testDuration = testDuration;
        data.numberOfPoints = numCoords;
        data.flashDuration = flashDuration;

        // Convert data to JSON format
        string json = JsonUtility.ToJson(data, true);

        // Save the JSON file
        
        string filePath = System.IO.Path.Combine(Application.dataPath, outputPath);

        // if doesnt exist, create
        // System.IO.FileInfo file = new System.IO.FileInfo(filePath);
        // file.Directory.Create(); 
        Debug.Log(filePath);
        System.IO.Directory.CreateDirectory(filePath);
        System.IO.File.WriteAllText(System.IO.Path.Combine(filePath, "run_data.json"), json);

        eyeTracking.saveEyeJson();

        Debug.Log($"Run data JSON saved at: {filePath}");
        // Application.OpenURL(filePath); // Open file location

        runPythonScript.RunPython();
        // Stop the Unity scene
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #else
        // Application.Quit(); // Closes application if built
        // #endif
        changeScene.GoToScene(0);
    }


    IEnumerator UpdateStimulusPosition()
    {   
        if (count >= numCoords || gridPoints.Count == 0)
        {
            yield break;
        }

        Vector2 randomCoord = gridPoints[count];

        // // Adjust shape in the periphery
        // if (r > 0.7f) // Peripheral vision (outer 30%) EXPLAIN WHYYY
        // {
        //     stimulus.transform.localScale = new Vector3 stimulusSize * 2.5f, stimulusSize * 1.2f, 1); // Rectangular shape
        //     stimulus.transform.rotation = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg); // Rotate along curvature
        // }

        // Calculate new stimulus position based on grid
        float xStep = wallSize.x / (columns+1);
        float yStep = wallSize.y / (rows+1);

        float newX = wallStart.x + (randomCoord.x + rows/2 + 0.5f) * xStep;
        float newY = wallStart.y + (randomCoord.y + columns/2 + 0.5f) * yStep;

        Debug.Log(count + " " + newX + " " + newY);
        data.coords.Add(randomCoord);
        data.stimulusTime.Add(Time.time - testStartTime);
        
        // Update stimulus position
        stimulus.SetActive(true);
        // Debug.Log( stimulus is activated");
        // stimulus.transform.position = new Vector3(newX, newY, stimulus.transform.position.z);
        stimulus.transform.localPosition = new Vector3(newX, newY, 0);

        yield return StartCoroutine(WaitForTimer(flashDuration));
        // Debug.Log( stimulus is deactivated");
        stimulus.SetActive(false);
        

        yield return StartCoroutine(WaitForSpaceKey());

        count += 1;
    }

    IEnumerator WaitForTimer(float duration)
    {
        float timer = 0.0f;
        while(timer <= duration){
            timer += Time.deltaTime;
            yield return null;
        }
        // Debug.Log(flashTimer);
    }

    IEnumerator WaitForSpaceKey()
    {
        float timer = 0.0f;
        float waitTime = updateInterval;
        bool spacePressed = false;
        float currentRT = 0.0f;

        while (Input.GetKey(KeyCode.Space))
        {
            yield return null; // Keep waiting until the space key is fully released so theres no false inputs
        }

        float startTime = Time.time;

        while (timer < waitTime)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !spacePressed)
            {
                spacePressed = true;
                currentRT = Time.time - startTime;
                // break;
            }

            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        if (spacePressed){
            Debug.Log("Detected");
        }
        else{
            Debug.Log("Not Detected");
        }

        float currentReactionTime = spacePressed ? (currentRT) : -1.0f; // -1 for no response
        Debug.Log($"Space pressed: {spacePressed}, Reaction Time: {currentReactionTime:0.000}s");

        // Add detection data to Json
        data.reactionTime.Add(currentReactionTime);
        data.detected.Add(spacePressed);
        
    }

    void ShuffleList(List<Vector2> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Vector2 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}


[System.Serializable]
public class saveData
{
    public float testDuration;
    public int numberOfPoints;
    public float flashDuration;
    public float centerSize;
    public List<Vector2> coords = new List<Vector2>();
    public List<bool> detected = new List<bool>();
    public List<float> reactionTime = new List<float>();
    public List<float> stimulusTime = new List<float>();
    
}
