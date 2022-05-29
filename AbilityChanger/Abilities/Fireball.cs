namespace AbilityChanger
{
    public class Fireball : AbilityManager
    {

        public override string abilityName { get; protected set; } = Abilities.FIREBALL;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.fireballLevel)) > 0;
        public override string inventoryTitleKey { 
            get{
                return $"INV_NAME_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
            }
            protected set { }
        } 
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
            }
            protected set { }
        }

        public Fireball() : base() { }
        public override GameObject getIconGo() => InvGo.Find("Spell Fireball");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Fireball?",
                    eventName = "CAST",
                    toStateDefault = "Wallside?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => this.isCustom() && this.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Fireball",
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
