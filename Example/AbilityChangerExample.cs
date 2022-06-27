using AbilityChanger;
using Modding;
using Satchel;
using System;
using System.Collections;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using static AbilityChanger.AbilityChanger;


namespace AbilityChangerExample
{

    public class greenflowerPlanter : Ability
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower3.png"); }
        static Sprite getInactiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png"); }


        public override string name { get => "flower_planter"; set { } }
        public override string title { get => "Green Flower"; set { } }
        public override string description { get => "Ability to plant pretty flowers where you stand. Use it in the same way as setting a dream gate."; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }
        public override Sprite inactiveSprite { get => getInactiveSprite(); set { } }

        public greenflowerPlanter() { }
        public override bool hasAbility() => true;
        public void handleAbilityUse(string interceptedState, string interceptedEvent)
        {
            if (interceptedState == "Can Set?")
            {
                AbilityChangerExample.plantFlower(0);
            }
            if (interceptedState == "Can Warp?")
            {
                Modding.Logger.Log("Green warp");
            }
        }

    }

    public class flower : Ability
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png"); }
        static Sprite getInactiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png"); }
        public override string name { get => "Teleport"; set { } }
        public override string title { get => "Teleport"; set { } }
        public override string description { get => "Ability to Teleport."; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }
        public override Sprite inactiveSprite { get => getInactiveSprite(); set { } }
        public teleport() { }
        public override bool hasAbility() => true;

        public void handleAbilityUse()
        {
            AbilityChangerExample.plantFlower();
        }

        public override bool hasStart() => false;
        public override bool hasCharging() => false;
        public override bool hasCharged() => false;
        public override bool hasCancel() => false;
        public override bool hasTrigger() => false;
        public override bool hasOngoing() => false;
        public override bool hasContact() => false;
        public override bool hasComplete() => true;

        public override void Charged() => AbilityChangerExample.plantFlower();
        public override void Start() => AbilityChangerExample.plantFlower();
        public override void Charging(Action Next, Action Cancel) => AbilityChangerExample.plantFlower();
        public override void Cancel() => AbilityChangerExample.plantFlower();
        public override void Trigger(string type) => AbilityChangerExample.plantFlower();
        public override void Ongoing() => AbilityChangerExample.plantFlower();
        public override void Contact(GameObject other) => AbilityChangerExample.plantFlower();
        public override void Complete(bool wasCancelled) => AbilityChangerExample.plantFlower();

    }

    public class AbilityChangerExample : Mod
    {
        public static GameObject flower, flower2, flower3;
        public override string GetVersion() => "0.1";
        public override void Initialize()
        {
            #region Defining GOs
            flower = new GameObject()
            {
                name = "flower"
            };
            SpriteRenderer sr = flower.AddComponent<SpriteRenderer>();
            Texture2D tex = AssemblyUtils.GetTextureFromResources("flower.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower.SetActive(false);
            GameObject.DontDestroyOnLoad(flower);

            flower2 = new GameObject()
            {
                name = "flower2"
            };
            sr = flower2.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("flower2.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower2.SetActive(false);
            GameObject.DontDestroyOnLoad(flower2);

            flower3 = new GameObject()
            {
                name = "flower3"
            };
            sr = flower3.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("flower3.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower3.SetActive(false);
            GameObject.DontDestroyOnLoad(flower3);
            #endregion


            RegisterAbility(Abilities.DREAMGATE, new flower());
        }

        public static void plantFlower(int DreamnailType = 0)
        {
            GameObject f = null;
            if (DreamnailType == 1)
            {
                f = GameObject.Instantiate(AbilityChangerExample.flower);
            }
            else if (DreamnailType == 2)
            {
                f = GameObject.Instantiate(AbilityChangerExample.flower2);
            }
            else
            {
                f = GameObject.Instantiate(AbilityChangerExample.flower3);
            }
            f.transform.position = HeroController.instance.transform.position + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(0.2f, -0.2f) - 1f, -0.01f);
            f.SetActive(true);
        }
    }
}
