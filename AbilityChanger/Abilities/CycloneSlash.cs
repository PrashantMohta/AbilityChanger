namespace AbilityChanger
{
    public class CycloneSlash : AbilityManager, IStartable, IChargable,ITriggerable, IOngoing,ICompletable, ICancellable, IContacting{
       
        public override string abilityName { get; protected set; } = Abilities.CYCLONESLASH;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasCyclone);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_CYCLONE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_CYCLONE";

        public CycloneSlash() : base ()
        {
            

        }
        public override GameObject getIconGo() => InvGo.Find("Art Cyclone");

        

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if ((self.gameObject.name == "Hit L" || self.gameObject.name == "Hit R") && self.FsmName == "damages_enemy")
            {
                #region Contact
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Send Event",
                    toStateDefault="Parent",
                    eventName="FINISHED",
                    toStateCustom="Parent",
                    shouldIntercept=()=> currentAbility.hasContact(),
                    onIntercept=(fsmstate,fsmevent)=> HandleContact(self.FsmVariables.FindFsmGameObject("Collider").Value)

                });

                #endregion
            }

            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {

                #region Start
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Flash",
                    eventName = "FINISHED",
                    toStateDefault = "Cyclone Start",
                    toStateCustom = "Cyclone Start",
                    shouldIntercept = ()=> currentAbility.hasStart(),
                    onIntercept = (fsmstate, fsmevent) =>HandleStart()
                });
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

                #region Cancel
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Cyclone?",
                    eventName = "CANCEL",
                    toStateDefault = "Regain Control",
                    toStateCustom = "Regain Control",
                    shouldIntercept = () => currentAbility.hasCancel(),
                    onIntercept = (fsmstate, fsmevent) => HandleCancel()
                });
                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Has Cyclone?",
                    eventName ="FINISHED",
                    toStateDefault="Flash",
                    toStateCustom="Regain Control",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate,fsmevent) => HandleTrigger()
                });
                #endregion

                #region OnGoing

                self.AddAction("Cyclone Spin", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));

                self.AddAction("Cyclone Extend", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));


                #endregion

                #region Complete
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Cyclone End",
                    eventName = "FINISHED",
                    toStateDefault = "Cyc Send Msg",
                    toStateCustom = "Cyc Send Msg",
                    shouldIntercept = () => currentAbility.hasComplete(),
                    onIntercept = (fsmstate, fsmevent) => HandleComplete()
                });
                #endregion

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Cyclone",
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

        public void HandleCharged()
        {
            if (!currentAbility.hasCharged()) return;
            if(!(Satchel.Reflected.HeroControllerR.nailChargeTime< Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
            currentAbility.Charged();
        }
        public void HandleCharge() {
            if (!currentAbility.hasCharging()) return;
            if (Satchel.Reflected.HeroControllerR.nailChargeTimer ==0 || (Satchel.Reflected.HeroControllerR.nailChargeTime < Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
            currentAbility.Charging(() => { }, () => { });
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON UP");
        }

        public void HandleOngoing()
        {
            if (!currentAbility.hasOngoing()) return;
            currentAbility.Ongoing();
        }

        public void HandleComplete()
        {
            currentAbility.Complete(false);
        }

        public void HandleCancel()
        {
            currentAbility.Cancel();
        }

        public void HandleContact(GameObject go)
        {
            if (!currentAbility.hasContact()) return;
            currentAbility.Contact(go);

        }
    }
}