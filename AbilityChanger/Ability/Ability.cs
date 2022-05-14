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
        public Func<bool> hasAbility = () => true;
        public Sprite activeSprite,inactiveSprite;
        public Ability(string name,Func<bool> hasAbility,bool isCustom = true){
            this.isCustom = isCustom;
            this.name = name;
            this.hasAbility = hasAbility;
        }
        public Ability(string name,string title, string description,Sprite activeSprite,Sprite inactiveSprite,Func<bool> hasAbility,bool isCustom = true){
            this.isCustom = isCustom;
            this.name = name;
            this.title = title;
            this.description = description;
            this.activeSprite = activeSprite;
            this.inactiveSprite = inactiveSprite;
            this.hasAbility = hasAbility;
        }
        public virtual void handleAbilityUse(string interceptedState,string interceptedEvent){}
    }
}