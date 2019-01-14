using BlyadleField.ConsoleSystem;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BlyadleField.CommandSystem
{
    internal static class CommandHandler
    {
        public static List<Command> Commands = new List<Command>();
        public static void Worker()
        {
            while (BlyadleField.IsAttached)
            {
                var fullCommand = Console.ReadLine();
                var commandArray = fullCommand.ToLower().Split(' ');
                var command = commandArray[0];
                var param = commandArray.Length > 1 ? commandArray[1] : "";
                var value = commandArray.Length > 2 ? commandArray[2] : "";
                HandleCommand(command, param, value);
                Console.WriteCommandLine();
            }
        }

        public static void Setup()
        {
            Console.Title = Utils.RandomString(new System.Random().Next(10, 32));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteWatermark();

            Commands.Add(new Command("esp", "Wallhax"));
            Commands.Add(new Command("crosshair", "Crosshair"));
            Commands.Add(new Command("bullet", "Bullet modifications"));
            Commands.Add(new Command("weapon", "bla"));

            AddParameter("esp", "active", "1", "Wether walls are on or not.");
            AddParameter("esp", "box", "1", "Show boxes");
            AddParameter("esp", "bone", "1", "Draw bones");
            AddParameter("esp", "health", "1", "Health ESP");
            AddParameter("esp", "radar", "1", "Radar");
            AddParameter("esp", "vehicle", "1", "Vehicle ESP");
            AddParameter("esp", "distance", "1", "Show Distance");
            AddParameter("esp", "name", "1", "Show Names");
            AddParameter("esp", "trace", "1", "Tracelines to enemies");
            AddParameter("esp", "glow", "1", "Fancy glow");

            AddParameter("crosshair", "active", "1", "Wether crosshair is active or not.");

            AddParameter("bullet", "active", "0", "Bla bla bla");
            AddParameter("bullet", "damage", "1", "Damage Multiplier");
            AddParameter("bullet", "speed", "1", "Speed of the bullets");
            AddParameter("bullet", "gravity", "1", "Gravity of the bullets");

            AddParameter("weapon", "norecoil", "1", "No Recoil");
        }

        private static void AddParameter(string command, string parameter, string defaultValue, string desc = "This is a basic parameter")
        {
            GetCommand(command).Parameters.Add(new CommandParameter(parameter, new CommandParameterValue(defaultValue), desc));
        }

        private static void HandleCommand(string command, string parameter, string value)
        {
            switch (command)
            {
                case "load":
                    Load();
                    break;
                case "save":
                    Save();
                    break;
                case "help":
                    DisplayHelp();
                    break;
                default:
                    var cmd = GetCommand(command);
                    if (!cmd)
                    {
                        Console.WriteSuccess($"Could not find command '{command}'.", false);
                        return;
                    }
                    if (parameter == "")
                    {
                        DisplayParameters(cmd);
                        return;
                    }
                    var param = GetParameter(command, parameter);
                    if (!param)
                    {
                        Console.WriteSuccess($"Could not find parameter '{parameter}' in command '{command}'.", false);
                        return;
                    }
                    if (value == "")
                    {
                        Console.WriteNotification($"  - {cmd.Name} {param.Name} ({param.Description})\n    Current value of '{command} {parameter}' is {GetParameter(command, parameter).Value}\n");
                        return;
                    }
                    if (!param.IsFunction)
                    {
                        param.Value = new CommandParameterValue(value);
                        if (param.Value.ToFloat() < 0.0f)
                        {
                            Console.WriteSuccess($"Value has to be convertable to a digit", false);
                            return;
                        }
                        Console.WriteNotification($"Set value of '{command} {parameter}' to '{value}'.");
                        return;
                    }
                    break;
            }
        }

        public static void Save()
        {
            foreach(var cmd in Commands)
            {
                foreach(var param in cmd.Parameters)
                {
                    WriteValue(cmd.Name.ToUpper(), param.Name, param.Value.Value);
                }
            }
            Console.WriteNotification("  Saved Settings!");
        }

        public static void Load()
        {
            foreach (var cmd in Commands)
            {
                foreach (var param in cmd.Parameters)
                {
                    param.Value.Value = ParseInteger(ReadValue(cmd.Name.ToUpper(), param.Name), 1).ToString();
                }
            }
            Console.WriteNotification("  Loaded Settings!");
        }

        private static void DisplayParameters(Command cmd)
        {
            cmd.Parameters.ForEach(delegate (CommandParameter param)
            {
                Console.WriteNotification($"  - {cmd.Name} {param.Name} ({param.Description})");
            });
        }

        private static void DisplayHelp()
        {
            Commands.ForEach(delegate (Command pCmd)
            {
                Console.WriteNotification($"  - {pCmd.Name} ({pCmd.Description})");
            });
        }

        private static Command GetCommand(string command)
        {
            return Commands.FirstOrDefault(com => com.Name == command);

        }
        public static CommandParameter GetParameter(string command, string parameter)
        {
            return GetCommand(command).Parameters.FirstOrDefault(param => param.Name == parameter);
        }

        #region ReadWrite
        public static void WriteValue(string section, string key, string value, string File = ".\\settings.ini")
        {
            WritePrivateProfileString(section, key, value, File);
        }

        public static string ReadValue(string section, string key, string File = ".\\settings.ini")
        {
            var temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, File);

            return temp.ToString();
        }
        #endregion
        #region Parsing
        public static bool ParseBoolean(string input, bool defaultVal = false)
        {
            if (string.IsNullOrEmpty(input))
                return defaultVal;

            bool output;

            if (!bool.TryParse(input, out output))
                return defaultVal;

            return output;
        }

        public static int ParseInteger(string input, int defaultVal = 0)
        {
            if (string.IsNullOrEmpty(input))
                return defaultVal;

            int output;

            if (!int.TryParse(input, out output))
                return defaultVal;

            return output;
        }

        public static float ParseFloat(string input, float defaultVal = 0.0f)
        {
            if (string.IsNullOrEmpty(input))
                return defaultVal;

            float output;

            if (!float.TryParse(input, out output))
                return defaultVal;

            return output;
        }
        #endregion
        #region Native
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion
    }
}
