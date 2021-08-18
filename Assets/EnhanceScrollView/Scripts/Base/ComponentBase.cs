using UnityEngine;
using UnityEngine.EventSystems;

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
