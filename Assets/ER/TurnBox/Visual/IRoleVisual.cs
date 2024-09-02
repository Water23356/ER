namespace ER.TurnBox
{
    public interface IRoleVisual
    {
        /// <summary>
        /// 设置源对象并同步显示
        /// </summary>
        /// <param name="origin"></param>
        public void UpdateVisual(Role origin);
    }
}