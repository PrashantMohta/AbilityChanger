namespace AbilityChanger
{
    public class Dash : AbilityManager {
  
        public override string abilityName { get; protected set; } = Abilities.DASH;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDash) || PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.canDash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DASH";
        public Dash() : base (){
            On.HeroController.Dash += DashFixedUpdate;
            On.HeroController.HeroDash += DashAbilityTrigger;
        }
        public void DashAbilityTrigger(On.HeroController.orig_HeroDash orig, HeroController self){
            orig(self);
            if(isCustom()){
                this.handleAbilityUse();
            }
        }
        public void DashFixedUpdate(On.HeroController.orig_Dash orig, HeroController self){
            if(!isCustom()){
                orig(self);
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Dash Cloak");

    }
}