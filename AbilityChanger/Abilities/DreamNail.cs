using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger
{
    public class DreamNail : AbilityManager, IStartable, IChargable, ICancellable, ITriggerable, IContacting, ICompletable,IOngoing
    {
        public override string abilityName { get; protected set; } = Abilities.DREAMNAIL;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamNail);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DREAMNAIL";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DREAMNAIL";
        public override GameObject getIconGo() => InvGo.Find("Dream Nail");
        public DreamNail() : base()
        {
            On.EnemyDreamnailReaction.RecieveDreamImpact += Contact;
        }

        private void Contact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            orig(self);
            HandleContact(self.gameObject);
        }

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail")
            {
                #region Start
                self.AddFirstAction("Start",new CustomFsmAction(() =>
                {
                    HandleStart();
                }));

                #endregion

                #region Charging
                self.AddFirstAction("Slash Antic", new CustomFsmActionFixedUpdate(()=>
                {
                    HandleCharge();
                }));
                #endregion

                #region Charged
                self.AddFirstAction("Slash", new CustomFsmAction(() =>
                {
                    HandleCharged();
                }));
                #endregion

                #region Cancel
                self.AddFirstAction("Charge Cancel", new CustomFsmAction(() =>
                {
                    HandleCancel();
                }));
                #endregion

                #region  Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Inactive",
                    toStateDefault="Can Dream Nail?",
                    eventName="BUTTON DOWN",
                    toStateCustom="Inactive",
                    shouldIntercept=()=>currentAbility.hasTrigger(),
                    onIntercept=(a,b)=>HandleTrigger()



                });

                #endregion

                #region Complete
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Slash",
                    toStateDefault="Cancelable",
                    eventName="FINISHED",
                    toStateCustom="Cancelable",
                    shouldIntercept=()=>currentAbility.hasComplete(),
                    onIntercept=(a,b)=>HandleComplete()
                });

                #endregion

                #region OnGoing
                self.AddFirstAction("Slash", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));
                #endregion


            }

            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Dream Nail",
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
            if(!currentAbility.hasCharged()) return;
            currentAbility.Charged();
        }

        public void HandleCancel()
        {
            if (!currentAbility.hasCancel()) return;
            currentAbility.Cancel();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleContact(GameObject go = null)
        {
            if(!currentAbility.hasContact()) return;
            currentAbility.Contact(go);
        }

        public void HandleComplete()
        {
            currentAbility.Complete(false);
        }

        public void HandleOngoing()
        {
            if (!currentAbility.hasOngoing()) return;
            currentAbility.Ongoing();
        }
    }








}