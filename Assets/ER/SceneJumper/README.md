# 场景跳转器

这个模块负责了场景过渡服务, 定义了一套模板, 以及一个现成的场景跳转方案; 

通常情况可以直接使用 SceneJumper.ToScene() 完成大多数的场景跳转需求, 对于较为复杂的跳转需求可以结合 SceneTransition 以及其他资源加载模块来实现

列表:

1. 类的描述
    1. [SceneTransition - 场景过渡](#section_SceneTransition)
    2. [SceneInitializer - 场景初始化器](#section_SceneInitializer)
    3. [SceneJumper - 场景跳转器](#section_SceneJumper)
        1. SceneJumpInfo
        2. MetaSceneJump
2. [示例](#section_sample)
    


## 类型描述
<a id="section_SceneTransition"></a>
### SceneTransition - 场景过渡


标签: [组件] [面板] [抽象类]

功能: 负责提供场景过渡所需的基本功能, 它需要有一个驱动源才能完成整个场景过渡的完整播放;

描述: SceneTransition 定义了一个场景过渡器的模板, 你可以通过重写该抽象类自定义场景过渡的具体行为, 它包含以下行为:

- PlayStartAsync() : 异步播放过渡动画的开始部分, 由控制源调用
- PlayEndAsync() : 异步播放过渡动画的结束部分, 由控制源调用

此外它包含以下属性:

- progress : 场景加载进度, 由控制源设置

---

<a id="section_SceneInitializer"></a>
### SceneInitializer - 场景初始化器

标签: [组件] [抽象类]

功能: 负责新场景的对象初始化

使用规则: 为了 SceneInitializer 能够正常运作, 你需要保证被加载的场景包含一个 SceneInitializer 对象, 且所有需要被控制初始化顺序的对象默认处于非活动状态

描述: SceneInitializer 定义了一个场景初始化的模板, 你可以通过重写该抽象类来定义不同场景的初始化行为, 它包含以下行为:

- InitContentAsync() : 一个异步的初始化过程, 由控制源调用

此外它包含以下属性:

- autoDestroy : (默认为 ture) 
    - true: 在完成初始化后自动销毁自身; 
    - flase: 在完成初始化后无行为; 

---

<a id="section_SceneJumper"></a>
### SceneJumper - 场景跳转器

标签: [组件] [单例]

功能: 提供一个统一的场景跳转功能模块, 根据配置信息完成场景跳转所需的步骤

ToScene(SceneJumpInfo): 根据配置信息(SceneJumpInfo)执行场景跳转步骤, 这包含以下步骤:

        1. 播放淡入过渡动画
        2. 资源依赖检测 和 资源加载
        3. 场景加载
        4. 场景初始化
        5. 播放淡出过渡动画

配置属性:
- Transition: 使用的场景过渡器对象, 这决定过渡时表现的具体视觉效果

#### SceneJumpInfo - 跳转信息

标签: [结构体] [中间容器]

描述: 这个结构体用于包装场景过渡所需的信息:

- loadMode: 场景加载模式(覆盖,单体)
- aimScene: 跳转的目标场景的 注册名
- depends[]: 本次加载需要额外加载的资源包(组)
- statusText[]: 加载依赖资源时显示的名称(组) 
    >元素个数应与 depends 相同

#### MetaSceneJump - 跳转信息(可读写)

标签: [序列化容器]

描述: SceneJumpInfo 的序列化容器

<a id="section_sample"></a>
## 示例
一般示例:
``` C#
class SampleTransition:SceneTransition
{
    private float width;
    private Action callback_enter;
    private Action callback_eixt;
    public virtual IEnumerator PlayStartAsync()
    {
        //这里编写淡入动画的逻辑, 下面是示例
        width = 0;
        yield return 0;
        while(true)
        {
            width += Time.deltaTime;
            if(width>=1f)
                break;
            yield return 0;
        }
        callback_enter?.Invoke();
        callback_enter = null;
    }
    public virtual IEnumerator PlayEndAsync()
    {
        //这里编写淡出动画的逻辑, 下面是示例
        width = 1;
        yield return 0;
        while(true)
        {
            width -= Time.deltaTime;
            if(width<=0f)
                break;
            yield return 0;
        }
        callback_eixt?.Invoke();
        callback_eixt = null;
    }

    public void PlayEnter(Action callback)
    {
        callback_enter=callback;
        StartCoroutine(PlayStartAsync());
    }

    public void PlayExit(Action callback)
    {
        callback_eixt=callback;
        StartCoroutine(PlayEndAsync());
    }

}

class Master:MonoBehaviour
{
    SampleTransition transition;
    MetaDic[] depends;

    //假如这个函数中, 我需要跳转到其他场景
    public void NextScene()
    {
        //配置过渡器
        SceneJumper.Transition = transition;
        //跳转至场景
        SceneTransition.ToScene(new SceneJumpInfo{
            loadMode = LoadSceneMode.Additive,  //加载模式为 叠加
            aimScene = "scene:origin:test_0",   //加载场景的 注册名
            depends = new string[][]{           //依赖的资源包, 这里直接用 string[][] 手动定义, 也可以由 MetaDic 传入
                new string[]{
                    "img:origin:test_0",
                    "img:origin:test_1",
                    "img:origin:test_2",
                },
                new string[]{                  
                    "map:origin:test_0",
                    "map:origin:test_1",
                    "map:origin:test_2",
                }
            },
            statusText = new string[]{           //显示的文本
                "loading images",
                "loading maps"
            }
        });
    }
}



```

如果希望通过过渡来遮盖一些加载初始化的中途过程, 可以像这样:
``` C#
    public void NextScene2()
    {
         transition.PlayEnter(NextScene2_1);
    }

    private void NextScene2_1()
    {
        //在这里可以插入一些操作
        transition.PlayExit(null);
    }
```
