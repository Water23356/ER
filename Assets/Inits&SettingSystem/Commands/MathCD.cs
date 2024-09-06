using ER;
using System;
using UnityEngine;

public class MathCD : ICommandDictionaryModfier
{

    public void Modify(CommandDictionary dic)
    {
        dic.AddCommand("sumof #float float", d =>//求和
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1+v2);
        });

        dic.AddCommand("prodof #float float", d =>//求积
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 * v2);
        });

        dic.AddCommand("diffof #float float", d =>//求差
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 - v2);
        });

        dic.AddCommand("quotof #float float", d =>//求商
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 / v2);
        });

        dic.AddCommand("modof #int int", d =>
        {
            int v1 = d.NextInt();
            int v2 = d.NextInt();
            return new Data(v1 % v2, DataType.Int);
        });

        dic.AddCommand("lessthan #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 < v2, DataType.Bool);
        });

        dic.AddCommand("morethan #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 > v2, DataType.Bool);
        });
        dic.AddCommand("equals #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 == v2, DataType.Bool);
        });
        dic.AddCommand("notequals #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(v1 != v2, DataType.Bool);
        });

        dic.AddCommand("andof #bool bool", d =>
        {
            bool v1 = d.NextBool();
            bool v2 = d.NextBool();
            return new Data(v1 && v2, DataType.Bool);
        });

        dic.AddCommand("orof #bool bool", d =>
        {
            bool v1 = d.NextBool();
            bool v2 = d.NextBool();
            return new Data(v1 || v2, DataType.Bool);
        });

        dic.AddCommand("notof #bool", d =>
        {
            bool v1 = d.NextBool();
            return new Data(!v1, DataType.Bool);
        });

        dic.AddCommand("max #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(Math.Max(v1, v2));
        });

        dic.AddCommand("min #float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            return new Data(Math.Min(v1, v2));
        });

        dic.AddCommand("clamp #float float float", d =>
        {
            float v1 = d.NextFloat();
            float v2 = d.NextFloat();
            float v3 = d.NextFloat();
            return new Data(Math.Clamp(v1, v2,v3));
        });
    }
}