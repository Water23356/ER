namespace ER.Save
{
    /// <summary>
    /// 存档还原类
    /// </summary>
    public interface SaveLoader
    {
        /// <summary>
        /// 根据存档数据还原对象
        /// </summary>
        /// <param name="data"></param>
        public void Restore(SaveData data);
    }
}