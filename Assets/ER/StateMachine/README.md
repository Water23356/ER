## 状态机

这个模块提供了一个状态机模板, 包含组件类和非组件类的 状态机模板;

此外还提供了以枚举泛型作为状态标签的状态机类

列表:

1. 类的描述
    1. [StateCell - 状态单元](#section_StateCell)
    2. [StateCellMachine - 状态机](#section_StateCellMachine)
    3. [CompStateMachine - 状态机(组件)](#section_CompStateMachine)
2. [示例](#section_sample)

## 类型描述
<a id="section_StateCell"></a>
### StateCell - 状态单元

这个类定义了状态单元的模板, 一个状态单元至少包含以下信息:
- index : 状态编号, 在同一个状态机内这是唯一的
- onEnter: 定义进入该状态时的行为
- onUpdate: 定义该状态中持续执行的行为
- onExit: 定义该状态离开时的行为


---
<a id="section_StateCellMachine"></a>
### StateCellMachine - 状态机

这个类定义了一个基础的状态机模板. 

使用规则: 你需要在正式使用前调用完成状态机初始化, 你只能在初始化阶段添加状态单元, 在正式运行后不可修改状态机

功能:
1. CreateStates<T>(): 你可以根据一个 T (枚举泛型)创建状态机所需的状态单元, 之后通过修改这些状态单元的属性, 完成状态机的初始化
2. TransitionTo()/ TransitionTo<T>(): 过渡到指定状态
3. SkipTo<T>(): 跳转到指定状态(无过渡逻辑)
4. Update(): 你需要在特定时机周期性持续调用该函数, 这里用于执行状态单元的 update 逻辑

---
<a id="section_CompStateMachine"></a>
### CompStateMachine - 状态机(组件)

这个类是状态机的组件版本, Update 直接连接Unity中的Update事件, 可以持续执行 onUpdate 内的逻辑

---
<a id="section_sample"></a>
## 示例

暂无

