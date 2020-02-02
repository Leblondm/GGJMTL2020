using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileStock : Interactible
{
	public GameRessource RessourceGiven = GameRessource.None;
	public override void ExecuteInteract(PlayerController Instigator)
	{
		base.ExecuteInteract(Instigator);
		Instigator.currentStuff = RessourceGiven;
		EnableInteract();
		//	update visuel de la ressource qu'on traine
	}
}
