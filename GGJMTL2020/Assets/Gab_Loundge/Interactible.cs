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

	public AudioClip repairClip;
	public AudioClip breakClip;
	public AudioSource source;
	public float timeToExecute = 1.0f;

	public bool bInteractible = false;
	public GameRessource needRessource = GameRessource.None;

	private Tween currentTween;

	// Start is called before the first frame update
	void Start()
    {
		
    }

	//	broken or make the interactable usable
	public void EnableInteract()
	{
		bInteractible = true;
		gameObject.GetComponent<SpriteRenderer>().sprite = breakVisual;
		(needResourceFeedback.GetComponent<SpriteRenderer>()).sprite = neededVisual;
		currentTween = needResourceFeedback.transform.DOLocalMoveY(2.0f, 2.0f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		Debug.Log("Is it working ???");
	}

	public void ExecuteInteract(PlayerInteractionComponent Instigator)
	{
		if(Instigator.currentStuff == needRessource && bInteractible == true)
		{
			Debug.Log("Faire un truc");
			gameObject.GetComponent<SpriteRenderer>().sprite = reparingVisual;
		}
		else
		{
			Debug.Log("Fuck off");
		}

	}
}
