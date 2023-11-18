using ER;
using UnityEngine;

class ProjectStart:MonoBehaviour
{
    private void Start()
    {
        SceneManager.Instance.AddScene(new SceneConfiger_1());
        SceneManager.Instance.AddScene(new SceneConfiger_2());
    }
    [ContextMenu("加载场景1")]
    public void LoadScene1()
    {
        SceneManager.Instance.LoadScene("Scene1");
    }
    [ContextMenu("加载场景2")]
    public void LoadScene2()
    {
        SceneManager.Instance.LoadScene("Scene2");
    }

}