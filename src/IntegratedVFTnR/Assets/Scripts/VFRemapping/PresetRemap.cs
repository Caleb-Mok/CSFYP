using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetRemap : MonoBehaviour
{
    public GameObject leftEyeCamera; //RenderPlane under dualcamera rig SRFramework
    public GameObject rightEyeCamera; //same above but right

    // vars
    public float scaleFactor;
    public float targetScale = 0.6f;
    public float targetPos = -0.5f;
    public float remapSpeed = 0.1f;
    public float posSpeed= 0.001f;
    private Vector3 positionIncrement;
    private Vector3 scaleIncrement;
    private Vector3 defaultPosition;
    private Vector3 defaultScale;



    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = new Vector3(0f, 0f, 2f);
        defaultScale = new Vector3(1f, 1f, 1f);
        scaleIncrement = new Vector3(0.5f, 0.5f, 0.5f);
        positionIncrement = new Vector3(0.005f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Adjust by using keyboard
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            remapHHRight();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            bitemporalAdj();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            Debug.Log("Smooth adjustment, HH-Right");
            smoothHHRight();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            Debug.Log("Smooth adjustment, Quadrantanopie-Left-Down");
            smoothQuadLeftDown();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            Debug.Log("Smooth adjustment, Bitemporal");
            smoothBitemporal();
        }
    }

    void remapHHRight() {
        Debug.Log("HH-Right set");
        // Define remapping parameters
        Vector3 hhRightRemapScale = defaultScale * 0.5f;
        Vector3 xAdjust = defaultPosition - new Vector3(0.6f, 0f, 0f); // around 50%?

        // Transform the scale & position of cameras on each eye
        leftEyeCamera.transform.localPosition = xAdjust;
        rightEyeCamera.transform.localPosition = xAdjust;
        leftEyeCamera.transform.localScale = hhRightRemapScale;
        rightEyeCamera.transform.localScale = hhRightRemapScale;
    }

    void bitemporalAdj() {
        Debug.Log("Bitemporal set");
        Vector3 bitemporalRemapScale = defaultScale * 0.5f;
        // Adjust towards the center
        Vector3 leftXAdjust = defaultPosition + new Vector3(0.6f, 0f, 0f);
        Vector3 rightXAdjust = defaultPosition - new Vector3(0.6f, 0f, 0f);
        
        leftEyeCamera.transform.localPosition = leftXAdjust;
        rightEyeCamera.transform.localPosition = rightXAdjust;
        leftEyeCamera.transform.localScale = bitemporalRemapScale;
        rightEyeCamera.transform.localScale = bitemporalRemapScale;
    }

    #region Hemianopia
    // Right-side Hemianopia autoremap
    public void smoothHHRight() {
        // Run coroutine for both scaling and positioning
        StartCoroutine(scaleAdjustmentHHRight());
        StartCoroutine(posAdjustmentHHRight());
    }

    IEnumerator scaleAdjustmentHHRight() {
        while (leftEyeCamera.transform.localScale.x >= 0.6f) {
            leftEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;
            rightEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;

            yield return null;
        }
        Debug.Log("Smoowth scaled!");
    }

    IEnumerator posAdjustmentHHRight() {
        while (leftEyeCamera.transform.localPosition.x >= targetPos) {

            Vector3 leftPos = leftEyeCamera.transform.localPosition;
            Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // Subtract from the x component
            leftPos -= new Vector3(posSpeed, 0f, 0f);
            rightPos -= new Vector3(posSpeed, 0f, 0f);

            // Assign the updated position back
            leftEyeCamera.transform.localPosition = leftPos;
            rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
        Debug.Log("Smoowth pos'd!");
    }

    // Left-side Hemianopia autoremap
    public void smoothHHLeft() {
        // Run coroutine for both scaling and positioning
        StartCoroutine(scaleAdjustmentHHLeft());
        StartCoroutine(posAdjustmentHHLeft());
    }

    IEnumerator scaleAdjustmentHHLeft() {
        while (leftEyeCamera.transform.localScale.x >= 0.6f) {
            leftEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;
            rightEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;

            yield return null;
        }
        Debug.Log("Smoowth scaled!");
    }

    IEnumerator posAdjustmentHHLeft() {
        while (leftEyeCamera.transform.localPosition.x <= 0.5) {

            Vector3 leftPos = leftEyeCamera.transform.localPosition;
            Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // ADD from the x component
            leftPos += new Vector3(posSpeed, 0f, 0f);
            rightPos += new Vector3(posSpeed, 0f, 0f);

            // Assign the updated position back
            leftEyeCamera.transform.localPosition = leftPos;
            rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
        Debug.Log("Smoowth pos'd!");
    }

    #endregion

    #region Quadrantanopia
    public void smoothQuadLeftDown() {
        StartCoroutine(scaleAdjustmentQuadLeftDown());
        StartCoroutine(posAdjustmentQuadLeftDown());
    }

    IEnumerator scaleAdjustmentQuadLeftDown() {
        float quadTargetScale = 0.65f;
        while (leftEyeCamera.transform.localScale.x >= quadTargetScale) {
            leftEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;
            rightEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;

            yield return null;
        }
        Debug.Log("Smoowth scaled!");
    }

    IEnumerator posAdjustmentQuadLeftDown() {
        float quadTargetPos = 1f;
        while (leftEyeCamera.transform.localPosition.x <= quadTargetPos) {

            Vector3 leftPos = leftEyeCamera.transform.localPosition;
            Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // ADD from the x component
            leftPos += new Vector3((posSpeed*2), 0f, 0f);
            rightPos += new Vector3((posSpeed*2), 0f, 0f);

            // Assign the updated position back
            leftEyeCamera.transform.localPosition = leftPos;
            rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
    }

    public void smoothQuadRightTop() {
        StartCoroutine(scaleAdjustmentQuadRightTop());
        StartCoroutine(posAdjustmentQuadRightTop());
    }

    IEnumerator scaleAdjustmentQuadRightTop() {
        float quadTargetScale = 0.65f;
        while (leftEyeCamera.transform.localScale.x >= quadTargetScale) {
            leftEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;
            rightEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;

            yield return null;
        }
        Debug.Log("Smoowth scaled!");
    }

    IEnumerator posAdjustmentQuadRightTop() {
        float quadTargetPos = -1f;
        while (leftEyeCamera.transform.localPosition.x >= quadTargetPos) {

            Vector3 leftPos = leftEyeCamera.transform.localPosition;
            Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // SUBTRACT to go to the left side
            leftPos -= new Vector3((posSpeed*2), 0f, 0f);
            rightPos -= new Vector3((posSpeed*2), 0f, 0f);

            // Assign the updated position back
            leftEyeCamera.transform.localPosition = leftPos;
            rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
    }

    #endregion

    #region Bitemporal

    /// <summary>
    /// WARNING! The use of bitemporal adjustments can cause SEVERE cybersickness and potentially
    /// prolonged headache (i meant it!). Use with EXTRA caution.
    /// </summary>
    public void smoothBitemporal() {
        StartCoroutine(scaleAdjustmentBitemporal());
        StartCoroutine(posAdjustmentBitemporalLeft());
        StartCoroutine(posAdjustmentBitemporalRight());
    }
    
    IEnumerator scaleAdjustmentBitemporal() {
        float bitempTargetScale = 0.4f;
        while (leftEyeCamera.transform.localScale.x >= bitempTargetScale) {
            leftEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;
            rightEyeCamera.transform.localScale -= new Vector3(remapSpeed, remapSpeed,remapSpeed) * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator posAdjustmentBitemporalLeft() {
        float bitempTargetPosL = -0.06f;
        while (leftEyeCamera.transform.localPosition.x >= bitempTargetPosL) {

            Vector3 leftPos = leftEyeCamera.transform.localPosition;
            // Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // Subtract from the x component
            leftPos += new Vector3(posSpeed, 0f, 0f);
            // rightPos -= new Vector3(posSpeed, 0f, 0f);

            // Assign the updated position back
            leftEyeCamera.transform.localPosition = leftPos;
            // rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
    }
        IEnumerator posAdjustmentBitemporalRight() {
        float bitempTargetPosR = -0.4f;
        while (leftEyeCamera.transform.localPosition.x >= bitempTargetPosR) {

            // Vector3 leftPos = leftEyeCamera.transform.localPosition;
            Vector3 rightPos = rightEyeCamera.transform.localPosition;

            // Subtract from the x component
            // leftPos -= new Vector3(posSpeed, 0f, 0f);
            rightPos -= new Vector3(posSpeed, 0f, 0f);

            // Assign the updated position back
            // leftEyeCamera.transform.localPosition = leftPos;
            rightEyeCamera.transform.localPosition = rightPos;

            yield return null;
        }
    }

    #endregion

}
