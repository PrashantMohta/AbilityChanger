namespace AbilityChanger
{
    public class Scream : AbilityManager
    {
        public override string abilityName { get; protected set; } = Abilities.SCREAM;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.screamLevel)) > 0;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_SPELL_SCREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_SPELL_CREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
            }
            protected set { }
        }
        public Scream() : base() { }
        public override GameObject getIconGo() => InvGo.Find("Spell Scream");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Scream?",
                    eventName = "CAST",
                    toStateDefault = "Scream Get?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => this.isCustom() && this.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Scream",
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
