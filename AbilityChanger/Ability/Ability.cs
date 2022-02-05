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
    
    public class Ability{
        public string name;
        public string title;
        public string description;
        public bool isCustom = true;
        public Sprite activeSprite,inactiveSprite;
        public Ability(string name,bool isCustom = true){
            this.isCustom = isCustom;
            this.name = name;
        }
        public Ability(string name,string title, string description,Sprite activeSprite,Sprite inactiveSprite,bool isCustom = true){
            this.isCustom = isCustom;
            this.name = name;
            this.title = title;
            this.description = description;
            this.activeSprite = activeSprite;
            this.inactiveSprite = inactiveSprite;
        }
        public virtual void handleAbilityUse(string interceptedState,string interceptedEvent){}
    }
    public abstract class AbilityManager{
        protected List<Ability> options;
        protected string currentlySelected;
        protected GameObject InvGo;    
        protected string inventoryTitleKey, inventoryDescKey;
        public void addAbility(Ability ability){
            options.Add(ability);
        }
        public void removeAbility(string abilityName){
            options.Where( a => a.name == abilityName).ToList();;
        }        
        public bool isCustom(){
            return getAbility().isCustom;
        }
        public Ability getAbility(){
            return options.First(a => a.name == currentlySelected) ?? getDefaultAbility();
        }

        public Ability getDefaultAbility(){
            return options.First(a => a.isCustom == false);
        }

        public Ability nextAbility(){
            var currentIndex = options.FindIndex(a => a.name == currentlySelected);
            Ability nextAbility;
            if(options.Count() > currentIndex + 1){
               nextAbility = options[currentIndex + 1];
            } else {
               nextAbility = getDefaultAbility();
            }
            return nextAbility;
        }

        public AbilityManager(string abilityName,string inventoryTitleKey,string inventoryDescKey){
            this.inventoryTitleKey = inventoryTitleKey;
            this.inventoryDescKey = inventoryDescKey;
            options = new(){new Ability(abilityName,false)};
            currentlySelected = abilityName;
            On.PlayMakerFSM.OnEnable += CacheInventoryObject;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            ModHooks.LanguageGetHook += LanguageGet;

        }
        public abstract GameObject getIconGo();
        public abstract void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self);

        public virtual void updateIcon(GameObject icon){
            var itemdisplay = icon.GetComponent<InvItemDisplay>();
            var currentAbility = getAbility();
            var defaultAbility = getDefaultAbility();
            if(defaultAbility.activeSprite == null){
                defaultAbility.activeSprite = itemdisplay.activeSprite;
            }
            if(defaultAbility.inactiveSprite == null){
                defaultAbility.inactiveSprite = itemdisplay.inactiveSprite;
            }
            if(currentAbility.activeSprite != null){
                
                itemdisplay.activeSprite = currentAbility.activeSprite;
            }
            if(currentAbility.inactiveSprite != null){
                itemdisplay.inactiveSprite = currentAbility.inactiveSprite;
            }
            itemdisplay.SendMessage("OnEnable");
        }

        private void updateText(){
            if(InvGo != null){
                InvGo.LocateMyFSM("Update Text").Fsm.Event("UPDATE TEXT");
            }
        }
        private void CacheInventoryObject(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "Update Text")
            {
                InvGo = self.gameObject;
            }
        }
        protected void updateInventory(){
            updateText();
            var icon = getIconGo();
            if(icon != null){
                updateIcon(icon);
            }
        }
        private string LanguageGet(string title, string sheet, string orig){
            if(sheet == "UI" && title == inventoryTitleKey && isCustom()){
                   return getAbility().title;
            }
            
            if(sheet == "UI" && title == inventoryDescKey && options.Count() > 1){
                var final = orig;
                if(isCustom()){
                    final = getAbility().description;
                }
                return $"Press the confirm button to cycle abilities. <br><br> {final}";
            }
            return orig;
        }
        public void handleAbilityUse(string interceptedState,string interceptedEvent){
            getAbility().handleAbilityUse(interceptedState,interceptedEvent);
        }

    }
}