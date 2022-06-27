namespace AbilityChanger
{
    public class DoubleJump : AbilityManager, IStartable, ICancellable,ITriggerable, IOngoing,ICompletable {
       
        public override string abilityName { get; protected set; } = Abilities.DOUBLEJUMP;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDoubleJump);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DOUBLEJUMP";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DOUBLEJUMP";
        public DoubleJump() : base (){
            On.HeroController.DoubleJump += DoubleJumpFixedUpdate;
            On.HeroController.DoDoubleJump += DoubleJumpAbilityTrigger;
            On.HeroController.CancelDoubleJump += CancelComplete;
        }

        private void CancelComplete(On.HeroController.orig_CancelDoubleJump orig, HeroController self)
        {
            if (HeroControllerR.doubleJump_steps > 3) HandleComplete();
            else if(HeroControllerR.doubleJump_steps!=0) HandleCancel();
            orig(self);
        }

        public void DoubleJumpAbilityTrigger(On.HeroController.orig_DoDoubleJump orig, HeroController self){
            #region Trigger
            if (currentAbility.hasTrigger()) { HandleTrigger(); return; }
            #endregion

            #region Start
            HandleStart();
            orig(self);
            #endregion 
        }
        public void DoubleJumpFixedUpdate(On.HeroController.orig_DoubleJump orig, HeroController self){
            #region OnGoing
            if (currentAbility.hasOngoing()) {  HandleOngoing();  }
            #endregion 
            orig(self);
        }
        public override GameObject getIconGo() => InvGo.Find("Double Jump");

        public void HandleStart()
        {
            if(!currentAbility.hasStart()) return;
            currentAbility.Start();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleOngoing()
        {
            currentAbility.Ongoing();
        }

        public void HandleComplete()
        {
            if (currentAbility.hasComplete()) currentAbility.Complete(false);
        }

        public void HandleCancel()
        {
            if(currentAbility.hasCancel()) currentAbility.Cancel();
        }
    }
}