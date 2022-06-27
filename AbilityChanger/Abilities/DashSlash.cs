namespace AbilityChanger
{
    public class DashSlash : AbilityManager, IStartable,IChargable,ICancellable,ITriggerable,IContacting,ICompletable {
       
        public override string abilityName { get; protected set; } = Abilities.DASHSLASH;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasUpwardSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_UPPER";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_UPPER";
        public DashSlash() : base (){}        
        public override GameObject getIconGo() => InvGo.Find("Art Uppercut");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Dash Slash" && self.FsmName == "damages_enemy")
            {
                Log("oi");
                #region Contact
                /*self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Grandparent",
                    toStateDefault = "Idle",
                    eventName = "FINISHED",
                    toStateCustom = "Idle",
                    shouldIntercept = () => currentAbility.hasContact(),
                    onIntercept = (fsmstate, fsmevent) => HandleContact(self.FsmVariables.FindFsmGameObject("Collider").Value)

                });*/
                self.InsertAction("Send Event", new CustomFsmAction(() => Log("ola")), 9);// HandleContact(self.FsmVariables.FindFsmGameObject("Collider").Value)), 5);

                self.InsertAction("Parent", new CustomFsmAction(() => Log("ola1")), 5);// HandleContact(self.FsmVariables.FindFsmGameObject("Collider").Value)), 5);
                self.InsertAction("Grandparent", new CustomFsmAction(() => Log("ola2")), 5);// HandleContact(self.FsmVariables.FindFsmGameObject("Collider").Value)), 5);

                #endregion
            }



            if (self.gameObject.name == "Knight" && self.FsmName == "Nail Arts")
            {
                #region Start
                /*self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Dash Slash Ready",
                    eventName = "DASH END",
                    toStateDefault = "DSlash Start",
                    toStateCustom = "DSlash Start",
                    shouldIntercept = () => currentAbility.hasStart(),
                    onIntercept = (fsmstate, fsmevent) => HandleStart()
                });*/

                self.AddAction("Dash Slash",new CustomFsmAction(()=> HandleStart()));

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
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Dash Slash Ready",
                    eventName = "DASH END",
                    toStateDefault = "DSlash Start",
                    toStateCustom = "Regain Control",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => HandleTrigger()
                }); ;
                #endregion

                #region Complete
                /*self.Intercept(new TransitionInterceptor()
                {
                    fromState = "D Slash End",
                    eventName = "FINISHED",
                    toStateDefault = "Regain Control" +
                    "",
                    toStateCustom = "Regain Control",
                    shouldIntercept = () => currentAbility.hasComplete(),
                    onIntercept = (fsmstate, fsmevent) => HandleComplete()
                }); ;*/
                self.AddAction("D Slash End",new CustomFsmAction(HandleComplete));
                #endregion


            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Uppercut",
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

        public void HandleCharged()
        {
            if (!currentAbility.hasCharged()) return;
            if (!(Satchel.Reflected.HeroControllerR.nailChargeTime < Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
            currentAbility.Charged();
        }
        public void HandleCharge()
        {
            if (!currentAbility.hasCharging()) return;
            if (Satchel.Reflected.HeroControllerR.nailChargeTimer == 0 || (Satchel.Reflected.HeroControllerR.nailChargeTime < Satchel.Reflected.HeroControllerR.nailChargeTimer)) return;
            currentAbility.Charging(() => { }, () => { });
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
            Log(go);
            if(!currentAbility.hasContact()) return;
            currentAbility.Contact(go);
        }

        public void HandleComplete()
        {
            if(!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }
    }
}