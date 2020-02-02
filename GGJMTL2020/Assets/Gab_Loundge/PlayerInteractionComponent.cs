using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
	public GameRessource currentStuff = GameRessource.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetButton("Fire3")/*GetKeyDown(KeyCode.Space)*/ == true)
		{
			Collider2D[] interactInRange = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0, LayerMask.GetMask("Interact"));
			if(interactInRange.Length > 0)
			{
				for (int i = 0; i < interactInRange.Length; i++)
				{
					Interactible myInteract = interactInRange[i].GetComponent<Interactible>();
					if (myInteract != null)
					{
						//myInteract.StartExecute(this);
						Debug.Log("Get GameObject = " + interactInRange[i].gameObject.name);
					}
				}
				Debug.Log("Fin");
			}
		}
		else if(Input.GetButtonUp("Fire3"))
		{
			Collider2D[] interactInRange = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0, LayerMask.GetMask("Interact"));
			if (interactInRange.Length > 0)
			{
				for (int i = 0; i < interactInRange.Length; i++)
				{
					Interactible myInteract = interactInRange[i].GetComponent<Interactible>();
					if (myInteract != null)
					{
						myInteract.EndExecute();
					}
				}
			}
		}
    }

	
}
