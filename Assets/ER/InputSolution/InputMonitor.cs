using ER;
using System.Collections.Generic;
using UnityEngine;

namespace ER.InputSolution
{
    /// <summary>
    /// 输入监听器; 需要根据项目需要修改 FixedUpdate 中固定监听的 按键选项
    /// 已为 InputRecorder 注册锚点
    /// </summary>
    public sealed class InputMonitor : MonoBehaviour
    {
        private InputRecorder recorder;

        private InputController controller;

        public InputController Controller
        {
            get => controller;
            set
            {
                controller = value;
                if (controller != null)
                {
                    controller.Recorder = recorder;
                }
            }
        }

        private void FixedUpdate()
        {
            HashSet<InputButtons> inputs = new HashSet<InputButtons>();

            /*使用时需要在此更新输入监听, 并将输入信息封装在 inputs 中*/
            /*
            int move = 5;

            if (InputManager.InputActions.Player.MoveLeft.IsPressed())
            {
                move = 4;
            }
            if (InputManager.InputActions.Player.MoveRight.IsPressed())
            {
                move = 6;
            }
            if (InputManager.InputActions.Player.MoveUp.IsPressed())
            {
                if (move == 4 || move == 6)
                {
                    move += 3;
                }
                else
                {
                    move = 8;
                }
            }
            if (InputManager.InputActions.Player.MoveDown.IsPressed())
            {
                if (move == 4 || move == 6)
                {
                    move -= 3;
                }
                else
                {
                    move = 2;
                }
            }
            switch (move)
            {
                case 1: inputs.Add(InputButtons.M1); break;
                case 2: inputs.Add(InputButtons.M2); break;
                case 3: inputs.Add(InputButtons.M3); break;
                case 4: inputs.Add(InputButtons.M4); break;
                case 6: inputs.Add(InputButtons.M6); break;
                case 7: inputs.Add(InputButtons.M7); break;
                case 8: inputs.Add(InputButtons.M8); break;
                case 9: inputs.Add(InputButtons.M9); break;
            }

            if (InputManager.InputActions.Player.Jump.IsPressed())
            {
                inputs.Add(InputButtons.JUMP);
            }
            if(InputManager.InputActions.Player.Dash.IsPressed())
            {
                inputs.Add(InputButtons.DASH);
            }
            if (InputManager.InputActions.Player.Attack.IsPressed())
            {
                inputs.Add(InputButtons.ATTACK);
            }

            recorder.UpdateInputFrame(inputs, Time.fixedDeltaTime);

            MonitorFrameInfo mfi = new MonitorFrameInfo()
            {
                move = move,
                inputs = inputs
            };

            controller?.UpdateMonitor(mfi);*/
        }

        [ContextMenu("保存输入记录")]
        private void SaveInputRecord()
        {
            recorder.SaveDebugLog();
        }

        private void Awake()
        {
            Anchor ac = AM.GetAnchor("InputRecorder");
            if (ac == null)
            {
                recorder = new InputRecorder();
            }
            else
            {
                recorder = ac.Owner as InputRecorder;
            }

            this.RegisterAnchor("InputMonitor");
        }

        private void OnDestroy()
        {
            AM.RemoveAnchor("InputMonitor");
        }
    }

    public struct MonitorFrameInfo
    {
        public int move;//移动编号, 5表示无移动
        public HashSet<InputButtons> inputs;//其他输入
    }
}