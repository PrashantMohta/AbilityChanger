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
    internal static class Equipment{
        public static AbilityManager getEquipmentForIndex(int index){
           int count = 1;
           Dictionary<int,AbilityManager> IndexToAbility = new();
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasDash)){
               IndexToAbility[count] = AbilityChanger.AbilityMap[Dash.abilityName];
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasWalljump)){
               //IndexToAbility[count] = AbilityChanger.AbilityMap[Walljump.abilityName];
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasSuperDash)){
               //IndexToAbility[count] = AbilityChanger.AbilityMap[SuperDash.abilityName];
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasDoubleJump)){
               IndexToAbility[count] = AbilityChanger.AbilityMap[DoubleJump.abilityName];
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasAcidArmour)){
               //IndexToAbility[count] = AbilityChanger.AbilityMap[AcidArmour.abilityName];
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasLantern)){
               //IndexToAbility[count] = AbilityChanger.AbilityMap[Lantern.abilityName];
               count++;
           } 
           /*
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasMap) || PlayerDataPatcher.GetBool(PlayerDataPatcher.hasQuill)){
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasKingsBrand)){
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasTramPass)){
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasCityKey)){
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasSlykey)){
               count++;
           } 
           if(!PlayerDataPatcher.GetBool(PlayerDataPatcher.hasSlykey) && PlayerDataPatcher.GetBool(PlayerDataPatcher.hasWhiteKey) && !PlayerDataPatcher.GetBool(PlayerDataPatcher.usedWhiteKey)){
               count++;
           } 
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasLoveKey)){
               count++;
           }
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.hasXunFlower)){
               count++;
           }
           if(PlayerDataPatcher.GetBool(PlayerDataPatcher.simpleKeys)){
               count++;
           }*/
           if(IndexToAbility.ContainsKey(index)){
                return IndexToAbility[index];
           }
           return null;
        }
        public static void changeOption(int index){
            AbilityManager ability = getEquipmentForIndex(index);
            if(ability != null){
                ability.currentlySelected = ability.nextAbility().name;
                ability.updateInventory();
            }
        }
        public static Action getChangeAction(int index){
            return () => { changeOption(index); };
        }
        public static void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {   
                for(var i = 1; i <= 16 ; i++){
                    self.Intercept(new EventInterceptor(){
                        fromState = $"Equip Item {i}",
                        eventName = "UI CONFIRM",
                        onIntercept = getChangeAction(i)
                    });
                }
                
            }
        }

    }
}