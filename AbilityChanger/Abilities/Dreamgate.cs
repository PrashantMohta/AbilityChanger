namespace AbilityChanger
{
    public class Dreamgate : AbilityManager, IStartable, IChargable, ICancellable, ITriggerable, IOngoing, ICompletable, ISpawnable
    {
        // wont actually be usable till atleast 1 dreamNail ability is also acquired
        public override string abilityName { get; protected set; } = Abilities.DREAMGATE;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamGate);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DREAMGATE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DREAMGATE";
        public Dreamgate() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Dream Gate");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail")
            {
                #region Start
                self.AddFirstAction("Set Charge Start", new CustomFsmAction(() =>
                {
                    HandleStart();

                }));
                #endregion

                #region Charging
                self.AddFirstAction("Set Charge", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));

                #endregion

                #region Charged
                self.AddFirstAction("Set Antic", new CustomFsmAction(() =>
                {
                    HandleCharged();
                }));

                #endregion

                #region Cancel
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Set Charge Start",
                    toStateDefault="Set Recover",
                    eventName="CANCEL",
                    toStateCustom="Set Recover",
                    shouldIntercept=()=> currentAbility.hasCancel(),
                    onIntercept=(a,b)=>HandleCancel()

                });
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Set Charge",
                    toStateDefault = "Set Recover",
                    eventName = "CANCEL",
                    toStateCustom = "Set Recover",
                    shouldIntercept = () => currentAbility.hasCancel(),
                    onIntercept = (a, b) => HandleCancel()

                });
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Set Antic",
                    toStateDefault = "Set Recover",
                    eventName = "CANCEL",
                    toStateCustom = "Set Recover",
                    shouldIntercept = () => currentAbility.hasCancel(),
                    onIntercept = (a, b) => HandleCancel()

                });

                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Set Charge Start",
                    toStateDefault="Set Charge",
                    eventName="FINISHED",
                    toStateCustom="Set Recover",
                    shouldIntercept=()=>currentAbility.hasTrigger(),
                    onIntercept=(a,b) => HandleTrigger()

                });

                #endregion

                #region OnGoing

                #endregion

                #region Complete
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Set",
                    toStateDefault="Set Recover",
                    toStateCustom="Set Recover",
                    eventName="FINISHED",
                    shouldIntercept=()=>currentAbility.hasComplete(),
                    onIntercept=(a,b)=>HandleComplete()

                });;

                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Set Fail",
                    toStateDefault = "Set Recover",
                    toStateCustom = "Set Recover",
                    eventName = "FINISHED",
                    shouldIntercept = () => currentAbility.hasComplete(),
                    onIntercept = (a, b) => HandleComplete()

                }); ;

                #endregion

                #region Spawn 
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Set",
                    toStateDefault="Spawn Gate",
                    toStateCustom="Set Recover",
                    eventName="FINISHED",
                    shouldIntercept=()=>currentAbility.hasSpawn(),
                    onIntercept=(a,b)=>HandleSpawn()

                });;

                #endregion




                /*
                #region Charged
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FINISHED",
                    toStateDefault="Set",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => currentAbility.hasCharged(),
                    onIntercept = (fsmstate,fsmevent) => HandleCharged()
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Set?",
                    eventName ="FAIL",
                    toStateDefault="Set Fail",
                    toStateCustom="Set Recover",
                    shouldIntercept = () => currentAbility.hasCharged(),
                    onIntercept = (fsmstate, fsmevent) => HandleCharged()
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FINISHED",
                    toStateDefault="Check Scene",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => currentAbility.hasCharged(),
                    onIntercept = (fsmstate, fsmevent) => HandleCharged()
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="FAIL",
                    toStateDefault="Warp Fail",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => currentAbility.hasCharged(),
                    onIntercept = (fsmstate, fsmevent) => HandleCharged()
                });
                
                self.Intercept(new TransitionInterceptor(){
                    fromState ="Can Warp?",
                    eventName ="NO ESSENCE",
                    toStateDefault="Show Essence",
                    toStateCustom="Warp Cancel",
                    shouldIntercept = () => currentAbility.hasCharged(),
                    onIntercept = (fsmstate, fsmevent) => HandleCharged()
                });
                #endregion*/

            }
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dream Gate",
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

        public void HandleCancel()
        {
            if (!currentAbility.hasCancel()) return;
            currentAbility.Cancel();
        }

        public void HandleTrigger()
        {
            if (!currentAbility.hasTrigger()) return;
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleOngoing()
        {
            if (!currentAbility.hasOngoing()) return;
            currentAbility.Ongoing();
        }

        public void HandleComplete()
        {
            if (!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }

        public void HandleSpawn()
        {
            if (!currentAbility.hasSpawn()) return;
            currentAbility.Spawn();
        }
    }
}