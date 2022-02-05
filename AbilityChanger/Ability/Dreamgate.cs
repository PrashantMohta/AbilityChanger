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
    public class Dreamgate : AbilityManager {
       
        public static string abilityName = "Dream Gate";
        private static string inventoryTitleKey = "INV_NAME_DREAMGATE";
        private static string inventoryDescKey = "INV_DESC_DREAMGATE";
        public Dreamgate() : base (Dreamgate.abilityName,Dreamgate.inventoryTitleKey,Dreamgate.inventoryDescKey){}
        public override GameObject getIconGo() => InvGo.Find("Dream Gate");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail")
            {
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState ="Can Set?",
                            eventName ="FINISHED",
                            toStateDefault="Set",
                            toStateCustom="Set Recover"
                        }));
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState = "Can Set?",
                            eventName = "FAIL",
                            toStateDefault = "Set Fail",
                            toStateCustom = "Set Recover"
                        }));
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState = "Can Warp?",
                            eventName = "FINISHED",
                            toStateDefault = "Check Scene",
                            toStateCustom = "Warp Cancel"
                        }));
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState = "Can Warp?",
                            eventName = "FAIL",
                            toStateDefault = "Warp Fail",
                            toStateCustom = "Warp Cancel"
                        }));
                self.InterceptTransition(
                    new Interceptor((AbilityManager)this,
                        new InterceptorParams{
                            fromState = "Can Warp?",
                            eventName = "NO ESSENCE",
                            toStateDefault = "Show Essence",
                            toStateCustom = "Warp Cancel"
                        }));

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.EventInterceptor("Dream Gate","UI CONFIRM",() => {
                    currentlySelected= nextAbility().name;
                    updateInventory();
                });
            }
        }

    }
}