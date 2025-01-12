using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dev.AI
{
    [Serializable]
    public class Cell
    {
        public double[] wi;
        public double wc;
        public double b;
        public double c;

        public Cell() { }
            

        public Cell(int inputCount)
        {
            Init(inputCount);
        }

        public void Init(int inputCount)
        {
            wi = new double[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                wi[i] = Random.value;
            }
            wc = Random.value;
        }

        public void UpdateState(double[] inputs, Func<double, double> actFunc)
        {
            c = wc * c + b;
            int minLen = Math.Min(inputs.Length, wi.Length);
            for (int i = 0; i < inputs.Length; i++)
            {
                c += inputs[i] * wi[i];
            }
            c = actFunc(c);
        }

        public void Mutate(CellControlAgent factor)
        {
            for(int i=0;i< wi.Length; i++)
            {
                factor.wi[i].Muate(ref wi[i]);
            }
            factor.wc.Muate(ref wc);
            factor.b.Muate(ref b);
        }
        public Cell Copy()
        {
            var wiCopy = new double[wi.Length];
            for(int k=0;k<wi.Length;k++)
            {
                wiCopy[k] = wi[k];
            }
            return new Cell()
            {
                wi = wiCopy,
                wc = wc,
                b = b,
                c = c,
            };
        }
    }

    public class CellControlAgent
    {
        public ControlAgent[] wi;
        public ControlAgent wc;
        public ControlAgent b;

        public CellControlAgent(int inputCount)
        {
            wi = new ControlAgent[inputCount];
            for(int i=0;i<wi.Length;i++)
            {
                wi[i].dir = GetRandomValue();
            }
            wc.dir = GetRandomValue();
            b.dir = GetRandomValue();
        }

        public static double GetRandomValue()
        {
            return Random.value - 0.5;
        }
    }

    public struct ControlAgent
    {
        public static double enhanceAlpha = 1.1;//增长学习率
        public static double weakenAlpha = 0.9;//削弱学习率
        public static double rate = 0.5;//变异概率

        /// <summary>
        /// 变异方向
        /// </summary>
        public double dir;


        /// <summary>
        /// 增强
        /// </summary>
        public void Enhance()
        {
            dir *= enhanceAlpha;
        }

        /// <summary>
        /// 削弱
        /// </summary>
        public void Weaken()
        {
            dir *= weakenAlpha;
        }
        /// <summary>
        /// 获取变异值
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public void Muate(ref double origin)
        {
            if (UnityEngine.Random.value < rate)
            {
                origin += dir * UnityEngine.Random.value;
            }
        }
    }
}