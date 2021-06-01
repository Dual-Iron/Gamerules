using BepInEx;
using BepInEx.Logging;
using RWCustom;
using System;
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
                    return ParseJson("{}");
                }
                return ParseJson(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                return Result.FromErr("There was an error accessing the save file. " + e.Message);
            }
        }

        private static Result ParseJson(string json)
        {
            Result invalid = Result.FromErr("Invalid JSON. Use https://jsonlint.com/ to check your JSON.");
            StringBuilder errors = new();

            int index = 0;

            ConsumeWhitespace();

            if (!Consume('{'))
                return invalid;

            ConsumeWhitespace();

            while (Consume('"'))
            {
                int nameStart = index;

                while (!Consume('"'))
                    if (index < json.Length)
                        index++;
                    else return invalid;

                string name = json.Substring(nameStart, index - nameStart);

                ConsumeWhitespace();

                if (!Consume(':'))
                    return invalid;

                ConsumeWhitespace();

                int valueStart = index;
                int stack = 0;

                while (true)
                {
                    if (index >= json.Length)
                        return invalid;

                    char cur = Advance();

                    if (cur == ',' && stack != 0 || cur == '}' && stack == 0)
                        break;
                    else if (cur == '{')
                        stack++;
                    else if (cur == '}')
                        stack--;
                }

                string value = json.Substring(valueStart, index - valueStart);

                if (RuleAPI.rules.TryGetValue(name, out var rule))
                {
                    try
                    {
                        if (rule.Deserialize(value.Trim()).Err is string s)
                            errors.AppendLine($"Gamerule '{name}' couldn't be parsed. {s}");
                    }
                    catch (Exception e)
                    {
                        errors.AppendLine($"Gamerule '{name}' threw an exception while deserializing. {e}");
                    }
                }
                else errors.AppendLine($"Gamerule '{name}' does not exist.");

                ConsumeWhitespace();
            }

            ConsumeWhitespace();

            if (!Consume('}'))
                errors.AppendLine(invalid.Err);

            if (errors.Length > 0)
                return Result.FromErr(errors.ToString());

            return Result.FromOk();

            char Advance() => index < json.Length ? json[index++] : '\0';
            bool Consume(char c) => Advance() == c;

            void ConsumeWhitespace()
            {
                while (char.IsWhiteSpace(Advance()))
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
