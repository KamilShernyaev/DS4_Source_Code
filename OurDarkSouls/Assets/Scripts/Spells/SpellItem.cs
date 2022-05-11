using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
	public class SpellItem: Item
	{
		public GameObject spellWarmUpFX;
		public GameObject spellCastFX;
		public string spellAnimation;

		[Header("Spell Cost")]
		public int focusPointCost;

		[Header("Spell Type")]
		public bool isFaithSpell;
		public bool isMagicSpell;
		public bool isPyroSpell;

		[Header("Spell Description")]
		[TextArea]
		public string spellDescription;

		public virtual void AttemptToCastSpell(
			PlayerAnimatorManager playerAnimatorManager, 
			PlayerStatsManager playerStatsManager, 
			PlayerWeaponSlotManager playerWeaponSlotManager)
		{
			Debug.Log("You attempt to cast a spell!");
		}

		public virtual void SuccessfullyCastSpell(
			PlayerAnimatorManager playerAnimatorManager, 
			PlayerStatsManager playerStatsManager, 
			CameraHandler cameraHandler, 
			PlayerWeaponSlotManager playerWeaponSlotManager)
		{
			Debug.Log("You successfully cast a spell");
			playerStatsManager.DeductFocusPoints(focusPointCost);
		}
	}
}