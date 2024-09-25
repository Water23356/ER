using ER.GUI;
using ER.GUI.Animations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dev2
{
    [RequireComponent(typeof(UIFadeAnimation), typeof(ImageProgressBar))]
    public class FadeTransition : SceneTransition
    {
        [SerializeField]
        private Image img_background;

        [SerializeField]
        private TMP_Text txt_status;

        private UIFadeAnimation uiFadeAnimation;
        private ImageProgressBar bar;

        public override string statusText { get => txt_status.text; set => txt_status.text = value; }

        private void Awake()
        {
            uiFadeAnimation = GetComponent<UIFadeAnimation>();
            bar = GetComponent<ImageProgressBar>();
        }

        public override IEnumerator PlayEndAsync()
        {
            bool wait = true;
            uiFadeAnimation.ClearOnDisableEvent();
            uiFadeAnimation.onDisable += () => wait = false;
            uiFadeAnimation.PlayHiden();
            yield return new WaitWhile(() => wait);
        }

        public override IEnumerator PlayStartAsync()
        {
            bool wait = true;
            uiFadeAnimation.ClearOnEnableEvent();
            uiFadeAnimation.onEnable += () => wait = false;
            uiFadeAnimation.PlayDisplay();
            yield return new WaitWhile(() => wait);
        }

        private void Update()
        {
            if (progress == null) return;
            bar.SetAmount(progress.doneCount, progress.total);
            if (progress.IsDone)
            {
                bar.ProgressText = "ok";
                bar.Progress = 1f;
                progress = null;
            }
        }
    }
}