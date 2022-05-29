
namespace AbilityChanger
{
    public class AbilityChanger : Mod
    {
        public override string GetVersion() => "0.1";

        public static Dictionary<string,AbilityManager> AbilityMap = new(){
            {Dreamgate.abilityName,new Dreamgate()},
            {CycloneSlash.abilityName,new CycloneSlash()},
            {GreatSlash.abilityName,new GreatSlash()},
            {DashSlash.abilityName,new DashSlash()},
            {Dash.abilityName,new Dash()},
            {DoubleJump.abilityName,new DoubleJump()},
            {WallJump.abilityName,new WallJump()},
            {Nail.abilityName,new Nail()},
            {Fireball.abilityName,new Fireball()},
            {Quake.abilityName,new Quake()},
            {Scream.abilityName,new Scream()},
            {Focus.abilityName,new Focus()},

        };
        public override void Initialize()
        {
            ModHooks.GetPlayerBoolHook += PlayerDataPatcher.OnGetPlayerBoolHook;
            On.PlayMakerFSM.OnEnable += Equipment.OnFsmEnable;
        }

    }
}
