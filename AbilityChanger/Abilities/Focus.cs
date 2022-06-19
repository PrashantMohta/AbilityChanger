namespace AbilityChanger
{
    public class Focus : AbilityManager, IStartable, IChargable, ITriggerable, ICancellable, ICompletable

    {

        public override string abilityName { get; protected set; } = Abilities.FOCUS;
        public override bool hasDefaultAbility()  => true;
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_SPELL_FOCUS";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_SPELL_FOCUS";
        public Focus() : base() { }
        public override GameObject getIconGo() => InvGo.Find("Spell Focus");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                #region Start
                self.AddAction("Focus Start",new CustomFsmAction(()=>
                {
                    HandleStart();

                }));
                #endregion

                #region Charging
                self.AddAction("Focus", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));
                #endregion

                #region Charged
                self.AddAction("Focus Heal", new CustomFsmAction(() =>
                {
                    HandleCharged();
                }));
                #endregion

                #region Cancel
                self.AddAction("Focus Cancel", new CustomFsmAction(() =>
                {
                    HandleCancel();

                }));

                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Can Focus?",
                    toStateDefault="Start Slug Anim",
                    eventName="FINISHED",
                    toStateCustom="Regain Control",
                    shouldIntercept=()=> currentAbility.hasTrigger(),
                    onIntercept=(a,b)=> HandleTrigger() 
                });

                #endregion




                #region Complete
                /*self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Focus",
                    eventName = "FOCUS COMPLETED",
                    toStateDefault = "Spore Cloud",
                    toStateCustom = "Full HP?",
                    shouldIntercept = () => currentAbility.hasComplete(),
                    onIntercept = (fsmstate, fsmevent) => this.handleAbilityUse(fsmstate, fsmevent)
                });*/
                self.AddAction("Focus Heal", new CustomFsmAction(() =>
                {
                    HandleComplete();
                }));

                #endregion

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Focus",
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
            if (!currentAbility.hasStart()) return;
            currentAbility.Start();
        }

        public void HandleCharge()
        {
            if (!currentAbility.hasCharging()) return;
            currentAbility.Charging(() => { }, () => { });
        }

        public void HandleCharged()
        {
            if (!currentAbility.hasCharged()) return;
            currentAbility.Charged();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");

        }

        public void HandleCancel()
        {
            if(!currentAbility.hasCancel()) return;
            currentAbility.Cancel();
        }

        public void HandleComplete()
        {
            if (!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }
    }
}
