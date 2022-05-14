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
    public class DashSlash : AbilityManager {
       
        public static string abilityName = "Dash Slash";
        private static new string inventoryTitleKey = "INV_NAME_ART_UPPER";
        private static new string inventoryDescKey = "INV_DESC_ART_UPPER";
        public DashSlash() : base (DashSlash.abilityName,DashSlash.inventoryTitleKey,DashSlash.inventoryDescKey,() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasUpwardSlash)){}        
        public override GameObject getIconGo() => InvGo.Find("Art Uppercut");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Dash Slash Ready",
                    eventName ="DASH END",
                    toStateDefault="DSlash Start",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Uppercut",
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