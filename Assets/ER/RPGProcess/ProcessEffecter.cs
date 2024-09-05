namespace ER.RPGProcess
{
    /// <summary>
    /// 进度效果器
    /// </summary>
    public class ProcessEffecter
    {
        //注册名
        private string registryName;
        //是否是一次性的(加载存档时会被忽略)
        private bool onetime;
        //是否是短暂性效果(仅在两个节点内生效)
        private bool shortEffect;
        //短暂生效起点
        private string shortEffectStart;
        //短暂生效终点
        private string shortEffectEnd;
        //lua效果代码
        private string effectLua;

    }
}