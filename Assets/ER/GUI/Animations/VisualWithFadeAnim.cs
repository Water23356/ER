using UnityEngine;
using ER;
using ER.ForEditor;

namespace ER.GUI.Animations
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public abstract class VisualWithFadeAnim : MonoBehaviour
    {
        protected UIFadeAnimation uiFadeAnimation;

        [DisplayLabel("显示键")]
        [SerializeField]
        private string m_visualKey = "visual control";

        public string VisualKey { get => m_visualKey; set => m_visualKey = value; }
        public bool IsVisible { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

        protected virtual void Awake()
        {
            uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.onDisable += () =>
            {
                gameObject.SetActive(false);
            };
        }

        [ContextMenu("播放淡入")]
        public virtual void PlayDisplay()
        {
            IsVisible = true;
            uiFadeAnimation.PlayDisplay();
        }

        [ContextMenu("播放淡出")]
        public virtual void PlayHiden()
        {
            uiFadeAnimation.PlayHiden();
        }

        public abstract void UpdateVisual(object infos);

        public VisualWithFadeAnim(string key)
        { m_visualKey = key; }
    }
}