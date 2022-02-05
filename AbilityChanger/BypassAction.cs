using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using Logger = Modding.Logger;
using Satchel;
namespace AbilityChanger
{
    public class BypassAction : FsmStateAction{
        public Func<string> method;
        public override void Reset()
        {
            method = null;
            base.Reset();
        }

        public override void OnEnter()
        {
            base.Fsm.Event(method?.Invoke());
            Finish();
        }
    }

    public struct InterceptorParams{
        public string fromState;
        public string eventName;
        public string toStateDefault;
        public string toStateCustom;
    }
    public class Interceptor{
        public string fromState;
        public string eventName;
        public string toStateDefault;
        public string toStateCustom;
        public AbilityManager ability;
        public static string defaultEvent = "Intercept-DEFAULT";
        public static string customEvent = "Intercept-CUSTOM";

        public Interceptor(AbilityManager ability,InterceptorParams param){
            this.ability = ability;
            this.fromState = param.fromState;
            this.eventName = param.eventName;
            this.toStateDefault = param.toStateDefault;
            this.toStateCustom = param.toStateCustom;
        }

        public string GetNextEvent(string interceptedState,string interceptedEvent){ 
            if(ability.isCustom()){
                ability.handleAbilityUse(interceptedState,interceptedEvent);
                return Interceptor.customEvent;
            }  
            return Interceptor.defaultEvent;
        }
        public string GetNextEventOrDefault(string interceptedState,string interceptedEvent){ return GetNextEvent(interceptedState,interceptedEvent) ?? Interceptor.defaultEvent; }

    }
    public static class InterceptorExtensions{
        public static void InterceptTransition(this PlayMakerFSM fsm, Interceptor interceptor){
           var interceptorName = $"Intercept_{interceptor.fromState}_{interceptor.eventName}";
           Modding.Logger.Log(interceptorName);
           var interceptionState = new FsmState(fsm.Fsm){Name=interceptorName};
           interceptionState.InsertAction(new BypassAction(){method = () => interceptor.GetNextEventOrDefault(interceptor.fromState,interceptor.eventName)},0);
           fsm.AddState(interceptionState);
           fsm.ChangeTransition(interceptor.fromState,interceptor.eventName,interceptorName);
           fsm.AddTransition(interceptorName,Interceptor.defaultEvent,interceptor.toStateDefault);
           fsm.AddTransition(interceptorName,Interceptor.customEvent,interceptor.toStateCustom);
        }

        public static void EventInterceptor(this PlayMakerFSM fsm,string state,string eventName,Action handler){
            var interceptorName = $"Intercept_Event_{state}_{eventName}";
            var interceptionState = new FsmState(fsm.Fsm){Name=interceptorName};
                interceptionState.InsertAction(new BypassAction(){method = () => { handler(); return $"back to {state}";}},0);
                fsm.AddState(interceptionState);
                fsm.AddTransition(state,eventName,interceptorName);
                fsm.AddTransition(interceptorName,$"back to {state}",state);
        }
    }

}