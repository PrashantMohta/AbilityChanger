namespace AbilityChanger
{
    public class GreatSlash : AbilityManager {
       
        public override string abilityName { get; protected set; } = Abilities.GREATSLASH;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDashSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_DASH";
        public GreatSlash() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Art Dash");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                 self.Intercept(new TransitionInterceptor(){
                    fromState ="Flash 2",
                    eventName ="FINISHED",
                    toStateDefault="Facing?",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dash Slash",
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