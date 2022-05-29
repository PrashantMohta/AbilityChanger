namespace AbilityChanger
{

    public abstract class Ability{
        public abstract string name { get; set; }
        public abstract string title { get; set; }
        public abstract string description { get; set; }
        public abstract Sprite activeSprite { get; set; }
        public abstract Sprite inactiveSprite { get; set; }
        public Ability(){ }
        public abstract bool hasAbility();
        public bool isCustom { get; set; } = true;
        public virtual void handleAbilityUse(string interceptedState,string interceptedEvent){}
    }

    public class DefaultAbility : Ability
    {
        public override string name { get; set; }
        public override string title { get; set; }
        public override string description { get; set; }
        public Func<bool> _hasAbility { get; set; }
        public override Sprite activeSprite { get; set; }
        public override Sprite inactiveSprite { get; set; }

        public DefaultAbility(string name, Func<bool> hasAbility)
        {
            this.isCustom = false;
            this.name = name;
            this._hasAbility = hasAbility;
        }

        public override bool hasAbility()
        {
            return this._hasAbility();
        }
    }
}