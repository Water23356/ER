
## 0.1.0s7.20
- 优化 InitMonitor 系统
- InitMonitor 内容格式化, 允许引用项目直接修改代码加速开发
- 优化 0 场景, 内包含游戏的必要 Managers
- 重置了 InitMonitor 初始化结束后的场景跳转功能

## 0.1.0s7.19
- InitMonitor 是一个游戏项目的初始化入口, 由三部分组成
    - OackLoadInit: 用来加载 资源加载器,需要游戏中所需的 资源加载器 拖入该组件的列表中
    - ResourceLoadInit: 用来加载初始资源
- 所有需要加载的资源都按照 资源标签 打包进 LoadTaskPack 中(GameResource工具自动封装)
- 然后这些 资源加载包 需要被引用进 pack:origin:init 中, pack:origin:init 作为所有资源的加载入口

InitMonitor 的初始化顺序:
1. 配置控制台的 解释器(需要根据项目需要修改)
2. 装载游戏所需要资源加载器(需要根据项目需要修改)(资源加载器需要挂载在 PackLoadInit 所在的物体上)
3. 读取 pack:origin:init 加载包(路径可修改)
4. 加载 pack:origin:init 包中的资源, 通常里面是其他加载包
5. 按照初始化包列表, 加载资源分包
6. 加载结束, 调用LoadEnd(), (该方法需根据项目需要修改)