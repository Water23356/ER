using System;

namespace ER
{
    public class ParseException : Exception
    {
        public ParseException() : base("指令解析错误")
        {
            Console.WriteLine("指令解析错误");
        }

        public ParseException(string message) : base(message)
        {
            Console.WriteLine("指令解析错误");
        }
    }
}