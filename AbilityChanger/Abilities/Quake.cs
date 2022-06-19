namespace AbilityChanger
{
    public class Quake : AbilityManager, IStartable, IChargable, ICancellable, ITriggerable, IContacting, ICompletable,IOngoing
    {
       
        public override string abilityName { get; protected set; } = Abilities.QUAKE;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.quakeLevel)) > 0;
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

                #region Start
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Has Quake?",
                    toStateDefault="On Ground?",
                    eventName="CAST",
                    toStateCustom= "On Ground?",
                    shouldIntercept =()=>currentAbility.hasStart(),
                    onIntercept=(a,b)=>HandleStart()

                });

                #endregion

                #region Charging
                self.AddAction("Quake Antic", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));

                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Quake?",
                    eventName = "CAST",
                    toStateDefault = "On Gound?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => HandleTrigger()
                });
                #endregion

                #region OnGoing
                self.AddAction("Quake1 Down", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));

                self.AddAction("Quake2 Down", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));

                #endregion

                #region Complete
                self.AddAction("Quake Finish", new CustomFsmAction(() =>
                {
                    HandleComplete();
                }));


                #endregion

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Quake",
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

        public void HandleComplete()
        {
            if (!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }

        public void HandleContact(GameObject go = null)
        {
            throw new NotImplementedException();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");

        }

        public void HandleCancel()
        {
            throw new NotImplementedException();
        }

        public void HandleCharge()
        {
            if (!currentAbility.hasCharging()) return;
            currentAbility.Charging(() => { }, () => { });
        }

        public void HandleCharged()
        {
            throw new NotImplementedException();
        }

        public void HandleOngoing()
        {
            if (!currentAbility.hasOngoing()) return;
            currentAbility.Ongoing();
        }
    }
}
