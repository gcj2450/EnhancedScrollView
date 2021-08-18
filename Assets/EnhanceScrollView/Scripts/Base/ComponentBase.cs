using UnityEngine;
using UnityEngine.EventSystems;
///<summary>
///<para>Copyright (C) 2021 北京华创智丰科技有限公司版权所有</para>
/// <para>文 件 名： ComponentBase.cs</para>
/// <para>文件功能： 基础交互组件</para>
/// <para>开发部门： 软件部</para>
/// <para>创 建 人： 高参军</para>
/// <para>电子邮件： </para>
/// <para>创建日期：2021-7-20</para>
/// <para>修 改 人：</para>
/// <para>修改日期：</para>
/// <para>备    注：</para>
/// </summary>
public abstract class ComponentBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerUpHandler,IPointerDownHandler,
                                    IPointerClickHandler,ISelectHandler,IDeselectHandler,IUpdateSelectedHandler, 
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected bool isHovered;
    protected virtual void OnClick()
    {

    }

    protected virtual void OnEnter()
    {
        isHovered = true;
    }
    protected virtual void OnExit()
    {
        isHovered = false;
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
    }

    public virtual void OnUpdateSelected(BaseEventData eventData)
    {
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }
    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {

    }

    public virtual void OnDestroy()
	{
		
	}

}
