namespace AbilityChanger
{
    public class Fireball : AbilityManager, IStartable, ITriggerable, IContacting, ICompletable
    {

        public override string abilityName { get; protected set; } = Abilities.FIREBALL;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.fireballLevel)) > 0;
        public override string inventoryTitleKey { 
            get =>$"INV_NAME_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
            
            protected set { }
        } 
        public override string inventoryDescKey
        {
            get => $"INV_DESC_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
            protected set { }
        }

        public Fireball() : base() { }
        public override GameObject getIconGo() => InvGo.Find("Spell Fireball");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {

            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {

                #region Start
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Level Check",
                    toStateDefault="Fireball 1",
                    toStateCustom="Fireball Recoil",
                    eventName="LEVEL 1",
                    shouldIntercept=()=> currentAbility.hasStart(),
                    onIntercept=(a,b)=> HandleStart()
                });

                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Level Check",
                    toStateDefault = "Fireball 2",
                    toStateCustom = "Fireball Recoil",
                    eventName = "LEVEL 2",
                    shouldIntercept = () => currentAbility.hasStart(),
                    onIntercept = (a, b) => HandleStart()
                });
                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState = "Has Fireball?",
                    eventName = "CAST",
                    toStateDefault = "Wallside?",
                    toStateCustom = "Inactive",
                    shouldIntercept = () => currentAbility.hasTrigger(),
                    onIntercept = (fsmstate, fsmevent) => HandleTrigger()
                });
                #endregion




                #region Complete
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Fireball Recoil",
                    toStateDefault="Spell End",
                    toStateCustom="Spell End",
                    eventName="ANIM END",
                    shouldIntercept= () => currentAbility.hasComplete(),
                    onIntercept = (fsmstate, fsmevent) => HandleComplete()
                });

                #endregion

            }

            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Fireball",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }
            orig(self);
        }

        public void HandleStart()
        {
            currentAbility.Start();
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
            currentAbility.Complete(false);
        }

    }
}
