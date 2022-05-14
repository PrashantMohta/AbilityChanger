using Modding;

namespace AbilityChanger {
    internal static class PlayerDataPatcher{
        internal static string hasNailArt = nameof(PlayerData.hasNailArt);
        internal static string hasAllNailArts = nameof(PlayerData.hasAllNailArts);
        internal static string hasCyclone = nameof(PlayerData.hasCyclone);
        internal static string hasUpwardSlash = nameof(PlayerData.hasUpwardSlash); //tc weirdness
        internal static string hasDashSlash = nameof(PlayerData.hasDashSlash); //tc weirdness
        internal static string hasDreamNail = nameof(PlayerData.hasDreamNail);
        internal static string hasDreamGate = nameof(PlayerData.hasDreamGate);
        internal static string canDash = nameof(PlayerData.canDash);
        internal static string hasDash = nameof(PlayerData.hasDash);
        internal static string hasDoubleJump = nameof(PlayerData.hasDoubleJump);
        static PlayerDataPatcher(){
        }
        public static bool GetBoolInternal(string name){
            return PlayerData.instance.GetBoolInternal(name);
        }
        public static bool OnGetPlayerBoolHook(string name,bool orig){
            if(name == hasNailArt){
                return (AbilityChanger.AbilityMap[CycloneSlash.abilityName].hasAcquiredAbility() ||
                 AbilityChanger.AbilityMap[DashSlash.abilityName].hasAcquiredAbility() || 
                 AbilityChanger.AbilityMap[GreatSlash.abilityName].hasAcquiredAbility());
            }
            if(name == hasAllNailArts){
                return (AbilityChanger.AbilityMap[CycloneSlash.abilityName].hasAcquiredAbility() &&
                 AbilityChanger.AbilityMap[DashSlash.abilityName].hasAcquiredAbility() && 
                 AbilityChanger.AbilityMap[GreatSlash.abilityName].hasAcquiredAbility());
            }
            if(name == hasCyclone){
                return AbilityChanger.AbilityMap[CycloneSlash.abilityName].hasAcquiredAbility(); 
            }
            if(name == hasUpwardSlash){
                return AbilityChanger.AbilityMap[DashSlash.abilityName].hasAcquiredAbility(); 
            }
            if(name == hasDashSlash){
                return AbilityChanger.AbilityMap[GreatSlash.abilityName].hasAcquiredAbility();
            }
            if(name == hasDreamGate){
                return AbilityChanger.AbilityMap[Dreamgate.abilityName].hasAcquiredAbility(); 
            }
            if(name == hasDash || name == canDash){
                return AbilityChanger.AbilityMap[Dash.abilityName].hasAcquiredAbility(); 
            }
            if(name == hasDoubleJump){
                return AbilityChanger.AbilityMap[DoubleJump.abilityName].hasAcquiredAbility();
            }
            return orig;
        }
    }
}