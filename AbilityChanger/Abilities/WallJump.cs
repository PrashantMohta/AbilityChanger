namespace AbilityChanger
{
    public class WallJump : AbilityManager, IStartable, ITriggerable {
        public override string abilityName { get; protected set; } = Abilities.WALLJUMP;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasWalljump);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_WALLJUMP";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_WALLJUMP";
        public WallJump() : base (){
            On.HeroController.DoWallJump += WallJumpAbilityTrigger;
        }
        public void WallJumpAbilityTrigger(On.HeroController.orig_DoWallJump orig, HeroController self){
            
            //Trigger
            if(currentAbility.hasTrigger()){
                HandleTrigger();
            } else {
                //Start
                HandleStart();
                orig(self);
            }
        }

        public override GameObject getIconGo() => InvGo.Find("Mantis Claw");

        public void HandleStart()
        {
            if (!currentAbility.hasStart()) return;
            currentAbility.Start();

        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");

        }
    }
}