namespace AbilityChanger
{
    public class Dash : AbilityManager, IStartable, ITriggerable, IOngoing, ICompletable
    {
  
        public override string abilityName { get; protected set; } = Abilities.DASH;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDash) || PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.canDash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DASH";
        public Dash() : base (){
            On.HeroController.Dash += DashFixedUpdate;
            On.HeroController.HeroDash += DashAbilityTrigger;
            On.HeroController.FinishedDashing += HeroController_FinishedDashing;
        }

        private void HeroController_FinishedDashing(On.HeroController.orig_FinishedDashing orig, HeroController self)
        {
            orig(self);
            HandleComplete();
        }

        public void DashAbilityTrigger(On.HeroController.orig_HeroDash orig, HeroController self){
            HandleStart();
            if (currentAbility.hasTrigger()) {
                HandleTrigger();
            } else
            {
                orig(self);
            } 
        }
        public void DashFixedUpdate(On.HeroController.orig_Dash orig, HeroController self){
            if(!currentAbility.hasOngoing()){
                orig(self);
            } else {
                if (HeroControllerR.dash_timer > self.DASH_TIME)
                {
                    HandleComplete();
                    return;
                }
                HandleOngoing();
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Dash Cloak");

        public void HandleStart()
        {
            if (currentAbility.hasStart())
            {
                currentAbility.Start();
            }
        }

        public void HandleTrigger()
        {
            this.handleAbilityUse();
        }

        public void HandleOngoing()
        {
            currentAbility.Ongoing();
        }

        public void HandleComplete()
        {

            if (currentAbility.hasComplete())
            {
                currentAbility.Complete(false);
            }
        }
    }
}