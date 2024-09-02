using JetBrains.Annotations;

namespace ER.Extender
{
    /// <summary>
    /// 可携带装饰器
    /// </summary>
    public interface IDecoratorOwner
    {
        public Decorator DData { get; }

    }
}