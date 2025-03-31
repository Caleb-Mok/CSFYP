using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void goToXScene(){
        SceneManager.LoadScene("RemapScene",LoadSceneMode.Single);
    }

    public void goToXYScene(){
        SceneManager.LoadScene("RemapSceneXY",LoadSceneMode.Single);

    }

    public void exitApplication(){
        Application.Quit();
    }
}
