using System;

namespace SGGames.Script.EditorExtensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheatCodeAttribute : Attribute
    {
        public string CommandName { get; }
        public string Description { get; }
        public object Instance { get; set; }
        
        public CheatCodeAttribute(string commandName, string description)
        {
            CommandName = commandName;
            Description = description;
        }
    }
}

