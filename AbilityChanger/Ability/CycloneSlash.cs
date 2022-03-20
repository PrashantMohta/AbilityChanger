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
    public class CycloneSlash : AbilityManager {
       
        public static string abilityName = "Cyclone Slash";
        private static string inventoryTitleKey = "INV_NAME_ART_CYCLONE";
        private static string inventoryDescKey = "INV_DESC_ART_CYCLONE";
        public CycloneSlash() : base (CycloneSlash.abilityName,CycloneSlash.inventoryTitleKey,CycloneSlash.inventoryDescKey){}
        public override GameObject getIconGo() => InvGo.Find("Art Cyclone");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Flash",
                    eventName ="FINISHED",
                    toStateDefault="Cyclone Start",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Cyclone",
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