namespace AbilityChanger
{
    public  class Focus: AbilityManager
    {

        public override string abilityName { get; protected set; } = Abilities.FOCUS;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => true;
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_SPELL_FOCUS";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_SPELL_FOCUS";
        public Focus() : base() { }
        public override GameObject getIconGo() => InvGo.Find("Spell Focus");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Focus",
                    eventName = "FOCUS COMPLETED",
                    toStateDefault = "Spore Cloud",
                    toStateCustom = "Full HP?",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Focus",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentlySelected = nextAbility().name;
                        updateInventory();
                    }
                });
            }
        }






    }
}
