using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using Satchel;
using AbilityChanger;
using static AbilityChanger.AbilityChanger;
namespace AbilityChangerExample {

    public class greenflowerPlanter : Ability{
        static string name = "flower_planter";
        static string title = "Green Flower";
        static string description = "Ability to plant pretty flowers where you stand. Use it in the same way as setting a dream gate.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower3.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        
        static Func<bool> hasAbility = () => true;
        public greenflowerPlanter() : base (greenflowerPlanter.name,greenflowerPlanter.title,greenflowerPlanter.description,greenflowerPlanter.getActiveSprite(),greenflowerPlanter.getInactiveSprite(),greenflowerPlanter.hasAbility){

        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){ 
            if(interceptedState == "Can Set?"){
                AbilityChangerExample.plantFlower(0); 
            }
            if(interceptedState == "Can Warp?"){
                Modding.Logger.Log("Green warp"); 
            }
        }

    }
    public class redflowerPlanter : Ability{
        static string name = "flower_planter2";
        static string title = "Red Flower";
        static string description = "Ability to plant pretty flowers where you stand. Use it in the same way as setting a dream gate.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower2.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Func<bool> hasAbility = () => true;
        public redflowerPlanter() : base (redflowerPlanter.name,redflowerPlanter.title,redflowerPlanter.description,redflowerPlanter.getActiveSprite(),redflowerPlanter.getInactiveSprite(),redflowerPlanter.hasAbility){

        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
            if(interceptedState == "Can Set?"){
                AbilityChangerExample.plantFlower(2); 
            }
            if(interceptedState == "Can Warp?"){
                Modding.Logger.Log("Red warp"); 
            }
        }

    }
    public class redflowerCyclone : Ability{
        static string name = "flower_cyclone2";
        static string title = "Red Flower";
        static string description = "Ability to furiously plant pretty flowers around where you stand. Use it in the same way as a cyclone slash.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower2.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Func<bool> hasAbility = () => true;
        public redflowerCyclone() : base (redflowerCyclone.name,redflowerCyclone.title,redflowerCyclone.description,redflowerCyclone.getActiveSprite(),redflowerCyclone.getInactiveSprite(),redflowerCyclone.hasAbility){

        }
        public Coroutine coroutine;
        public IEnumerator StopPlanting(){
            yield return new WaitForSeconds(2f);
            CoroutineHelper.GetRunner().StopCoroutine(coroutine);
        }
        public IEnumerator Planting(){
            while(true){
                AbilityChangerExample.plantFlower(2); 
                yield return new WaitForSeconds(0.02f);
            }
        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
            
            AbilityChangerExample.plantFlower(2); 
            coroutine = CoroutineHelper.GetRunner().StartCoroutine(Planting());
            CoroutineHelper.GetRunner().StartCoroutine(StopPlanting());

        }

    }

    public class flowerPlanter3 : Ability{
        static string name = "flower_planter3";
        static string title = "White Flower";
        static string description = "Ability to plant pretty flowers where you stand. Use it in the same way as great slash.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Func<bool> hasAbility = () => true;
        public flowerPlanter3() : base (flowerPlanter3.name,flowerPlanter3.title,flowerPlanter3.description,flowerPlanter3.getActiveSprite(),flowerPlanter3.getInactiveSprite(),flowerPlanter3.hasAbility){

        }
        
        public Coroutine coroutine;
        public IEnumerator StopPlanting(){
            yield return new WaitForSeconds(5f);
            CoroutineHelper.GetRunner().StopCoroutine(coroutine);
        }
        public IEnumerator Planting(){
            while(true){
                AbilityChangerExample.plantFlower(1); 
                yield return new WaitForSeconds(0.1f);
            }
        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
             
            AbilityChangerExample.plantFlower(1); 
            coroutine = CoroutineHelper.GetRunner().StartCoroutine(Planting());
            CoroutineHelper.GetRunner().StartCoroutine(StopPlanting());

        }

    }

    
    public class flowerPlanter4 : Ability{
        static string name = "flower_planter4";
        static string title = "White Flower 2";
        static string description = "Ability to plant pretty flowers where you stand. Use it in the same way as Dash slash.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}

        static Func<bool> hasAbility = () => true;
        public flowerPlanter4() : base (flowerPlanter4.name,flowerPlanter4.title,flowerPlanter4.description,flowerPlanter4.getActiveSprite(),flowerPlanter4.getInactiveSprite(),flowerPlanter4.hasAbility){

        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
            AbilityChangerExample.plantFlower(1); 
        }

    }

    public class teleport : Ability{
        static string name = "Teleport";
        static string title = "Teleport";
        static string description = "Ability to Teleport.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}

        static Func<bool> hasAbility = () => true;
        public teleport() : base (teleport.name,teleport.title,teleport.description,teleport.getActiveSprite(),teleport.getInactiveSprite(),teleport.hasAbility){

        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
            var looking_right = HeroController.instance.transform.localScale.x > 0;
            var pos = HeroController.instance.transform.position;
            pos.x += looking_right ? -3f : 3f;
            HeroController.instance.transform.position = pos;
            Satchel.Reflected.HeroControllerR.FinishedDashing();
        }

    }

    
    public class superJump : Ability{
        static string name = "superJump";
        static string title = "superJump";
        static string description = "Ability to superJump.";
        static Sprite getActiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}
        static Sprite getInactiveSprite(){ return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png");}

        static Func<bool> hasAbility = () => true;
        public superJump() : base (superJump.name,superJump.title,superJump.description,superJump.getActiveSprite(),superJump.getInactiveSprite(),superJump.hasAbility){

        }
        private bool routineRunning = false;
        public IEnumerator hover(){
            var rb2d = HeroController.instance.GetComponent<Rigidbody2D>();
            /*var pos = HeroController.instance.transform.position;
            pos.y += 0.1f;
            HeroController.instance.transform.position = pos;*/            
            yield return rb2d.moveTowards(new Vector2(0,1f),3f,0.1f);
            
            yield return null;
            Satchel.Reflected.HeroControllerR.CancelDoubleJump();
            routineRunning = false;
        }
        public override void handleAbilityUse(string interceptedState,string interceptedEvent){
            if(!routineRunning){
                CoroutineHelper.GetRunner().StartCoroutine(hover());
            }
        }

    }
    public class AbilityChangerExample : Mod
    {
        public static GameObject flower,flower2,flower3;
        public override string GetVersion() => "0.1";
        public override void Initialize()
        {

            flower = new GameObject(){
                name = "flower"
            };
            SpriteRenderer sr = flower.AddComponent<SpriteRenderer>();
            Texture2D tex =  AssemblyUtils.GetTextureFromResources("flower.png");
            sr.sprite = Sprite.Create(tex,new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),128f,0,SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower.SetActive(false);
            GameObject.DontDestroyOnLoad(flower);

            flower2 = new GameObject(){
                name = "flower2"
            };
            sr = flower2.AddComponent<SpriteRenderer>();
            tex =  AssemblyUtils.GetTextureFromResources("flower2.png");
            sr.sprite = Sprite.Create(tex,new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),128f,0,SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower2.SetActive(false);
            GameObject.DontDestroyOnLoad(flower2);

            flower3 = new GameObject(){
                name = "flower3"
            };
            sr = flower3.AddComponent<SpriteRenderer>();
            tex =  AssemblyUtils.GetTextureFromResources("flower3.png");
            sr.sprite = Sprite.Create(tex,new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),128f,0,SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower3.SetActive(false);
            GameObject.DontDestroyOnLoad(flower3);

            AbilityMap[Dreamgate.abilityName].addAbility(new greenflowerPlanter());
            AbilityMap[Dreamgate.abilityName].addAbility(new redflowerPlanter());
            //AbilityMap[CycloneSlash.abilityName].addAbility(new redflowerCyclone());
            AbilityMap[GreatSlash.abilityName].addAbility(new flowerPlanter3());
            AbilityMap[DashSlash.abilityName].addAbility(new flowerPlanter4());
            AbilityMap[Dash.abilityName].addAbility(new teleport());
            AbilityMap[DoubleJump.abilityName].addAbility(new superJump());
        }

        public static void plantFlower(int DreamnailType = 0){
            GameObject f = null;
            if(DreamnailType == 1){
                f = GameObject.Instantiate(AbilityChangerExample.flower);
            }else if(DreamnailType == 2){
                f = GameObject.Instantiate(AbilityChangerExample.flower2);
            }else {
                f = GameObject.Instantiate(AbilityChangerExample.flower3);
            }
            f.transform.position = HeroController.instance.transform.position + new Vector3(UnityEngine.Random.Range(-0.1f,0.1f),UnityEngine.Random.Range(0.2f,-0.2f)-1f,-0.01f);
            f.SetActive(true);
        }
    }
}
