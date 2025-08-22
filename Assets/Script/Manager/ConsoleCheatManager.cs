using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SGGames.Scripts.EditorExtensions;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    /// <summary>
    /// Main class for cheat code used in game.
    /// To use this put the attribute [CheatCode] on the method means to be used for cheat code
    /// </summary>
    public class ConsoleCheatManager : MonoBehaviour
{
    private delegate void CheatCommandDelegate(object[] args);
    private static Dictionary<string, (CheatCommandDelegate action, string description, ParameterInfo[] parameters, object instance)> m_commands;

    private void Awake()
    {
        m_commands = new Dictionary<string, (CheatCommandDelegate, string, ParameterInfo[], object)>();
    }

    /// <summary>
    /// Register this script to gather all its cheat code method inside
    /// </summary>
    /// <param name="instance"></param>
    public static void RegisterCommands(object instance)
    {
        var type = instance.GetType();
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                         .Where(m => m.GetCustomAttribute<CheatCodeAttribute>() != null);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<CheatCodeAttribute>();
            var parameters = method.GetParameters();

            // Set the instance in the attribute
            attribute.Instance = instance;

            // Create delegate for the method
            CheatCommandDelegate action = args => method.Invoke(attribute.Instance, args);

            // Cache the command
            m_commands[attribute.CommandName.ToLower()] = (action, attribute.Description, parameters, attribute.Instance);
        }
    }

    [ContextMenu("Test Cheat")]
    private void TestCommand()
    {
        ExecuteCommand("test 5");
    }

    public void ExecuteCommand(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        
        var parts = input.Trim().Split(' ');
        if (parts.Length == 0) return;

        string command = parts[0].ToLower();
        string[] args = parts.Skip(1).ToArray();

        if (!m_commands.TryGetValue(command, out var cmd))
        {
            Debug.LogError($"Command '{command}' not found.");
            return;
        }

        try
        {
            if (cmd.parameters.Length != args.Length)
            {
                Debug.LogError($"Invalid number of arguments for '{command}'. Expected {cmd.parameters.Length}, got {args.Length}.");
                return;
            }

            object[] convertedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                convertedArgs[i] = ConvertArgument(args[i], cmd.parameters[i].ParameterType);
            }

            cmd.action(convertedArgs);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing command '{command}': {ex.Message}");
        }
    }

    private object ConvertArgument(string arg, Type targetType)
    {
        try
        {
            if (targetType == typeof(int)) return int.Parse(arg);
            if (targetType == typeof(float)) return float.Parse(arg);
            if (targetType == typeof(string)) return arg;
            if (targetType == typeof(bool)) return bool.Parse(arg);
            throw new ArgumentException($"Unsupported parameter type: {targetType}");
        }
        catch
        {
            throw new ArgumentException($"Failed to convert '{arg}' to type {targetType}");
        }
    }
}
}
