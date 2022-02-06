using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;

using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;

namespace AbilityChanger {
    public class DashSlash : AbilityManager {
       
        public static string abilityName = "Dash Slash";
        private static string inventoryTitleKey = "INV_NAME_ART_UPPER";
        private static string inventoryDescKey = "INV_DESC_ART_UPPER";
        public DashSlash() : base (DashSlash.abilityName,DashSlash.inventoryTitleKey,DashSlash.inventoryDescKey){}
        public override GameObject getIconGo() => InvGo.Find("Art Uppercut");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState ="Dash Slash Ready",
                            eventName ="DASH END",
                            toStateDefault="DSlash Start",
                            toStateCustom="Regain Control"
                        }));

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.EventInterceptor("Uppercut","UI CONFIRM",() => {
                    currentlySelected= nextAbility().name;
                    updateInventory();
                });
            }
        }

    }
}