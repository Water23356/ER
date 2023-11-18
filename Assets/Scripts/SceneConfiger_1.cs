using ER;
using UnityEngine;

class SceneConfiger_1 : SceneConfigure
{
    public string SceneName => "Scene1";

    public void Initialize()
    {
        Debug.Log("加载" + SceneName);
    }
}