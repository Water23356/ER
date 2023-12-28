using ER;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace P2
{
    public class Test:MonoBehaviour
    {
        [ContextMenu("加载音频管理器")]
        public void Func_1()
        {
            Debug.Log("1111");
            SoundManager.Instance.Init();
            Debug.Log("2222");
        }
        [ContextMenu("加载音频资源")]
        public void Func_2()
        {
            Debug.Log("加载音频资源");
            SoundManager.Instance.Load("sounds.sound_1");
            SoundManager.Instance.Load("sounds.sound_2");
            SoundManager.Instance.Load("sounds.sound_3");
            SoundManager.Instance.Load("sounds.sound_4");
            SoundManager.Instance.Load("sounds.sound_5");
            SoundManager.Instance.Load("sounds.sound_6");
            SoundManager.Instance.Load("sounds.sound_7");
        }
        [ContextMenu("播放 背景音乐_1")]
        public void Func_3()
        {
            SoundManager.Instance.PlayBGM("sounds.sound_1");
        }
        [ContextMenu("播放 背景音乐_2")]
        public void Func_4()
        {
            SoundManager.Instance.PlayBGM("sounds.sound_2");
        }
        [ContextMenu("播放 背景音乐_3")]
        public void Func_5()
        {
            SoundManager.Instance.PlayBGM("sounds.sound_3");
        }
        [ContextMenu("暂停播放背景音乐")]
        public void Func_6()
        {
            SoundManager.Instance.PauseBGM();
        }
        [ContextMenu("停止播放背景音乐")]
        public void Func_7()
        {
            SoundManager.Instance.StopBGM();
        }
    }
}
