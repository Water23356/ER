using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    public class LineSetter : MonoBehaviour
    {
        [DisplayLabel("线组")]
        [SerializeField]
        private List<LineSetting> lineGroup;

        [Tooltip("必须包含 ILineGetter 接口")]
        [DisplayLabel("设置对象")]
        [SerializeField]
        private MonoBehaviour setAim;

        private ILineGetter m_lineGetter;

        public ILineGetter Getter
        {
            get
            {
                if (m_lineGetter == null)
                {
                    m_lineGetter = setAim as ILineGetter;
                }
                return m_lineGetter;
            }
            set
            {
                m_lineGetter = value;
            }
        }

        public void UpdateSetLine()
        {
            var getter = Getter;
            if (getter == null) return;

            getter.ClearAllLine();

            for (int i = 0; i < lineGroup.Count; i++)
            {
                if (lineGroup[i].enable)
                    getter.AddLine(CreateLine(lineGroup[i]));
            }
        }

        public Line CreateLine(LineSetting setting)
        {
            Line line = new Line();
            line.positions = new List<Vector2>();
            for (int i=0;i<setting.pos.Length;i++)
            {
                line.positions.Add(setting.pos[i].localPosition);
            }
            line.loop = setting.loop;
            return line;
        }
    }

    [Serializable]
    public struct LineSetting
    {
        [DisplayLabel("锚点")]
        public Transform[] pos;

        [DisplayLabel("是否循环")]
        public bool loop;

        public bool enable;
    }
}