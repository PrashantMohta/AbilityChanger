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
    public class Quake : AbilityManager
    {

        public static string abilityName = "Quake";
        private static new string inventoryTitleKey = $"INV_NAME_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
        private static new string inventoryDescKey = $"INV_DESC_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
        public Quake() : base(Quake.abilityName, Quake.inventoryTitleKey, Quake.inventoryDescKey, () => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.quakeLevel)) > 0) { }
        public override GameObject getIconGo() => InvGo.Find("Spell Quake");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Quake?",
                    eventName = "CAST",
                    toStateDefault = "On Gound?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Quake",
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
