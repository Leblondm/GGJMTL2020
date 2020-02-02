using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameRessource
{
	None,
	Bois,
	Toile,
	Boullet
}

public class Interactible : MonoBehaviour
{
    public SpriteRenderer interactibleRenderer;
	public GameObject needResourceFeedback;
	public Sprite normalVisual;
	public Sprite reparingVisual;
	public Sprite breakVisual;
	public Sprite neededVisual;

	public string repairClipName;
	public string breakClipName;
	protected AudioManager masterAudioManager;

	public float timeToExecute = 1.0f;

	public bool bInteractible = false;
	public bool bDuringInteract = false;
	public GameRessource needRessource = GameRessource.None;
	public float progressExecution = 0.0f;
	public PlayerController currentInteractor;

	private Tween currentTween;

	private void Start()
	{
		masterAudioManager = FindObjectOfType<AudioManager>();

		if (bInteractible == true)
		{
			EnableInteract();
		}
	}

	//	broken or make the interactable usable
	public void EnableInteract()
	{
		bInteractible = true;
		gameObject.GetComponent<SpriteRenderer>().sprite = breakVisual;
		(needResourceFeedback.GetComponent<SpriteRenderer>()).sprite = neededVisual;
		currentTween = needResourceFeedback.transform.DOLocalMoveY(1.5f, 2.0f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		Debug.Log("Is it working ???");
	}

	public void EndExecute()
	{
		currentInteractor = null;
		progressExecution = 0.0f;
		bDuringInteract = false;
	}

	public void StartExecute(PlayerController Instigator)
	{
		if(Instigator == null || bInteractible == false)
		{
			return;
		}

		if(currentInteractor == null)
		{
			if ((needRessource == GameRessource.None || Instigator.currentStuff == needRessource) && bInteractible == true)
			{
				//	the first to interact with this object
				Debug.Log("Commence a Faire un truc");

				masterAudioManager.Play(repairClipName);

				if(reparingVisual != null)
				{
					gameObject.GetComponent<SpriteRenderer>().sprite = reparingVisual;
				}
				bDuringInteract = true;
				currentInteractor = Instigator;
			}
			else
			{
				Debug.Log("Fuck off");
				return;
			}
		}
		else if(currentInteractor != Instigator)
		{
			return;
		}

		//	start the execution, one complete, do the thing
		if (bDuringInteract == true)
		{
			//	someone is already interacting with this object
			progressExecution += Time.deltaTime;
			if (progressExecution >= timeToExecute)
			{
				//	the execution is finish
				Debug.Log("Yeeeeeet!!!!");
				bInteractible = false;
				gameObject.GetComponent<SpriteRenderer>().sprite = normalVisual;
				(needResourceFeedback.GetComponent<SpriteRenderer>()).sprite = null;
				currentTween.Kill();
				ExecuteInteract(Instigator);
				//(needResourceFeedback.GetComponent<SpriteRenderer>()).sprite = neededVisual;
			}
		}
	}

	public virtual void ExecuteInteract(PlayerController Instigator)
	{
		//	do the changement
		
	}
}
