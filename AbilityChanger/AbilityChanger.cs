
namespace AbilityChanger
{
    public class AbilityChanger : Mod
    {
        public override string GetVersion() => "0.1";

        internal static Dictionary<string,AbilityManager> AbilityMap = new(){
            { Abilities.DREAMGATE,new Dreamgate()},
            { Abilities.CYCLONESLASH,new CycloneSlash()},
            { Abilities.GREATSLASH,new GreatSlash()},
            { Abilities.DASHSLASH,new DashSlash()},
            { Abilities.DASH,new Dash()},
            { Abilities.DOUBLEJUMP,new DoubleJump()},
            { Abilities.WALLJUMP,new WallJump()},
            { Abilities.NAIL,new Nail()},
            { Abilities.FIREBALL,new Fireball()},
            { Abilities.QUAKE,new Quake()},
            { Abilities.SCREAM,new Scream()},
            {Abilities.FOCUS,new Focus()},
            {Abilities.SUPERDASH, new SuperDash() },
            {Abilities.DREAMNAIL, new DreamNail() },
        };
        /// <summary>
        /// Register a new ability for the given ability type
        /// </summary>
        /// <param name="abilityType">A valid ability type</param>
        /// <param name="ability">An Ability</param>
        public static void RegisterAbility(string abilityType, Ability ability)
        {
            AbilityMap[abilityType].addAbility(ability);
        }
        /// <summary>
        /// Deregister an ability from ability changer
        /// </summary>
        /// <param name="abilityType">A valid ability type</param>
        /// <param name="ability">An Ability</param>
        public static void DeregisterAbility(string abilityType, Ability ability)
        {
            AbilityMap[abilityType].removeAbility(ability.name);
        }
        public override void Initialize()
        {
            ModHooks.GetPlayerBoolHook += PlayerDataPatcher.OnGetPlayerBoolHook;
            On.PlayMakerFSM.OnEnable += Equipment.OnFsmEnable;
        }

    }
}
