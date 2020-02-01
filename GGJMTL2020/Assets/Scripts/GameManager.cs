using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phase {
    public float timing = 0f;
    public float randomizedTiming = 0f;
}

public class GameManager : MonoBehaviour
{
    public float timing = 10f;
    public float randomizedTiming = 2f;

    public List<Phase> phases;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GameplayLoop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameplayLoop() {
        while(true)
        {
            // TODO break something

            float time = timing + Random.Range(-randomizedTiming, randomizedTiming);
            yield return new WaitForSeconds(time);
        }
    }
}
