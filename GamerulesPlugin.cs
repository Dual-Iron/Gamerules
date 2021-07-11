using BepInEx;
using RWCustom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gamerules
{
    [BepInPlugin("com.github.dual.gamerules", "Gamerules", "1.0.0")]
    internal sealed class GamerulesPlugin : BaseUnityPlugin
    {
        public void OnEnable()
        {
            On.RainWorldGame.ctor += RainWorldGame_ctor;
            On.RainWorld.Start += RainWorld_Start;
            On.RainWorld.Update += RainWorld_Update;
        }

        public void OnDisable()
        {
            RuleAPI.rules.Clear();
        }

        private void RainWorld_Update(On.RainWorld.orig_Update orig, RainWorld self)
        {
            orig(self);
            try
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F3))
                {
                    CreateJsonFile();
                }
                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F2))
                {
                    LoadGamerules();
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F1))
                {
                    var sb = new StringBuilder("\n");
                    foreach (var rule in RuleAPI.rules)
                    {
                        sb.AppendLine(
                            $"\nname......{rule.Key}\n" +
                            $"default...{rule.Value.DefaultValue}\n" +
                            $"current...{rule.Value.Value}\n" +
                            $"desc......{rule.Value.Description}");
                    }
                    Logger.LogMessage(sb);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        private void CreateJsonFile()
        {
            var result = GenerateJson();
            if (result.IsOk)
            {
                try
                {
                    File.WriteAllText(GameruleFile, result.Ok);
                }
                catch (Exception e)
                {
                    Logger.LogError("Failed to write the JSON file: " + e.Message);
                }
                Logger.LogMessage("Successfully generated JSON.");
            }
            else
            {
                Logger.LogError("Failed to generate JSON: " + result.Err);
            }
        }

        private void RainWorld_Start(On.RainWorld.orig_Start orig, RainWorld self)
        {
            orig(self);
            LoadGamerules();
        }

        private void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
        {
            orig(self, manager);
            LoadGamerules();
        }

        private void LoadGamerules()
        {
            if (Read().Err is not string s)
            {
                Logger.LogMessage("Successfully loaded gamerules.");
            }
            else
            {
                Logger.LogError("Failed to load gamerules: " + s);
            }
        }

        private static string GameruleFile => Combine(Custom.RootFolderDirectory(), "UserData", $"Gamerules.json");

        private static string Combine(string first, params string[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                first = Path.Combine(first, other[i]);
            }
            return first;
        }

        private static Result Read()
        {
            string path = GameruleFile;

            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "{}");
                    return Result.FromOk();
                }

                var jsonResult = Json.Deserialize(File.ReadAllText(path));
                if (jsonResult is not Dictionary<string, object> d || d.Values.Any(v => v == null))
                    return Result.FromErr("Invalid JSON. Use jsonlint.com to validate and format your JSON. Ensure no values are null.");

                var errors = new StringBuilder();
                foreach (var kvp in d)
                {
                    if (RuleAPI.TryGetRule(kvp.Key, out var rule))
                    {
                        try
                        {
                            var userResult = rule.Deserialize(kvp.Value);
                            if (userResult.Err is string ruleErr)
                                errors.AppendLine($"Rule '{kvp.Key}' couldn't be parsed. {ruleErr}");
                        }
                        catch (Exception e)
                        {
                            errors.AppendLine($"Rule '{kvp.Key}' threw an exception while deserializing. {e}");
                        }
                    }
                }

                if (errors.Length > 0)
                    return Result.FromErr(errors.ToString());

                return Result.FromOk();
            }
            catch (Exception e)
            {
                return Result.FromErr("There was an error accessing the save file. " + e.Message);
            }
        }

        private static Result<string> GenerateJson()
        {
            // TODO Formatting
            StringBuilder err = new();
            StringBuilder ret = new("{");

            int count = 0;
            foreach (var item in RuleAPI.rules)
            {
                try
                {
                    string val = item.Value.Serialize();

                    if (err.Length > 0)
                        continue;

                    string comma = count++ < RuleAPI.rules.Count - 1 ? "," : "";
                    ret.Append($"\"{item.Key}\":{val}{comma}");
                }
                catch (Exception e)
                {
                    err.AppendLine($"Gamerule '{item.Key}' threw an exception while serializing. {e}");
                }
            }

            if (err.Length > 0)
                return Result<string>.FromErr(err.ToString());

            ret.Append('}');

            var retString = ret.ToString();

            var jsonResult = Json.Deserialize(retString);
            if (jsonResult is not Dictionary<string, object> d || d.Values.Any(v => v == null))
                return Result<string>.FromErr("Invalid JSON. Use jsonlint.com to validate and format your JSON. Ensure no values are null.\n" + retString);

            return Result.FromOk(retString);
        }

        private struct Tuple<T1, T2>
        {
            public readonly T1 Item1;
            public readonly T2 Item2;

            public Tuple(T1 item1, T2 item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }
    }
}
