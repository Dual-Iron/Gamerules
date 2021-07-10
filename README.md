Example optional dependency.

```cs
private int myValue;

public void OnEnable() {
	try {
		LoadGamerules();
	}
	catch { }
}

private void LoadGamerules() {
	new IntRuleBuilder()
        .Default(0)
        .Min(0)
        .Max(10)
        .Description("This is an example rule.")
        .OnUpdate(x => myValue = x)
        .Register("my_mod/example_rule");
}
```