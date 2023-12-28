using ER.Shikigami.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_1: MonoBehaviour
{
    public BasicDialogPanel panel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    [ContextMenu("显示面板")]
    public void Display()
    {
        panel.Text = string.Empty;
        panel.NameText = string.Empty;
        panel.OpenPanel();
    }
    [ContextMenu("隐藏面板")]
    public void HIde()
    {
        panel.ClosePanel();
    }
    [ContextMenu("设置文本1")]
    public void SetText()
    {
        panel.NameText = "博丽灵梦";
        panel.SetText("好香啊~是烤红薯的气味",true);
    }
    [ContextMenu("设置文本2")]
    public void SetText2()
    {
        panel.NameText = "秋镶子";
        panel.SetText("混蛋巫女, 居然想要吃了神明", false);
    }
}
