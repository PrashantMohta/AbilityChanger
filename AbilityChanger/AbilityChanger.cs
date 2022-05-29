
namespace AbilityChanger
{
    public class AbilityChanger : Mod
    {
        public override string GetVersion() => "0.1";

        public static Dictionary<string,AbilityManager> AbilityMap = new(){
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

        };
        public override void Initialize()
        {
            ModHooks.GetPlayerBoolHook += PlayerDataPatcher.OnGetPlayerBoolHook;
            On.PlayMakerFSM.OnEnable += Equipment.OnFsmEnable;
        }

    }
}
