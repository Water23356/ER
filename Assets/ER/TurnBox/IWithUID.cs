namespace ER.TurnBox
{
    /// <summary>
    /// 携带UID锚点
    /// </summary>
    public interface IWithUID
    {
        public VirtualAnchor UID { get; set;  }
        public void CreateUID(string uid)
        {
            UID = this.RegisterAnchor(uid);
        }
    }
}