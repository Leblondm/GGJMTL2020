using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : Interactible
{
	public override void ExecuteInteract(PlayerController Instigator)
	{
		base.ExecuteInteract(Instigator);
		Instigator.currentStuff = GameRessource.None;
		FindObjectOfType<GameManager>().StuffRepair(this);
		//	update game manager
		//	update visuel de la ressource qu'on traine
	}
}
