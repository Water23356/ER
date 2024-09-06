using ER.GUI;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 资源加载进度的显示面板
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class LoadProgressPanel : EGUIPanel
{
    public float smoothing = 8f;
    public float smoothing_part = 20f;

    [Header("组件")]
    public RectTransform rect_progress;

    public TMP_Text text_status;
    private float progress_sum;
    private float progress_sum_aim;

    public RectTransform rect_progress_part;
    public TMP_Text text_loadings;
    private CanvasGroup canvasGroup;
    private float progress_part;
    private float progress_part_aim;

    //public event Action OnClosePanelEvent;

    public void SetProgressSum(float p)
    {
        //Debug.Log("更新进度: " + p);
        progress_sum_aim = p;
    }

    public void SetProgressPart(float p)
    {
        progress_part_aim = p;
    }

    public void SetStatusTxt(string txt)
    {
        text_status.text = txt;
    }

    public void SetLoadTxt(string txt)
    {
        text_loadings.text = txt;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        IsVisible = true;
    }

    private void Update()
    {
        progress_sum = Mathf.Lerp(progress_sum, progress_sum_aim, Time.deltaTime * smoothing);
        rect_progress.anchorMax = new Vector2(progress_sum, 1f);

        progress_part = Mathf.Lerp(progress_part, progress_part_aim, Time.deltaTime * smoothing_part);
        rect_progress_part.anchorMax = new Vector2(progress_part, 1f);
    }
}