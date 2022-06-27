namespace AbilityChanger
{
    public class GreatSlash : AbilityManager, IStartable, IChargable,ICancellable,ITriggerable,IContacting,ICompletable {
       
        public override string abilityName { get; protected set; } = Abilities.GREATSLASH;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDashSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_DASH";
        public GreatSlash() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Art Dash");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {

                #region Start
                self.AddAction("Flash 2", new CustomFsmAction(() =>
                {
                    HandleStart();
                }));

                #endregion

                #region Charging
                self.AddAction("Inactive", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));

                #endregion

                #region Charged
                self.AddAction("Inactive", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharged();
                }));

                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Flash 2",
                    eventName ="FINISHED",
                    toStateDefault="Facing?",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) =>HandleTrigger()
                });
                #endregion

                #region Complete
                self.InsertAction("G Slash End", new CustomFsmAction(() =>
                {
                    HandleComplete();
                }),0);


                #endregion


            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dash Slash",
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
            if (Satchel.Reflected.HeroControllerR.nailChargeTimer == 0 || (Satchel.Reflected.HeroControllerR.nailChargeTime < Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
            currentAbility.Charging(() => { }, () => { });
        }

        public void HandleCharged()
        {
            {
                if (!currentAbility.hasCharged()) return;
                if (!(Satchel.Reflected.HeroControllerR.nailChargeTime < Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
                currentAbility.Charged();
            }
        }

        public void HandleCancel()
        {
            throw new NotImplementedException();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON UP");
        }

        public void HandleContact(GameObject go = null)
        {
            throw new NotImplementedException();
        }

        public void HandleComplete()
        {
            if(!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);

        }
    }
}