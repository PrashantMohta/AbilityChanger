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
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState ="Flash 2",
                            eventName ="FINISHED",
                            toStateDefault="Facing?",
                            toStateCustom="Regain Control"
                        }));

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.EventInterceptor("Dash Slash","UI CONFIRM",() => {
                    currentlySelected= nextAbility().name;
                    updateInventory();
                });
            }
        }

    }
}