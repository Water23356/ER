using ER;
using UnityEngine;

class SceneConfiger_2 : SceneConfigure
{
    public string SceneName => "Scene2";

    public void Initialize()
    {
        Debug.Log("加载" + SceneName);
    }
}