namespace AbilityChanger
{
    public class Fireball : AbilityManager
    {

       public static string abilityName = "Fireball";
        private static new string inventoryTitleKey =$"INV_NAME_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
        private static new string inventoryDescKey = $"INV_DESC_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
        public Fireball() : base(Fireball.abilityName, Fireball.inventoryTitleKey, Fireball.inventoryDescKey, () => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.fireballLevel))>0) { }
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
                    shouldIntercept = () => this.isCustom(),
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
