using BepInEx;
using BepInEx.Logging;
using RWCustom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gamerules
{
    [BepInPlugin("com.github.dual.gamerules", "Gamerules", "1.0.0")]
    internal sealed class GamerulesPlugin : BaseUnityPlugin
    {
        public static event Action? OnUnload;

        public void OnEnable()
        {
            On.RainWorldGame.ctor += RainWorldGame_ctor;
        }

        public void OnDisable()
        {
            OnUnload?.Invoke();
            OnUnload = null;

            RuleAPI.rules.Clear();
        }

        private void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
        {
            orig(self, manager);
            Logger.LogMessage("Loading gamerules");
            if (Read(self, self.GetStorySession.saveStateNumber).Err is string s)
            {
                Logger.LogError(s);
            }
        }

        private static string Combine(string first, params string[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                first = Path.Combine(first, other[i]);
            }
            return first;
        }

        private static Result Read(RainWorldGame game, int saveNum)
        {
            string path = Combine(Custom.RootFolderDirectory(), "UserData", $"Gamerules-Save_Slot_{saveNum + 1}-{game.session.characterStats.name}_Slugcat.json");

            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "{}");
                    return Result.FromOk();
                }

                var jsonResult = ParseJson(File.ReadAllText(path));
                if (jsonResult.Err is string jsonErr)
                    return Result.FromErr(jsonErr);

                var errors = new StringBuilder();
                foreach (var kvp in jsonResult.Ok!)
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

        private static Result<Dictionary<string, string>> ParseJson(string json)
        {
            var invalid = Result<Dictionary<string, string>>.FromErr("Invalid JSON. Use jsonlint.com to validate your JSON.");

            int index = 0;

            ConsumeWhitespace();

            if (json[index++] != '{')
                return invalid;

            ConsumeWhitespace();

            var ret = new Dictionary<string, string>();

            while (json[index] == '"')
            {
                index++;

                int nameStart = index;

                while (json[index] != '"')
                    if (index < json.Length)
                        index++;
                    else return invalid;

                string name = json.Substring(nameStart, index - nameStart);

                index++;

                ConsumeWhitespace();

                if (json[index++] != ':')
                    return invalid;

                int valueStart = index;
                int stack = 0;

                while (true)
                {
                    index++;

                    if (index >= json.Length)
                        return invalid;

                    char cur = json[index];

                    if (cur == ',' && stack != 0 || cur == '}' && stack == 0)
                        break;
                    else if (cur == '{')
                        stack++;
                    else if (cur == '}')
                        stack--;
                }

                string value = json.Substring(valueStart, index - valueStart);

                ret[name] = value.Trim();

                ConsumeWhitespace();
            }

            if (json[index++] != '}')
                return invalid;

            ConsumeWhitespace();

            if (index < json.Length)
                return invalid;

            return Result.FromOk(ret);

            void ConsumeWhitespace()
            {
                while (index < json.Length && char.IsWhiteSpace(json[index]))
                    index++;
            }
        }

        private static Tuple<string, Result> GenerateJson()
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
                    string comma = count < RuleAPI.rules.Count - 1 ? "," : "";
                    ret.Append($"\"{item.Key}\":{val}{comma}");
                }
                catch (Exception e)
                {
                    err.AppendLine($"Gamerule '{item.Key}' threw an exception while serializing. {e}");
                }
            }

            ret.Append('}');

            return new(ret.ToString(), err.Length > 0 ? Result.FromErr(err.ToString()) : Result.FromOk());
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
