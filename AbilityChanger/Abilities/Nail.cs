namespace AbilityChanger
{
    public class Nail: AbilityManager, IStartable, ITriggerable,IContacting,ICompletable
    {
        public override string abilityName { get; protected set; } = Abilities.NAIL;
        public override bool hasDefaultAbility()  => true;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades)) + 1}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades)) + 1}";
            }
            protected set { }
        }

        public Nail() : base()
        {
            On.HeroController.Attack += NailTrigger;
            On.NailSlash.CancelAttack += EndSlash;
        }

        private void EndSlash(On.NailSlash.orig_CancelAttack orig, NailSlash self)
        {
            HandleComplete();
            orig(self);
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
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }

        }


        private void NailTrigger(On.HeroController.orig_Attack orig, HeroController self, GlobalEnums.AttackDirection attackDir)
        {
           
            //Trigger
            if (currentAbility.hasTrigger())
            {
                HandleTrigger();
            }
            else
            {
                orig(self,attackDir);
                HandleStart();
            }
        }
        public override GameObject getIconGo() => InvGo.Find("Nail");

        public void HandleStart()
        {
            if (currentAbility.hasStart())
            {
                currentAbility.Start(); 
            }
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleContact(GameObject go = null)
        {
            throw new NotImplementedException();
        }

        public void HandleComplete()
        {
            if (!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }
    }
}
