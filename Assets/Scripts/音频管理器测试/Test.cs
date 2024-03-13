using ER;
using ER.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace P2
{
    public class Test:MonoBehaviour
    {
        public RandomPlayer player;

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
            /*
            Debug.Log("加载音频资源");
            SoundManager.Instance.Load("wav:erinbone:sound_1");
            SoundManager.Instance.Load("wav:erinbone:sound_2");
            SoundManager.Instance.Load("wav:erinbone:sound_3");
            SoundManager.Instance.Load("wav:erinbone:sound_4");
            SoundManager.Instance.Load("wav:erinbone:sound_5");
            SoundManager.Instance.Load("wav:erinbone:sound_6");
            SoundManager.Instance.Load("wav:erinbone:sound_7");
            SoundManager.Instance.Load("wav:erinbone:sound_8");

            SoundManager.Instance.Load("wav:erinbone:punch_1");
            SoundManager.Instance.Load("wav:erinbone:punch_2");
            SoundManager.Instance.Load("wav:erinbone:punch_3");
            SoundManager.Instance.Load("wav:erinbone:punch_4");
            SoundManager.Instance.Load("wav:erinbone:punch_5");*/
            GR.AddLoader(new AudioLoader());
            GR.Load(() =>{ Debug.Log("音频加载完毕"); },true,
                "wav:erinbone:punch_1",
                "wav:erinbone:punch_2",
                "wav:erinbone:punch_3",
                "wav:erinbone:punch_4",
                "wav:erinbone:punch_5",
                "wav:erinbone:sound_1",
                "wav:erinbone:sound_2",
                "wav:erinbone:sound_3",
                "wav:erinbone:sound_4",
                "wav:erinbone:sound_5",
                "wav:erinbone:sound_6",
                "wav:erinbone:sound_7",
                "wav:erinbone:sound_8"
                );
        }
        [ContextMenu("播放 背景音乐_1")]
        public void Func_3()
        {
            SoundManager.Instance.PlayBGM("wav:erinbone:sound_1");
        }
        [ContextMenu("播放 背景音乐_5")]
        public void Func_4()
        {
            SoundManager.Instance.PlayBGM("wav:erinbone:sound_5");
        }
        [ContextMenu("播放 背景音乐_3")]
        public void Func_5()
        {
            SoundManager.Instance.PlayBGM("wav:erinbone:sound_3");
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
        [ContextMenu("播放 音乐_5")]
        public void Func_8()
        {
            SoundManager.Instance.Shoot("wav:erinbone:sound_5");
        }
        [ContextMenu("添加音乐列表")]
        public void Func_9()
        {
            player.AddClip("wav:erinbone:punch_1", 1);
            player.AddClip("wav:erinbone:punch_2", 1);
            player.AddClip("wav:erinbone:punch_3", 1);
            player.AddClip("wav:erinbone:punch_4", 1);
            player.AddClip("wav:erinbone:punch_5", 1);
        }
        [ContextMenu("播放 随机音乐")]
        public void Func_10()
        {
            loop = true;
            timer = 1f;
        }
        bool loop = false;
        float timer = 0f;
        private void Update()
        {
            if(loop)
            {
                if(timer>0)
                {
                    timer-=Time.deltaTime;
                }
                else
                {
                    timer = 1f;
                    player.Play();
                }
            }
        }
    }
}
