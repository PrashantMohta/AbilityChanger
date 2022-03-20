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
    public class GreatSlash : AbilityManager {
       
        public static string abilityName = "Great Slash";
        private static string inventoryTitleKey = "INV_NAME_ART_DASH";
        private static string inventoryDescKey = "INV_DESC_ART_DASH";
        public GreatSlash() : base (GreatSlash.abilityName,GreatSlash.inventoryTitleKey,GreatSlash.inventoryDescKey){}
        public override GameObject getIconGo() => InvGo.Find("Art Dash");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                 self.Intercept(new TransitionInterceptor(){
                    fromState ="Flash 2",
                    eventName ="FINISHED",
                    toStateDefault="Facing?",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dash Slash",
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