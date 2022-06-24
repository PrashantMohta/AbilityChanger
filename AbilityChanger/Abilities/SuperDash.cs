using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger
{
    public class SuperDash : AbilityManager, IStartable, IChargable, ICancellable, ITriggerable, IOngoing, IContacting, ICompletable
    {
        public override string abilityName { get; protected set; } = Abilities.SUPERDASH;
        public override bool hasDefaultAbility()=> PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasSuperDash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_SUPERDASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_SUPERDASH";
        public SuperDash():base()
        {
            
        }

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Knight" && self.FsmName == "Superdash")
            {
                #region Start
                self.AddFirstAction("Dash Start", new CustomFsmAction(() =>
                {
                    HandleStart();
                }));

                #endregion

                #region Charge
                self.AddFirstAction("Wall Charge", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));

                self.AddFirstAction("Ground Charge", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharge();
                }));

                #endregion

                #region Charged
                self.AddFirstAction("Wall Charged", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharged();
                }));

                self.AddFirstAction("Ground Charged", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleCharged();
                }));

                #endregion

                #region Cancel
                self.AddFirstAction("Air Cancel", new CustomFsmAction(() =>
                {
                    HandleCancel();
                }));

                self.AddFirstAction("Hit Wall", new CustomFsmAction(() =>
                {
                    HandleCancel();
                }));
                #endregion

                #region Trigger
                self.Intercept(new TransitionInterceptor()
                {
                    fromState="Can Superdash?",
                    toStateDefault="Relinquish Control",
                    eventName="FINISHED",
                    toStateCustom="Inactive",
                    shouldIntercept=()=> currentAbility.hasTrigger(),
                    onIntercept=(a,b)=> HandleTrigger()

                });

                #endregion

                #region OnGoing
                self.AddFirstAction("Dashing",new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));
                self.AddFirstAction("Cancelable", new CustomFsmActionFixedUpdate(() =>
                {
                    HandleOngoing();
                }));

                #endregion

                #region Contact

                #endregion

                #region Complete
                self.AddFirstAction("Regain Control",new CustomFsmAction(() =>
                {
                    HandleComplete();
                }));
                #endregion


            }






        }




        public override GameObject getIconGo()=>  InvGo.Find("Super Dash");

        public void HandleComplete()
        {
            if (!currentAbility.hasComplete()) return;
            currentAbility.Complete(false);
        }

        public void HandleContact(GameObject go = null)
        {
            throw new NotImplementedException();
        }

        public void HandleOngoing()
        {
            if (!currentAbility.hasOngoing()) return;
            currentAbility.Ongoing();
        }

        public void HandleTrigger()
        {
            currentAbility.Trigger("BUTTON DOWN");
        }

        public void HandleCancel()
        {
            if (!currentAbility.hasCancel()) return;
            currentAbility.Cancel();
        }

        public void HandleCharge()
        {
            if (!currentAbility.hasCharging()) return;
            currentAbility.Charging(()=>{ },()=>{ });
        }

        public void HandleCharged()
        {
            if(!currentAbility.hasCharged()) return;
            currentAbility.Charged();
        }

        public void HandleStart()
        {
            if (!currentAbility.hasStart()) return;
            currentAbility.Start();
        }
    }
}
