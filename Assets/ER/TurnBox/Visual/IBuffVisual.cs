namespace ER.TurnBox
{
    public interface IBuffVisual
    {
        /// <summary>
        /// 播放buff加入时的动画
        /// </summary>
        public void PlayApply();

        /// <summary>
        /// 播放覆盖动画
        /// </summary>
        public void PlayOverride();

        /// <summary>
        /// 播放buff被移除时的动画
        /// </summary>
        public void PlayRemove();

        /// <summary>
        /// 播放buff触发时的动画
        /// </summary>
        public void PlayTrigger(TriggerTime triggerTime);

        /// <summary>
        /// 设置源对象并同步显示
        /// </summary>
        /// <param name="origin"></param>
        public void UpdateVisual(Buff origin);
    }
}