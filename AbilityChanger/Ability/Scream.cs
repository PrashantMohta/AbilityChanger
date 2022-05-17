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
    public class Scream : AbilityManager
    {

        public static string abilityName = "Scream";
        private static new string inventoryTitleKey = $"INV_NAME_SPELL_SCREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
        private static new string inventoryDescKey = $"INV_DESC_SPELL_CREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
        public Scream() : base(Scream.abilityName, Scream.inventoryTitleKey, Scream.inventoryDescKey, () => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.screamLevel)) > 0) { }
        public override GameObject getIconGo() => InvGo.Find("Spell Scream");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Scream?",
                    eventName = "CAST",
                    toStateDefault = "Scream Get?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Scream",
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
