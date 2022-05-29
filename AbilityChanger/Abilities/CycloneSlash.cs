namespace AbilityChanger
{
    public class CycloneSlash : AbilityManager {
       
        public override string abilityName { get; protected set; } = Abilities.CYCLONESLASH;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasCyclone);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_CYCLONE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_CYCLONE";

        public CycloneSlash() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Art Cyclone");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Flash",
                    eventName ="FINISHED",
                    toStateDefault="Cyclone Start",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => this.isCustom(),
                    onIntercept = (fsmstate,fsmevent) => this.handleAbilityUse(fsmstate,fsmevent)
                });

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Cyclone",
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