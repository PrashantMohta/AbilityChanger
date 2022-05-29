namespace AbilityChanger
{
    public class Nail: AbilityManager
    {
        public static string abilityName = "Nail";
        private static new string inventoryTitleKey=>$"INV_NAME_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades))+1}";

        private static new string inventoryDescKey=>$"INV_DESC_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades))+1}";


        public Nail() : base(Nail.abilityName, Nail.inventoryTitleKey, Nail.inventoryDescKey, () => true)
        {
            On.HeroController.Attack += NailTrigger;
        }

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Nail",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        Modding.Logger.Log("oi");
                        currentlySelected = nextAbility().name;
                        updateInventory();
                    }
                });
            }

        }


        private void NailTrigger(On.HeroController.orig_Attack orig, HeroController self, GlobalEnums.AttackDirection attackDir)
        {
           
            if (isCustom())
            {
                this.handleAbilityUse();
            }
            else
            {
                orig(self,attackDir);
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Nail");

    }
}
