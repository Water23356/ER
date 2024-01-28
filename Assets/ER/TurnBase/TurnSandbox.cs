using System;
using System.Collections.Generic;

namespace ER.TurnBase
{
    /// <summary>
    /// 回合制沙盒
    /// </summary>
    public class TurnSandbox
    {
        /// <summary>
        /// 玩家槽位
        /// </summary>
        protected TurnPlayer[] players;
        /// <summary>
        /// 玩家槽位是否为AI
        /// </summary>
        protected bool[] is_ai;
        protected AI[] ai;
        /// <summary>
        /// 回合数
        /// </summary>
        protected int rounds;
        /// <summary>
        /// 当前回合所属者的索引
        /// </summary>
        protected int index;
        /// <summary>
        /// 拓展属性接口
        /// </summary>
        protected Dictionary<string,object> marks = new Dictionary<string,object>();

        public int PlayerCount { get=>players.Length;}
        /// <summary>
        /// 当一轮开始时触发的事件
        /// </summary>
        public event Action OnRoundStartEvent;
        /// <summary>
        /// 当一轮结束时触发的事件
        /// </summary>
        public event Action OnRoundEndEvent;


        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="start_index">游戏开始时第一个回合的玩家索引</param>
        public void GameStart(int start_index = 0)
        {
            GameInit();
            index = start_index;
            players[index].RoundStart();
        }
        /// <summary>
        /// 游戏初始化需要做的事情, 在游戏正式开始时, 第一个回合前 需要处理的逻辑(子类请重写该方法)
        /// </summary>
        public virtual void GameInit()
        {

        }
        /// <summary>
        /// 进入下一个玩家的回合, 不要调用这个函数, 如果你想强制跳转到下一个玩家回合, 请调用
        /// <code>BreakPlayerRound()</code>
        /// </summary>
        public void NextPlayerRound()
        {
            index++;
            if(index>=PlayerCount)
            {
                index = 0;
                rounds++;
            }
            if(IsAI(index))//如果是AI槽位, 则AI代替操控
            {
                ai[index].RoundStart();
            }
            else
            {
                players[index].RoundStart();
            }
        }
        /// <summary>
        /// 强制结束当前玩家的回合, 并跳转到下一个玩家的回合
        /// </summary>
        public void BreakPlayerRound()
        {
            players[index].RoundEnd();
            NextPlayerRound();
        }
        public void SetPlayerCount(int count)
        {
            players = new TurnPlayer[count];
            is_ai = new bool[count];
            ai=new AI[count];
        }
        /// <summary>
        /// 获取指定槽位的玩家
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TurnPlayer GetPlayer(int index)
        {
            return players[index];
        }
        
        /// <summary>
        /// 设置指定槽位的玩家
        /// </summary>
        /// <param name="index"></param>
        /// <param name="player"></param>
        public void SetPlayer(int index, TurnPlayer player)
        {
            players[index]=player;
        }
        /// <summary>
        /// 判断指定槽位是否是AI
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsAI(int index)
        {
            return is_ai[index];
        }
        /// <summary>
        /// 设定指定槽位是否为AI操控
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        /// <param name="operator">如果是AI操控, 需要传入一个AI对象</param>
        public void SetAI(int index,bool state,AI @operator)
        {
            is_ai[index] = state;
            if(state)
            {
                ai[index] = @operator;
                @operator.sandbox = this;
                @operator.Index = index;
            }
        }

    }
}