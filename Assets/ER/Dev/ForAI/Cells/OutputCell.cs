using UnityEngine;

namespace Dev.AI
{
    /// <summary>
    /// 输出单元
    /// </summary>
    public abstract class OutputCell:MonoBehaviour
    {
        public abstract void UpdateState(double input);
    }
}