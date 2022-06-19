namespace AbilityChanger
{
    public class Scream : AbilityManager, IStartable, ITriggerable, ICompletable
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
                return $"INV_DESC_SPELL_SCREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
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
                #region Start
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Scream Antic1",
                    toStateDefault="Scream Burst 1",
                    toStateCustom="Scream End",
                    eventName="FINISHED",
                    shouldIntercept=()=> currentAbility.hasStart(),
                    onIntercept=(a,b)=>HandleStart()
                });

                self.Intercept(new TransitionInterceptor()
                {

                    fromState = "Scream Antic2",
                    toStateDefault = "Scream Burst 2",
                    toStateCustom = "Scream End",
                    eventName = "FINISHED",
                    shouldIntercept = () => currentAbility.hasStart(),
                    onIntercept = (a, b) => HandleStart()
                });

                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Scream?",
                    eventName = "CAST",
                    toStateDefault = "Scream Get?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => HandleTrigger()
                });
                #endregion

                #region Complete
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Scream End",
                    toStateDefault="Spell End",
                    toStateCustom="Spell End",
                    eventName="FINISHED",
                    shouldIntercept=()=>currentAbility.hasComplete(),
                    onIntercept=(a,b)=> HandleComplete()
                });

                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Scream End 2",
                    toStateDefault = "Spell End",
                    toStateCustom = "Spell End",
                    eventName = "FINISHED",
                    shouldIntercept = () => currentAbility.hasComplete(),
                    onIntercept = (a, b) => HandleComplete()
                });
                #endregion

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

        public void HandleStart()
        {
            currentAbility.Start();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleComplete()
        {
            currentAbility.Complete(false);
        }
    }
}
