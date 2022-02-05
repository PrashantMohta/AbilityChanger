using System;
using System.Collections.Generic;
using System.Linq;

using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using Satchel;
namespace AbilityChanger {
    public class AbilityChanger : Mod
    {
        public override string GetVersion() => "0.1";

        public static Dictionary<string,AbilityManager> AbilityMap = new(){
            {Dreamgate.abilityName,new Dreamgate()},
            {CycloneSlash.abilityName,new CycloneSlash()},
        };
        public override void Initialize()
        {
        }

    }
}
