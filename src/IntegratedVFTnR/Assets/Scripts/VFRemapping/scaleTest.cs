using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleTest : MonoBehaviour
{
    private Vector3 increment;
    private Vector3 pivot;
    private float amount;
    // Start is called before the first frame update
    void Start()
    {
        increment = new Vector3(0.01f, 0f, 0f);
        pivot = new Vector3(-0.5f, 0f, 2f);
        amount = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.RightArrow)){
            //transform.localScale += increment;
            //ScaleAround(gameObject, pivot, increment, true);
            resize(amount, false);
        }
        if (Input.GetKey (KeyCode.LeftArrow)){
            //transform.localScale -= increment;
            //ScaleAround(gameObject, pivot, increment, false);
            resize(amount, true);
        }
    }

    void resize(float amount, bool shrink) {
        if (!shrink) {
            transform.position = new Vector3 (transform.position.x + (amount/2), transform.position.y, transform.position.z);
            transform.localScale += increment;
        }
        else {
            transform.position = new Vector3 (transform.position.x - (amount/2), transform.position.y, transform.position.z);
            transform.localScale -= increment;
        }
    }


     public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale, bool increase)
 {
     Vector3 A = target.transform.localPosition;
     Vector3 B = pivot;
 
     Vector3 C = A - B; // diff from object pivot to desired pivot/origin
 
     float RS = newScale.x / target.transform.localScale.x; // relataive scale factor
 
     // calc final position post-scale
     Vector3 FP = B + C * RS;
 
     // finally, actually perform the scale/translation
     if (increase){
        target.transform.localScale += newScale;
        target.transform.localPosition = FP;
     }
     else{
        target.transform.localScale -= newScale;
        target.transform.localPosition = FP;
     }
 }
}


