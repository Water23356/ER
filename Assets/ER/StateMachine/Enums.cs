namespace ER.StateMachine
{
    public class Enums
    {
        /// <summary>
        /// 过渡型状态枚举
        /// </summary>
        public enum TransitionEnum
        {
            /// <summary>
            /// 无效的
            /// </summary>
            Disable,
            /// <summary>
            /// 进入过渡
            /// </summary>
            Entering,
            /// <summary>
            /// 有效的
            /// </summary>
            Enable,
            /// <summary>
            /// 离开过渡状态
            /// </summary>
            Exiting
        }

        /// <summary>
        /// 开关型状态枚举
        /// </summary>
        public enum SwithEnum
        {
            /// <summary>
            /// 有效
            /// </summary>
            Enable,
            /// <summary>
            /// 无效
            /// </summary>
            Disable
        }
    }
}