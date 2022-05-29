namespace AbilityChanger
{
    public class DashSlash : AbilityManager {
       
        public override string abilityName { get; protected set; } = Abilities.DASHSLASH;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasUpwardSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_UPPER";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_UPPER";
        public DashSlash() : base (){}        
        public override GameObject getIconGo() => InvGo.Find("Art Uppercut");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Dash Slash Ready",
                    eventName ="DASH END",
                    toStateDefault="DSlash Start",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Uppercut",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentlySelected= nextAbility().name;
                        updateInventory();
                    }
                });
            }
        }

    }
}