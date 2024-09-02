using ER.Control;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace ER.Shikigami.Message
{
    public class BasicSelectPanel : MonoBehaviour, ISelectPanel
    {
        #region 组件

        [SerializeField]
        private TMP_Text[] t_options;//选项文本

        [SerializeField]
        private Animator animator;//自身动画器

        #endregion 组件

        #region 私有字段
        private Instruct[] instructs;//选项效果
        private Action<Instruct> callback;//回调函数
        private bool visible = false;//窗口是否可见
        private List<string> options;//选项内容
        private int index;//当前索引
        private int count;//当前选项数量

        #endregion 私有字段

        #region 属性

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                if (visible)
                    OpenPanel();
                else
                    ClosePanel();
            }
        }
        public List<string> Options { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        #endregion 属性



        #region 方法


        public void ClosePanel()
        {
            animator.SetBool("enable", false);
            visible = false;
            for(int i=0;i<t_options.Length;i++)
            {
                t_options[i].gameObject.SetActive(false);
            }
        }

        public bool Execute(Instruct ist, Action<Instruct> callback = null)
        {
            if (ist==null)
            {
                //Debug.Log("指令为空");
                return true;
            }
            this.callback = callback;
            switch (ist.name)
            {
                case "OpenDialogPanel"://打开面板
                    OpenPanel();
                    return false;

                case "CloseDialogPanel":
                    ClosePanel();
                    return true;

                case "SetOptionsText"://设置选项文本(请在展示面板前设置)
                    for(int i=0;i<ist.marks.Length && i<t_options.Length;i++)//启用响应数量的选项
                    {
                        t_options[i].gameObject.SetActive(true);
                        t_options[i].text = ist.marks[i];
                    }
                    count = ist.marks.Length;
                    return true;
                case "SetOptionsEffect"://设置选项效果(请在展示面板前设置)
                    instructs = new Instruct[count];
                    for (int i = 0;i < instructs.Length; i++)//启用响应数量的选项
                    {
                        if (i < ist.marks.Length)
                        {
                            instructs[i] = SpellBook.ParseInstruct(ist.marks[i])[0];
                        }
                        else
                        {
                            instructs[i] = null;
                        }
                    }
                    return true;

                default:
                    Debug.LogError("未知指令:" + ist.name);
                    return true;
            }
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            animator.SetBool("enable", true);
            visible = true;
            index = 0;
        }

        protected virtual void OnDisable()
        {
        }

        public void SetPanelDisable()
        {
            gameObject.SetActive(false);
        }

        #endregion 方法

        protected virtual void Update()
        {
            if(Input.GetButton("Down"))
            {
                index = Math.Min(count - 1, index+1);
            }
            if(Input.GetButton("Up"))
            {
                index = Math.Max(0, index-1);
            }
            if(Input.GetButton("Submit"))
            {
                callback?.Invoke(instructs[index]);
                ClosePanel();
            }
        }
    }
}