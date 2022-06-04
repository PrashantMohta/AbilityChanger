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
        public bool isCustom { get; protected set; } = true;

        // triggers to override the ability life-cycle

        /// <summary>
        /// Does this ability have a Start Method
        /// </summary>
        public virtual bool hasStart() => false;

        /// <summary>
        /// Runs before the ability starts (guaranteed to run before ability starts)
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Does this ability have a Charging Method
        /// </summary>
        public virtual bool hasCharging() => false;

        /// <summary>
        /// Overrides the Charged state of an ability
        /// </summary>
        /// <param name="Next">Action that finishes the charging state</param>
        /// <param name="Cancel">Action that cancels the ability</param>
        public virtual void Charging(Action Next,Action Cancel) { }

        /// <summary>
        /// Does this ability have a Charged Method
        /// </summary>
        public virtual bool hasCharged() => false;

        /// <summary>
        /// Overrides the Charged state of an ability
        /// </summary>
        /// <param name="Next">Action that finishes the charged state</param>
        /// <param name="Cancel">Action that cancels the ability</param>
        public virtual void Charged(Action Next, Action Cancel) { }

        /// <summary>
        /// Does this ability have a Cancel Method
        /// </summary>
        public virtual bool hasCancel() => false;

        /// <summary>
        /// Overrides the Cancel state of an ability
        /// </summary>
        public virtual void Cancel() { }

        /// <summary>
        /// Does this ability have a Trigger Method
        /// </summary>
        public virtual bool hasTrigger() => false;

        /// <summary>
        /// Overrides the Trigger state of an ability
        /// </summary>
        /// <param name="type">An ability-specific string determining the type of trigger(In case of multiple triggers for single ability) </param>
        public virtual void Trigger(string type) { }

        /// <summary>
        /// Does this ability have an Ongoing Method
        /// </summary>
        public virtual bool hasOngoing() => false;

        /// <summary>
        /// Overrides the Ongoing state of an ability (expected to run many times)
        /// </summary>
        public virtual void Ongoing() { }

        /// <summary>
        /// Does this ability have a Contact Method
        /// </summary>
        public virtual bool hasContact() => false;

        /// <summary>
        /// Overrides the Contact state of an ability (always triggers if a contact is made)
        /// </summary>
        /// <param name="other">The other gameObject</param>
        public virtual void Contact(GameObject other) { }

        /// <summary>
        /// Does this ability have a Complete Method
        /// </summary>
        public virtual bool hasComplete() => false;
        /// <summary>
        /// Runs After the ability has completed (guaranteed to run after the ability has completed, will run even if the ability was cancelled)
        /// </summary>
        /// <param name="wasCancelled">Denotes if the ability was cancelled</param>
        public virtual void Complete(bool wasCancelled) { }

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