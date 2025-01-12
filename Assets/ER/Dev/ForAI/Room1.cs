using ER.ForEditor;
using UnityEngine;

namespace Dev.AI
{
    public class Room1 : MonoBehaviour
    {
        public Foam foam;
        public CellMachine cellMachine;

        [DisplayLabel("默认位置")]
        public Vector2 defaultPos = Vector2.zero;

        public DNA dna;

        private bool inited = false;
        public float liveTime = 0;
        public int eatCount = 0;
        public float score = 0;

        private void Start()
        {
            //初始化变异设置
            var settings = cellMachine.layCountSetting;
            dna = new DNA();
            dna.cellStruct = new CellControlAgent[settings.Length][];
            for (int i = 0; i < settings.Length; i++)
            {
                dna.cellStruct[i] = new CellControlAgent[settings[i]];
                for (int k = 0; k < dna.cellStruct[i].Length; k++)
                {
                    if (i == 0)
                    {
                        dna.cellStruct[i][k] = new CellControlAgent(cellMachine.inputsCount);
                    }
                    else
                    {
                        dna.cellStruct[i][k] = new CellControlAgent(dna.cellStruct[i - 1].Length);
                    }
                }
            }
            dna.liveTime = 0;
            dna.eatCount = 0;
            dna.score = 0;
        }

        private void Compare(bool better)
        {
            for (int i = 0; i < cellMachine.layers.Length; i++)
            {
                for (int k = 0; k < cellMachine.layers[i].size; k++)
                {
                    var now = cellMachine.layers[i].cells[k];
                    var origin = dna.cellMachineInfo.layers[i].cells[k];
                    for (int j = 0; j < now.wi.Length; j++)
                    {
                        if (now.wi[j] - origin.wi[j] > 0.001)
                        {
                            if (better)
                                dna.cellStruct[i][k].wi[j].Enhance();
                            else
                                dna.cellStruct[i][k].wi[j].Weaken();
                        }
                    }
                    if (now.b - origin.b > 0.001)
                    {
                        if (better)
                            dna.cellStruct[i][k].b.Enhance();
                        else
                            dna.cellStruct[i][k].b.Weaken();
                    }
                    if (now.b - origin.b > 0.001)
                    {
                        if (better)
                            dna.cellStruct[i][k].b.Enhance();
                        else
                            dna.cellStruct[i][k].b.Weaken();
                    }
                }
            }
        }

        private void Update()
        {
            if (!foam.gameObject.activeSelf)
            {
                if (!inited)
                {
                    dna.cellMachineInfo = cellMachine.GetInfo();
                    inited = true;
                }
                else
                {
                    score = liveTime * 0.1f + eatCount;
                    float v = score - dna.score;
                    var info = cellMachine.GetInfo();
                    for (int i = 0; i < info.layers.Length; i++)
                    {
                        for (int k = 0; k < info.layers[i].cells.Length; k++)
                        {
                            if (dna.cellMachineInfo.layers[i].cells[k] != info.layers[i].cells[k])
                            {
                            }
                        }
                    }
                }

                liveTime = 0;
                eatCount = 0;
                score = 0;
                foam.transform.position = defaultPos;
                foam.ResetState();
                cellMachine.Reset();
                cellMachine.Mutate(dna.cellStruct);
                foam.gameObject.SetActive(true);
            }
        }
    }
}