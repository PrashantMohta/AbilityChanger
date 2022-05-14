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
    public class Dash : AbilityManager {
       
        public static string abilityName = "Dash";
        private static new string inventoryTitleKey = "INV_NAME_DASH";
        private static new string inventoryDescKey = "INV_DESC_DASH";
        public Dash() : base (Dash.abilityName,Dash.inventoryTitleKey,Dash.inventoryDescKey,() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDash) || PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.canDash)){
            On.HeroController.Dash += DashFixedUpdate;
            On.HeroController.HeroDash += DashAbilityTrigger;
        }
        public void DashAbilityTrigger(On.HeroController.orig_HeroDash orig, HeroController self){
            orig(self);
            if(isCustom()){
                this.handleAbilityUse();
            }
        }
        public void DashFixedUpdate(On.HeroController.orig_Dash orig, HeroController self){
            if(!isCustom()){
                orig(self);
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Dash Cloak");

    }
}