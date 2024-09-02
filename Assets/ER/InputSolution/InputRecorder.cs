using ER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ER.InputSolution
{
    /// <summary>
    /// 输入记录器
    /// </summary>
    public class InputRecorder
    {
        //private Dictionary<InputButtons,InputRecordDuct> ducts = new Dictionary<InputButtons, InputRecordDuct>();//输入管道

        public static int MaxStoreSize = 25;

        private List<InputFrame> inputs = new List<InputFrame>();

        private StreamWriter writer;//debug使用
        private string DebugPath = @"D:\Desktop\Debug\Unity-InputHistroy";

        public InputFrame GetLastInput()
        {
            if (inputs.Count == 0) return InputFrame.Error;
            return inputs[inputs.Count - 1];
        }

        public void UpdateInputFrame(HashSet<InputButtons> origin, float keepTime)
        {
            InputFrame last = GetLastInput();
            if (!last.IsError() && last.keys.SetEquals(origin))//如果当前帧输入与上一帧输入相同,在仅增加持续时间
            {
                inputs[inputs.Count - 1] = new InputFrame()
                {
                    keys = last.keys,
                    keepTime = last.keepTime + keepTime
                };
                //Debug.Log("last: " + inputs[inputs.Count - 1].keepTime + "\n" +
                //  "LastGet:" + GetLastInput().keepTime);
            }
            else//否则记录新的输入状态
            {
                DebugLogs();

                //Debug.Log("有新的输入状态");
                InputFrame ifm = new InputFrame();

                ifm.keys = origin;
                ifm.keepTime = 0.01f;
                inputs.Add(ifm);
                if (inputs.Count > MaxStoreSize)
                {
                    inputs.RemoveAt(0);
                }
                
            }
        }

        private void DebugLogs()
        {
            InputFrame ifm = inputs[inputs.Count - 1];
            HashSet<InputButtons> origin = ifm.keys;
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{DateTime.UtcNow.ToString("u")}]: [{ifm.keepTime}]: ");
            foreach (var bt in origin)
            {
                sb.Append($"<{bt.ToString()}>");
            }
            writer.WriteLine(sb);
        }

        public void SaveDebugLog()
        {
            writer.Close();
        }

        public bool IsMatched(InputMatchingString matchingString)
        {
            return matchingString.IsMatched(inputs);
        }

        public InputRecorder()
        {
            DebugPath += $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.txt";
            Debug.Log(DebugPath);
            if (!File.Exists(DebugPath))
            {
                File.Create(DebugPath).Close();
            }
            writer = new StreamWriter(DebugPath);
            this.RegisterAnchor("InputRecorder");

            inputs.Add(new InputFrame()
            {
                keys = new HashSet<InputButtons>(),
                keepTime = 0.01f
            });
        }

        ~InputRecorder()
        {
            writer.Close();
            AM.RemoveAnchor("InputRecorder");
        }
    }
}