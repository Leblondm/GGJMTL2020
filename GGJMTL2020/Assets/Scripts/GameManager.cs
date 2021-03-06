﻿using System.Collections;
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
	public List<Interactible> lstBreak;

	public Animator CameraAnimator;
    public Animator KrakenAnimator;
    public Animation CameraShake;

	public ProgressBar KillBar;
	public ProgressBar krakenBar;

	public List<Phase> phases;
    public Phase currentPhase;

    public RectTransform gameUI;
    public RectTransform menuUI;
	public SpriteRenderer EndScreen;
	public Sprite WinScreen;
	public Sprite LooseScreen;

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
        FindObjectOfType<AudioManager>().Play("UIClick");
        CameraAnimator.SetTrigger("Game");
        KrakenAnimator.SetTrigger("Game");
        StartCoroutine("WaitStart");
        menuUI.gameObject.SetActive(false);
    }

    public void ShakeCamera() {
        CameraShake.Play();
    }

    IEnumerator WaitStart() {
        yield return new WaitForSeconds(15f);
        gameUI.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("MenuMusic");
        FindObjectOfType<AudioManager>().Play("AmbiantMusic");
        StartCoroutine("GameplayLoop");
    }

    IEnumerator GameplayLoop() {
        while(true)
        {
            // TODO break something
			if(lstBreakable.Count > 0)
			{
				int BreakStuffIndex = Random.Range(0, (lstBreakable.Count - 1));
				if (lstBreakable[BreakStuffIndex] != null)
				{
					lstBreakable[BreakStuffIndex].EnableInteract();
					lstBreak.Add(lstBreakable[BreakStuffIndex]);
					lstBreakable.RemoveAt(BreakStuffIndex);
					ShakeCamera();
				}
			}

			krakenBar.ReduceProgress(lstBreak.Count);
			if(krakenBar.progress <= 0.0f)
			{
				Loose();
			}

			float time = timing + Random.Range(-randomizedTiming, randomizedTiming);
            yield return new WaitForSeconds(time);
        }
    }

    public void Win()
	{
		gameUI.gameObject.SetActive(false);
        EndScreen.sprite = WinScreen;
		EndScreen.enabled = true;
		PlayerController[] myPlayer = FindObjectsOfType<PlayerController>();
		for (int i = 0; i < myPlayer.Length; i++)
		{
			myPlayer[i].blockInputEndGame = true;
		}
    }

    public void Loose()
	{
		gameUI.gameObject.SetActive(false);
        EndScreen.sprite = LooseScreen;
		EndScreen.enabled = true;
		PlayerController[] myPlayer = FindObjectsOfType<PlayerController>();
		for (int i = 0; i < myPlayer.Length; i++)
		{
			myPlayer[i].blockInputEndGame = true;
		}
	}

	public void CanonShoot()
	{
		counterCanon++;
		KillBar.SetProgress(counterCanon);
		if(counterCanon > 3)
		{
			//	fin win
			Win();
		}
		timing = phases[counterCanon].timing;
		randomizedTiming = phases[counterCanon].randomizedTiming;

	}


	public void StuffRepair(Interactible myCoolStuff)
	{
		for (int i = 0; i < lstBreak.Count; i++)
		{
			if(lstBreak[i] == myCoolStuff)
			{
				lstBreakable.Add(lstBreak[i]);
				lstBreak.RemoveAt(i);
				return;
			}
		}
	}
}
