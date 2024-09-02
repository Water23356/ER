using ER.Dynamic;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Security.Cryptography;

namespace ER
{
    /// <summary>
    /// 指令集
    /// </summary>
    public class CommandRunes : WithDynamicProperties, IEnumerable<Command>
    {
        private List<Command> commands = new List<Command>();

        private int index = 0;

        public void AddCommand(params Command[] command)
        {
            commands.AddRange(command);
        }

        public void RemoveCommand(Command command)
        {
            commands.Remove(command);
        }

        public IEnumerator<Command> GetEnumerator()
        {
            foreach (var cmd in commands)
                yield return cmd;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var cmd in commands)
                yield return cmd;
        }

        public bool isEnd
        {
            get => index >= commands.Count;
        }

        public bool isEmpty
        {
            get
            {
                return commands.Count == 0;
            }
        }

        public void MoveTo(int index)
        {
            this.index = index;
        }

        public void Move(int step)
        {
            index += step;
            index = Math.Max(index, 0);
        }

        public Command Next()
        {
            if (index < 0 || index >= commands.Count)
                return null;
            return commands[index++];
        }
        public Command First()
        {
            if (index < 0 || index >= commands.Count)
                return null;
            return commands[0];
        }

        public CommandRunes Copy()
        {
            CommandRunes copy = new CommandRunes();
            foreach(var cmd in commands)
            {
                copy.AddCommand(cmd.Copy());
            }
            return copy;
        }
    }
}