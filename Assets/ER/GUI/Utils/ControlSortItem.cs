using UnityEngine;

/// <summary>
/// UI 控件排序(挂在控件上)
/// </summary>
public class ControlSortItem:MonoBehaviour, IControlSortItem
{
    [SerializeField]
    private int sort;

    public int Sort { get => sort;set=>sort = value; }

    public Transform Transform => transform;
}