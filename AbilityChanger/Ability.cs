namespace AbilityChanger
{

    public abstract class Ability{
        public abstract string name { get; set; }
        public abstract string title { get; set; }
        public abstract string description { get; set; }
        public abstract bool isCustom { get; set; }
        public abstract Func<bool> hasAbility { get; set; }
        public abstract Sprite activeSprite { get; set; }
        public abstract Sprite inactiveSprite { get; set; }
        public Ability(){}
        public virtual void handleAbilityUse(string interceptedState,string interceptedEvent){}
    }

    public class DefaultAbility : Ability
    {
        public override string name { get; set; }
        public override string title { get; set; }
        public override string description { get; set; }
        public override bool isCustom { get; set; }
        public override Func<bool> hasAbility { get; set; }
        public override Sprite activeSprite { get; set; }
        public override Sprite inactiveSprite { get; set; }

        public DefaultAbility(string name, Func<bool> hasAbility, bool isCustom = true)
        {
            this.isCustom = isCustom;
            this.name = name;
            this.hasAbility = hasAbility;
        }
    }
}