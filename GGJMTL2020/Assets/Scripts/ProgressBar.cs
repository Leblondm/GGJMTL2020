using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float amount = 100;
    public float progress = 100;

    public Transform progressObject;

    // Start is called before the first frame update
    void Start()
    {
        SetProgress(progress);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetPercentage() {
        return progress / amount;
    }

    public void IncrementProgress(float value) {
        SetProgress(progress + value);
    }

    public void ReduceProgress(float value) {
        SetProgress(progress - value);
    }

    public void SetProgress(float value) {
        progress = value;
        progressObject.localScale = new Vector2(progress / amount, progressObject.localScale.y);
    }
}
