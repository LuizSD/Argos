using System;
using System.Collections.Generic;

namespace ArduinoSignalRClient.Models
{
    public class Command
    {
        private readonly string _command;

        public string Status
        {
            get
            {
                if (_command.EndsWith("1"))
                    return "Active";

                return _command.EndsWith("0") ? "Inactive" : "Unknown";
            }
        }

        public string Name
        {
            get { return _command.Substring(0, 2).ToLowerInvariant(); }
        }

        public Command(string command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (command.Length != 3)
                throw new Exception("Invalid command length.");

            _command = command;
        }

        public bool IsValid()
        {
            #region valid commands

            var validCommands = new List<string>
            {
                "l11",
                "l10",

                "l21",
                "l20",

                "l31",
                "l30",

                "p11",
                "p10",

                "p21",
                "p20",

                "p31",
                "p30",

                "j11",
                "j10",

                "j21",
                "j20",

                "j31",
                "j30",
            };

            #endregion

            return validCommands.Contains(_command);
        }
    }
}