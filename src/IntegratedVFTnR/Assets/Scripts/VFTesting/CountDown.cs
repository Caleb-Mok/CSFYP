using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public int totalTime;
    string timeDisp;
    public TextMeshPro StartCounter;
	  
	void Start()
	{
		totalTime = 3;
	}

	public IEnumerator startCountDown()
	{

        
		while (totalTime>=1)
		{
			timeDisp = totalTime.ToString();
			StartCounter.text = timeDisp;
            yield return new WaitForSeconds(1);
            totalTime-=1;
		}
        
        StartCounter.text = "";
        Destroy(StartCounter);

	}
}
