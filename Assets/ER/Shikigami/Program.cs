using ER.Shikigami;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        SpellParser parser = new SpellParser();
        Spell[] spells = parser.ParseSpellFromFile(@"D:\Desktop\Test\test.ykr");
        Console.WriteLine("符卡数量:" + spells.Length);
        foreach (Spell spell in spells)
        {
            spell.Print();
        }
    }
}