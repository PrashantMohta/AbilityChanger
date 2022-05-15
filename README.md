# AbilityChanger
A Library Mod that allows giving the player alternative abilities to the ones they already possess, modifies inventory so that other mods don't have to.

## Currently supported abilities 

- Dreamgate
- CycloneSlash
- GreatSlash
- DashSlash
- Walljump
- Doublejump
- Dash

This mod overrides the PlayerData related to the abilities using [PlayerDataPatcher](https://github.com/PrashantMohta/AbilityChanger/blob/main/AbilityChanger/Ability/PlayerDataPatcher.cs).

[Example of adding your own ability](https://github.com/PrashantMohta/AbilityChanger/blob/main/Example/AbilityChangerExample.cs#L21)
The consumer mod needs to define a func<bool> that will be used to determine if their ability is available to be used or not 

In case any one of the abilities is available (vanilla or not) the player data will be set, so replaced abilities should serve similar purposes as the vanilla ones.
This approach also has some interesting side effects such as (but not limited to) :
- Any dream gate abilities require the player to have at-least one dream nail ability, before they can be used in the game. 
