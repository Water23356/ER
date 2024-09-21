using Dev;
using UnityEngine;

public class TestGR:MonoBehaviour
{
    public GameResource gameResource;

    [ContextMenu("初始化所有加载器")]
    public void FindAll()
    {
        gameResource.FixedAllResourceLoader();
    }
}