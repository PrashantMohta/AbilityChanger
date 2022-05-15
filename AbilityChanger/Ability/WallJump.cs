using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;

using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;
using Satchel.Futils;

namespace AbilityChanger {
    public class WallJump : AbilityManager {
       
        public static string abilityName = "WallJump";
        private static new string inventoryTitleKey = "INV_NAME_WALLJUMP";
        private static new string inventoryDescKey = "INV_DESC_WALLJUMP";
        public WallJump() : base (WallJump.abilityName,WallJump.inventoryTitleKey,WallJump.inventoryDescKey,() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasWalljump)){
            On.HeroController.DoWallJump += WallJumpAbilityTrigger;
        }
        public void WallJumpAbilityTrigger(On.HeroController.orig_DoWallJump orig, HeroController self){
            if(isCustom()){
                this.handleAbilityUse();
            } else {
                orig(self);
            }
        }

        public override GameObject getIconGo() => InvGo.Find("Mantis Claw");

    }
}