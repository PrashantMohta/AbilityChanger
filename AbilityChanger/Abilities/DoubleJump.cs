namespace AbilityChanger
{
    public class DoubleJump : AbilityManager {
       
        public override string abilityName { get; protected set; } = Abilities.DOUBLEJUMP;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDoubleJump);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DOUBLEJUMP";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DOUBLEJUMP";
        public DoubleJump() : base (){
            On.HeroController.DoubleJump += DoubleJumpFixedUpdate;
            On.HeroController.DoDoubleJump += DoubleJumpAbilityTrigger;
        }
        public void DoubleJumpAbilityTrigger(On.HeroController.orig_DoDoubleJump orig, HeroController self){
            orig(self);
            if(isCustom()){
                this.handleAbilityUse();
            }
        }
        public void DoubleJumpFixedUpdate(On.HeroController.orig_DoubleJump orig, HeroController self){
            if(!isCustom()){
                orig(self);
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Double Jump");

    }
}