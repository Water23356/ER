using ER.ResourceManager;
using UnityEngine;
using UnityEngine.U2D;

public class TestGR:MonoBehaviour
{
    public GameResource gameResource;
    public SpriteAtlas atlas;

    [ContextMenu("初始化所有加载器")]
    public void FindAll()
    {
        gameResource.FixedAllResourceLoader();
    }
}