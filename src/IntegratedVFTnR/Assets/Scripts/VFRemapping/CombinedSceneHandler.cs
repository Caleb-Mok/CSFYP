using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class CombinedSceneHandler : MonoBehaviour
{
    [SerializeField] controllerRemap leftRemap;
    [SerializeField] controllerRemap rightRemap;
    [SerializeField] GameObject remap;
    [SerializeField] GameObject autoRemap;
    [SerializeField] GameObject menu;
    private ExperimentHandler experimentHandler;
    private PresetRemap presetRemap;
    

    void Start(){
        experimentHandler = this.GetComponent<ExperimentHandler>();
        presetRemap = this.GetComponent<PresetRemap>();
    }

    void Awake()
    {

    }

    public void leaveRemap(){
        // experimentHandler.isTiming = false;
        // Debug.Log("Is it timing?: " + experimentHandler.isTiming);
        remap.SetActive(false);
        menu.SetActive(true);
    }

    public void enterXRemap(){
        remap.SetActive(true);
        menu.SetActive(false);
        leftRemap.scaleXAndY = false;
        rightRemap.scaleXAndY = false;
    }

    public void enterXYRemap(){
        remap.SetActive(true);
        menu.SetActive(false);
        experimentHandler.StartTimeTrackingManual();
        Debug.Log("Is it timing?: " + experimentHandler.isTiming);
        leftRemap.scaleXAndY = true;
        rightRemap.scaleXAndY = true;
    }
    
    #region AutoRemap
    /// <summary>
    /// For use to test preset automation of remap
    /// 
    /// Condition 1: Homonymous Hemianopia Right
    ///  Condition 2: Homonymous Hemianopia Left
    /// Condition 3: Quadrantanopia Left Down (Left Parietal)
    /// Condition 4: Quadrantanopia Right Top (Right Temporal)
    /// 
    /// </summary>

    public void enterAutoRemapCond1() {
        remap.SetActive(true);
        menu.SetActive(false);
        experimentHandler.HideConditionsPanel();
        experimentHandler.StartTimeTrackingAuto();
        presetRemap.smoothHHRight();
        leftRemap.scaleXAndY = true;
        rightRemap.scaleXAndY = true;
    }

    public void enterAutoRemapCond2() {
        remap.SetActive(true);
        menu.SetActive(false);
        experimentHandler.HideConditionsPanel();
        experimentHandler.StartTimeTrackingAuto();
        presetRemap.smoothHHLeft();
        leftRemap.scaleXAndY = true;
        rightRemap.scaleXAndY = true;
    }

    public void enterAutoRemapCond3() {
        remap.SetActive(true);
        menu.SetActive(false);
        experimentHandler.HideConditionsPanel();
        experimentHandler.StartTimeTrackingAuto();
        presetRemap.smoothQuadLeftDown();
        leftRemap.scaleXAndY = true;
        rightRemap.scaleXAndY = true;
    }

    public void enterAutoRemapCond4() {
        remap.SetActive(true);
        menu.SetActive(false);
        experimentHandler.HideConditionsPanel();
        experimentHandler.StartTimeTrackingAuto();
        presetRemap.smoothQuadRightTop();
        leftRemap.scaleXAndY = true;
        rightRemap.scaleXAndY = true;
    }
//
    #endregion
}