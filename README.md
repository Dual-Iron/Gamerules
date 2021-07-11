Add and use gamerules that can be modified in real-time through a configuration file.

## Users: Reading and modifying gamerules

You can use a few hotkeys while Rain World is in focus to control your gamerules.
- Ctrl F1: Prints all gamerules in the console. You can find your console in `Rain World/BepInEx/LogOutput.log` .
- Ctrl F2: Reads all gamerules from the file `Rain World/UserData/Gamerules.json`. The input can be formatted or not.
- Ctrl F3: Writes all gamerules into the file `Rain World/UserData/Gamerules.json`. The output is unformatted.

Your Rain World folder will be with the rest of your Steam apps (e.g. `C:/Program Files (x86)/Steam/steamapps/common/Rain World`).

## Modders: Adding as a dependency

Just add the mod as a reference in your project. See [GameruleSet/Rules.cs](https://github.com/Dual-Iron/GameruleSet/blob/master/Rules.cs) for an example.

### Making the dependency optional

To make the dependency optional, keep all references to Gamerules.dll in a dedicated method and call that method within a try-catch.

<details>
<summary>Code example</summary> 
  
```cs
// In a plugin class...
    
public int ExampleRule;

public void OnEnable() {
    try { 
        LoadGamerules(); 
    }
    catch { 
    }
}

private void LoadGamerules() {
    new IntRuleBuilder()
        .Default(0)
        .Min(0)
        .Max(10)
        .Description("This is an example rule.")
        .OnUpdate(x => ExampleRule = x)
        .Register("my_mod/example_rule");
}
```

</details>

## Planned
- [ ] In-game UI for reading/writing gamerules in the pause menu and in the main menu
- [ ] Option to lock a set of gamerules for a save file permanently
- [ ] Maybe a ListRule
