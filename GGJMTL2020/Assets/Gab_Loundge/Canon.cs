using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Interactible
{
	public int currentStep = 0;
	public Sprite ReloadVisual;
	public string ReloadClipName;
	public string ShootClipName;

	//	0 = Le canon est vide et on doit le charger
	//	1 = Le canon est charger mais on doit attendre de pouvoir viser le kraken
	//	2 = Le canon tire et on reviens au step 0
	public override void ExecuteInteract(PlayerController Instigator)
	{
		base.ExecuteInteract(Instigator);
		Instigator.currentStuff = GameRessource.None;

		if(currentStep == 0)
		{
			//	Load the canon
			currentStep = 1;
			needRessource = GameRessource.None;
			timeToExecute = 0;
			repairClipName = ShootClipName;
			reparingVisual = null;
		}
		else if(currentStep == 1)
		{
			//	Shoot the canon
			currentStep = 0;
			//	call the shoot animation
			needRessource = GameRessource.Boullet;
			reparingVisual = ReloadVisual;
			timeToExecute = 10.0f;
			repairClipName = ReloadClipName;
			EnableInteract();
			//	update game manager
		}
		//	update visuel de la ressource qu'on traine
	}
}
