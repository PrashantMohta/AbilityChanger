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
    public class Dreamgate : AbilityManager {
        // wont actually be usable till atleast 1 dreamNail ability is also acquired
       
        public static string abilityName = "Dream Gate";
        private static new string inventoryTitleKey = "INV_NAME_DREAMGATE";
        private static new string inventoryDescKey = "INV_DESC_DREAMGATE";
        public Dreamgate() : base (Dreamgate.abilityName,Dreamgate.inventoryTitleKey,Dreamgate.inventoryDescKey,() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamGate)){}
        public override GameObject getIconGo() => InvGo.Find("Dream Gate");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FINISHED",
                    toStateDefault="Set",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FAIL",
                    toStateDefault="Set Fail",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FINISHED",
                    toStateDefault="Check Scene",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FAIL",
                    toStateDefault="Warp Fail",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="NO ESSENCE",
                    toStateDefault="Show Essence",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dream Gate",
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