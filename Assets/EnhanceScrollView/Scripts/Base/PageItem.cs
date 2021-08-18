using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 列表页列表项基类
/// </summary>
public class PageItem : ComponentBase
{
    private bool playScaleAnimation;
    //private float scale = 1;

    public RawImage ItemTexture;

    //点击是否播放音效
    [HideInInspector]
	public bool ClickPlayAudio=true;
    /// <summary>
    /// 在每页中的索引，每页都从0开始
    /// </summary>
    public int Id;
    /// <summary>
    /// 在列表控件中全局索引
    /// </summary>
    public int GlobalIndex = -1;

    /// <summary>
    /// 业务数据
    /// </summary>
    public PageItemData Data { get; set; }

    private Action<object,PageItemData> onClickHandler = null;
    public Action<object, PageItemData> OnClickHandler
    {
        get { return onClickHandler; }
        set { onClickHandler = value; }
    }

    private Action<object, PageItemData> onEnterHandler = null;
    public Action<object, PageItemData> OnEnterHandler
    {
        get { return onEnterHandler; }
        set { onEnterHandler = value; }
    }

    private Action<object, PageItemData> onExitHandler = null;
    public Action<object, PageItemData> OnExitHandler
    {
        get { return onExitHandler; }
        set { onExitHandler = value; }
    }

    private Action<object, PageItemData> onDownHandler = null;
    public Action<object, PageItemData> OnDownHandler
    {
        get { return onDownHandler; }
        set { onDownHandler = value; }
    }

    private Action<object, PageItemData> onUpHandler = null;
    public Action<object, PageItemData> OnUpHandler
    {
        get { return onUpHandler; }
        set { onUpHandler = value; }
    }

    private Action<PointerEventData> onItemBeginDrag = null;
    public Action<PointerEventData> OnItemBeginDrag
    {
        get { return onItemBeginDrag; }

        set { onItemBeginDrag = value; }
    }

    private Action<PointerEventData> onItemDrag = null;
    public Action<PointerEventData> OnItemDrag
    {
        get { return onItemDrag; }

        set { onItemDrag = value; }
    }

    private Action<PointerEventData> onItemEndDrag = null;
    public Action<PointerEventData> OnItemEndDrag
    {
        get { return onItemEndDrag; }

        set { onItemEndDrag = value; }
    }

    /// <summary>
    ///  获取焦点动画时间
    /// </summary>
    protected float AnimationTime = 0.2f;

    /// <summary>
    /// 获取焦点动画Z轴偏移
    /// </summary>
    protected float AnimationToOffset = -20f;

    /// <summary>
    /// 获取焦点Scale放大比例
    /// </summary>
    protected float AnimationToScale = 1.02f;

    /// <summary>
    /// 设置item初始状态（含全局索引并设置可以反复点击)
    /// </summary>
    /// <param name="index"></param>
    public virtual void Initialize(int index)
    {
        GlobalIndex = index;
    }
    /// <summary>用于标记该按钮是否为选中状态
    /// </summary>
    [HideInInspector]
    public bool isSelected = false;
    /// <summary>
    /// 设置业务数据实体
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetData(PageItemData data)
    {
        Data = data;
        OnClickHandler = data.OnClickHandler;
    }

    /// <summary>
    /// 重置为初始状态
    /// </summary>
    public virtual void Reset()
    {
        Data = null;
        OnClickHandler = null;
        //OnItemBeginDrag=null;
        //OnItemDrag = null;
        //OnItemEndDrag = null;
}

    protected override void OnClick()
    {
        base.OnClick();
        if (OnClickHandler != null)
        {
            OnClickHandler(this, Data);

        }
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        if (OnEnterHandler!=null)
        {
            OnEnterHandler(this, Data);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (OnUpHandler!=null)
        {
            OnUpHandler(this, Data);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (OnDownHandler != null)
        {
            OnDownHandler(this, Data);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        if (OnExitHandler != null)
        {
            OnExitHandler(this,Data);
        }
    }

    public virtual void SetDragActions(Action<PointerEventData> _beginDrag,
                                        Action<PointerEventData> _drag,
                                        Action<PointerEventData> _endDrag)
    {
        OnItemBeginDrag = _beginDrag;
        OnItemDrag = _drag;
        OnItemEndDrag = _endDrag;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (OnItemBeginDrag != null)
        {
            OnItemBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (OnItemDrag != null)
        {
            OnItemDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (OnItemEndDrag != null)
        {
            OnItemEndDrag(eventData);
        }
    }

}
