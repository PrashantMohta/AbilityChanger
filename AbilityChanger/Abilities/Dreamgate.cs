namespace AbilityChanger
{
    public class Dreamgate : AbilityManager {
        // wont actually be usable till atleast 1 dreamNail ability is also acquired
        public override string abilityName { get; protected set; } = Abilities.DREAMGATE;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamGate);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DREAMGATE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DREAMGATE";
        public Dreamgate() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Dream Gate");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FINISHED",
                    toStateDefault="Set",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => this.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FAIL",
                    toStateDefault="Set Fail",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => this.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FINISHED",
                    toStateDefault="Check Scene",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FAIL",
                    toStateDefault="Warp Fail",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="NO ESSENCE",
                    toStateDefault="Show Essence",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => this.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });
                
            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dream Gate",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }
        }

    }
}