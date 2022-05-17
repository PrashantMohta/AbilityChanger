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

namespace AbilityChanger
{
    public  class Focus: AbilityManager
    {

        public static string abilityName = "Focus";
        private static new string inventoryTitleKey = $"INV_NAME_SPELL_FOCUS";
        private static new string inventoryDescKey = $"INV_DESC_SPELL_FOCUS";
        public Focus() : base(Focus.abilityName, Focus.inventoryTitleKey, Focus.inventoryDescKey, () => true) { }
        public override GameObject getIconGo() => InvGo.Find("Spell Focus");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Focus",
                    eventName = "FOCUS COMPLETED",
                    toStateDefault = "Spore Cloud",
                    toStateCustom = "Full HP?",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Focus",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentlySelected = nextAbility().name;
                        updateInventory();
                    }
                });
            }
        }






    }
}
