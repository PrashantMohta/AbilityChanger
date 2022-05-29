namespace AbilityChanger
{
    public class Quake : AbilityManager
    {
       
        public override string abilityName { get; protected set; } = Abilities.QUAKE;
        public override Func<bool> hasDefaultAbility { get; protected set; } = () => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.quakeLevel)) > 0;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
            }
            protected set { }
        }
        public Quake() : base() {}
        public override GameObject getIconGo() => InvGo.Find("Spell Quake");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Quake?",
                    eventName = "CAST",
                    toStateDefault = "On Gound?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Quake",
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
