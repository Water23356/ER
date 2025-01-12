using ER.ForEditor;
using UnityEngine;

namespace Dev.AI
{
    /// <summary>
    /// 细胞机
    /// </summary>
    public class CellMachine : MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("输入细胞")]
        private InputCell[] inputs;

        [SerializeField]
        [DisplayLabel("输出细胞")]
        private OutputCell[] outputs;

        [SerializeField]
        [DisplayLabel("层单元个数")]
        private int[] layerCellCounts;

        /// <summary>
        /// 细胞层
        /// </summary>
        private CellLayer[] m_layers;

        public int inputsCount=>inputs.Length;
        public int[] layCountSetting => layerCellCounts;
        public CellLayer[] layers => m_layers;

        /// <summary>
        /// 更新细胞状态, 同时更新outputs状态
        /// </summary>
        public void UpdateState()
        {
            //获取新的输入细胞状态
            double[] inValues = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                inValues[i] = inputs[i].UpdateState();
            }

            //更新细胞层
            double[] lastInputs = layers[0].UpdateState(inValues);
            for (int i = 1; i < layers.Length; i++)
            {
                lastInputs = layers[i].UpdateState(inValues);
            }

            //更新输出细胞状态
            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i].UpdateState(lastInputs[i]);
            }
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            m_layers = new CellLayer[layerCellCounts.Length];
            for (int i = 0; i < layerCellCounts.Length; i++)
            {
                int inputCount = (i == 0) ? inputs.Length : layerCellCounts[i - 1];
                int outputCount = (i == layerCellCounts.Length - 1) ? outputs.Length : layerCellCounts[i + 1];
                layers[i] = new CellLayer(inputCount, outputCount);
            }
        }

        private void FixedUpdate()
        {
            //固定间隔更新
            UpdateState();
        }

        public CellMachineInfo GetInfo()
        {
            return new CellMachineInfo
            {
                layerCellCounts = layerCellCounts,
                layers = layers,
            };
        }

        public void SetWithInfo(CellMachineInfo info)
        {
            layerCellCounts = info.layerCellCounts;
            m_layers = info.layers;
        }

        /// <summary>
        /// 变异
        /// </summary>
        /// <param name="factor"></param>
        public void Mutate(CellControlAgent[][] factor)
        {
            for (int i = 0; i < factor.Length; i++)
            {
                for (int k = 0; k < factor[i].Length; k++)
                {
                    layers[i].cells[k].Mutate(factor[i][k]);
                }
            }
        }

        public void Reset()
        {
            for(int i=0;i<layers.Length;i++)
            {
                layers[i].Reset();
            }
        }
    }

    public struct CellMachineInfo
    {
        public int[] layerCellCounts;
        public CellLayer[] layers;
    }
}