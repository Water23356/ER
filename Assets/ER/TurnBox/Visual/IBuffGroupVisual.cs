namespace ER.TurnBox
{
    public interface IBuffGroupVisual
    {
        /// <summary>
        /// 当有新buff被添加进组时播放的动画(叠加不触发)
        /// </summary>
        /// <param name="newBuff"></param>
        /// <returns></returns>
        public IBuffVisual PlayAddBuff(Buff newBuff);

        /// <summary>
        /// 当有buff被移除时播放的动画
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public void PlayRemoveBuff(Buff buff);
        /// <summary>
        /// 设置源对象并同步显示
        /// </summary>
        /// <param name="origin"></param>
        public void UpdateVisual(BuffGroup origin);
    }
}