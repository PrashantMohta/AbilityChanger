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
    public class DoubleJump : AbilityManager {
       
        public static string abilityName = "DoubleJump";
        private static new string inventoryTitleKey = "INV_NAME_DOUBLEJUMP";
        private static new string inventoryDescKey = "INV_DESC_DOUBLEJUMP";
        public DoubleJump() : base (DoubleJump.abilityName,DoubleJump.inventoryTitleKey,DoubleJump.inventoryDescKey,() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDoubleJump)){
            On.HeroController.DoubleJump += DoubleJumpFixedUpdate;
            On.HeroController.DoDoubleJump += DoubleJumpAbilityTrigger;
        }
        public void DoubleJumpAbilityTrigger(On.HeroController.orig_DoDoubleJump orig, HeroController self){
            orig(self);
            if(isCustom()){
                this.handleAbilityUse();
            }
        }
        public void DoubleJumpFixedUpdate(On.HeroController.orig_DoubleJump orig, HeroController self){
            if(!isCustom()){
                orig(self);
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Double Jump");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Equip Item 4",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentlySelected= nextAbility().name;
                        updateInventory();
                    }
                });
            }
        }

    }
}