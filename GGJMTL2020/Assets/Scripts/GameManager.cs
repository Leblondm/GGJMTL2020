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

	public int counterCanon = 0;

	public List<Interactible> lstBreakable;

    public Animator CameraAnimator;
    public Animator KrakenAnimator;
    public Animation CameraShake;

    public List<Phase> phases;

    public Canvas canvas;

    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlay() {
        playing = true;
        StartCoroutine("GameplayLoop");

        CameraAnimator.SetTrigger("Game");
        KrakenAnimator.SetTrigger("Game");
    }

    public void ShakeCamera() {
        CameraShake.Play();
        canvas.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Stop("MenuMusic");
        FindObjectOfType<AudioManager>().Play("AmbiantMusic");
    }

    IEnumerator GameplayLoop() {
        while(true)
        {
            // TODO break something
            ShakeCamera();

            float time = timing + Random.Range(-randomizedTiming, randomizedTiming);
            yield return new WaitForSeconds(time);
        }
    }

	public void CanonShoot()
	{
		counterCanon++;
		timing = phases[counterCanon].timing;
		randomizedTiming = phases[counterCanon].randomizedTiming;
	}
}
