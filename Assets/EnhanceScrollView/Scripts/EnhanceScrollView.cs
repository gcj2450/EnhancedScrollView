using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class EnhanceScrollView : MonoBehaviour
{
    // Control the item's scale curve
    public AnimationCurve scaleCurve;
    // Control the position curve
    public AnimationCurve positionCurve;
    public AnimationCurve depthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    // The start center index
    [Tooltip("The Start center index")]
    public int startCenterIndex = 0;
    // Offset width between item
    public float cellWidth = 10f;
    private float totalHorizontalWidth = 500.0f;
    // vertical fixed position value 
    public float yFixedPositionValue = 46.0f;

    // Lerp duration
    public float lerpDuration = 0.2f;
    /// <summary>
    /// 初始ID
    /// </summary>
    public int initID = 0;
    public GameObject ItemPrefab;

    [Space(5)][Header("Do Not Change Below")]
    /// <summary>
    /// 当前Center ID
    /// </summary>
    public int curID = 0;

    private float mCurrentDuration = 0.0f;
    private int mCenterIndex = 0;
    public bool enableLerpTween = true;

    // center and preCentered item
    private EnhanceItem curCenterItem;
    private EnhanceItem preCenterItem;

    // if we can change the target item
    private bool canChangeItem = true;
    private float dFactor = 0.2f;

    // originHorizontalValue Lerp to horizontalTargetValue
    private float originHorizontalValue = 0.1f;
    public float curHorizontalValue = 0.5f;

    // "depth" factor (2d widget depth or 3d Z value)
    private int depthFactor = 5;

    // Drag enhance scroll view
    [Tooltip("Camera for drag ray cast")]
    public Camera sourceCamera;

    // targets enhance item in scroll view
    public List<EnhanceItem> listEnhanceItems=new List<EnhanceItem>();
    // sort to get right index
    public List<EnhanceItem> listSortedItems = new List<EnhanceItem>();

    float initPos = 0;

    List<PageItemData> Datas = new List<PageItemData>();


    void Awake()
    {
        //List<PageItemData> datas = new List<PageItemData>();
        //for (int i = 0; i < 100; i++)
        //{
        //    datas.Add(new PageItemData(i, OnClickHandler));
        //}
        //SetPageDatas(datas);
    }

    private void OnClickHandler(object arg1, PageItemData arg2)
    {
        PageItem pid = (PageItem)arg1;
        Debug.Log(pid.GlobalIndex);
    }

    public void  SetPageDatas(List<PageItemData>_datas,int _initId=0)
    {
        if (_initId!=0)
        {
            initID = _initId;
        }
        if(_datas.Count==0)
        {
            Debug.Log("NoData...");
            return;
        }
        Datas = _datas;
        if (sourceCamera == null)
        {
            sourceCamera = Camera.main;
        }
        canChangeItem = true;
        int count = listEnhanceItems.Count;

        if (count==0)
        {
            for (int i = 0; i < 7; i++)
            {
                GameObject go = Instantiate(ItemPrefab);
                go.SetActive(true);
                go.transform.SetParent(transform);
                go.name = i.ToString();
                EnhanceItem enhanceItem = go.GetComponent<EnhanceItem>();
                listEnhanceItems.Add(enhanceItem);
                enhanceItem.SetDragActions(OnItemBeginDrag, OnItemDrag, OnItemEndDrag);
            }
            count = 7;
        }

        dFactor = (Mathf.RoundToInt((1f / count) * 10000f)) * 0.0001f;
        mCenterIndex = count / 2;
        if (count % 2 == 0)
            mCenterIndex = count / 2 - 1;
        int index = 0;
        for (int i = count - 1; i >= 0; i--)
        {
            listEnhanceItems[i].Id = i;
            listEnhanceItems[i].CenterOffSet = dFactor * (mCenterIndex - index);
            //listEnhanceItems[i].SetSelectState(false);
            //GameObject obj = listEnhanceItems[i].gameObject;
            //EnhanceItem script = obj.GetComponent<EnhanceItem>();
            //if (script != null)
            //    script.SetScrollView(this);
            index++;
        }

        // set the center item with startCenterIndex
        if (startCenterIndex < 0 || startCenterIndex >= count)
        {
            Debug.LogError("## startCenterIndex < 0 || startCenterIndex >= listEnhanceItems.Count  out of index ##");
            startCenterIndex = mCenterIndex;
        }

        // sorted items
        listSortedItems = new List<EnhanceItem>(listEnhanceItems.ToArray());
        totalHorizontalWidth = cellWidth * count;
        curCenterItem = listEnhanceItems[startCenterIndex];
        curHorizontalValue = 0.5f - curCenterItem.CenterOffSet;
        initPos = curHorizontalValue;

        curID = Mathf.RoundToInt((initPos - curHorizontalValue) / dFactor)+ initID;
        //Debug.Log(curID);
        Invoke("SortItems", lerpDuration);
        LerpTweenToTarget(0f, curHorizontalValue, false);

        // 
        // enable the drag actions
        // 
        EnableDrag(true);
    }

    private void OnItemBeginDrag(PointerEventData eventData)
    {
    }

    private void OnItemDrag(PointerEventData eventData)
    {
            OnDragEnhanceViewMove(eventData.delta);
    }

    private void OnItemEndDrag(PointerEventData eventData)
    {
            OnDragEnhanceViewEnd();
    }

    public void EnableDrag(bool isEnabled)
    {
    }

    private void LerpTweenToTarget(float originValue, float targetValue, bool needTween = false)
    {
        if (!needTween)
        {
            SortEnhanceItem();
            originHorizontalValue = targetValue;
            UpdateEnhanceScrollView(targetValue);
            this.OnTweenOver();
        }
        else
        {
            originHorizontalValue = originValue;
            curHorizontalValue = targetValue;
            mCurrentDuration = 0.0f;
            //Debug.Log("curHorizontalValue: " + curHorizontalValue + "  dFactor: " + dFactor);
            curID = Mathf.RoundToInt((initPos - curHorizontalValue) / dFactor)+ initID;
            //Debug.Log(curID);
            Invoke("SortItems", lerpDuration);
        }
        enableLerpTween = needTween;
    }

    void SortItems()
    {
        //Sort Items
        SortEnhanceItem();

        //for (int i = 0; i < listSortedItems.Count; i++)
        //{
        //    Debug.Log(i+"___"+listSortedItems[i].transform.localPosition.x);
        //}
        //left most item id
        int firstItemDataId = curID - 3;
        //set items data
        for (int i = 0; i < 7; i++)
        {
            int id = firstItemDataId+i;
            while (id<0)
            {
                id += Datas.Count;
            }
            if (id>Datas.Count)
            {
                id = id % Datas.Count;
            }
            if (id == Datas.Count)
                id = 0;
            //Debug.Log("AAid:" + id);
            listSortedItems[i].SetData(Datas[id]);
        }
    }


    public void DisableLerpTween()
    {
        this.enableLerpTween = false;
    }

    /// 
    /// Update EnhanceItem state with curve fTime value
    /// 
    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            EnhanceItem itemScript = listEnhanceItems[i];
            float xValue = GetXPosValue(fValue, itemScript.CenterOffSet);
            float scaleValue = GetScaleValue(fValue, itemScript.CenterOffSet);
            float depthCurveValue = depthCurve.Evaluate(fValue + itemScript.CenterOffSet);
            itemScript.UpdateScrollViewItems(xValue, depthCurveValue, depthFactor, listEnhanceItems.Count, yFixedPositionValue, scaleValue);
        }
    }

    void Update()
    {
        if (enableLerpTween)
            TweenViewToTarget();
    }

    private void TweenViewToTarget()
    {
        mCurrentDuration += Time.deltaTime;
        if (mCurrentDuration > lerpDuration)
            mCurrentDuration = lerpDuration;

        float percent = mCurrentDuration / lerpDuration;
        float value = Mathf.Lerp(originHorizontalValue, curHorizontalValue, percent);
        UpdateEnhanceScrollView(value);
        if (mCurrentDuration >= lerpDuration)
        {
            canChangeItem = true;
            enableLerpTween = false;
            OnTweenOver();
        }
    }

    private void OnTweenOver()
    {
        //if (preCenterItem != null)
        //    preCenterItem.SetSelectState(false);
        //if (curCenterItem != null)
        //    curCenterItem.SetSelectState(true);
        //SortEnhanceItem();
    }

    // Get the evaluate value to set item's scale
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    // Get the X value set the Item's position
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * totalHorizontalWidth;
        return evaluateValue;
    }

    private int GetMoveCurveFactorCount(EnhanceItem preCenterItem, EnhanceItem newCenterItem)
    {
        SortEnhanceItem();
        int factorCount = Mathf.Abs(newCenterItem.GlobalIndex) - Mathf.Abs(preCenterItem.GlobalIndex);
        return Mathf.Abs(factorCount);
    }

    // sort item with X so we can know how much distance we need to move the timeLine(curve time line)
    static public int SortPosition(EnhanceItem a, EnhanceItem b) { return a.transform.localPosition.x.CompareTo(b.transform.localPosition.x); }
    private void SortEnhanceItem()
    {
        listSortedItems.Sort(SortPosition);
        for (int i = listSortedItems.Count - 1; i >= 0; i--)
            listSortedItems[i].GlobalIndex = i;
    }

    public void SetHorizontalTargetItemIndex(EnhanceItem selectItem)
    {
        if (!canChangeItem)
            return;

        if (curCenterItem == selectItem)
            return;

        canChangeItem = false;
        preCenterItem = curCenterItem;
        curCenterItem = selectItem;

        // calculate the direction of moving
        float centerXValue = positionCurve.Evaluate(0.5f) * totalHorizontalWidth;
        bool isRight = false;
        if (selectItem.transform.localPosition.x > centerXValue)
            isRight = true;

        // calculate the offset * dFactor
        int moveIndexCount = GetMoveCurveFactorCount(preCenterItem, selectItem);
        float dvalue = 0.0f;
        if (isRight)
        {
            dvalue = -dFactor * moveIndexCount;
        }
        else
        {
            dvalue = dFactor * moveIndexCount;
        }
        float originValue = curHorizontalValue;
        LerpTweenToTarget(originValue, curHorizontalValue + dvalue, true);
    }

    // Click the right button to select the next item.
    public void OnBtnRightClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.Id + 1;
        if (targetIndex > listEnhanceItems.Count - 1)
            targetIndex = 0;
        SetHorizontalTargetItemIndex(listEnhanceItems[targetIndex]);
    }

    // Click the left button the select next next item.
    public void OnBtnLeftClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.Id - 1;
        if (targetIndex < 0)
            targetIndex = listEnhanceItems.Count - 1;
        SetHorizontalTargetItemIndex(listEnhanceItems[targetIndex]);
    }

    public float factor = 0.001f;
    // On Drag Move
    public void OnDragEnhanceViewMove(Vector2 delta)
    {
        // In developing
        if (Mathf.Abs(delta.x) > 0.0f)
        {
            curHorizontalValue += delta.x * factor;
            LerpTweenToTarget(0.0f, curHorizontalValue, false);
        }
    }

    // On Drag End
    public void OnDragEnhanceViewEnd()
    {
        // find closed item to be centered
        int closestIndex = 0;
        float value = (curHorizontalValue - (int)curHorizontalValue);
        float min = float.MaxValue;
        float tmp = 0.5f * (curHorizontalValue < 0 ? -1 : 1);
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            float dis = Mathf.Abs(Mathf.Abs(value) - Mathf.Abs((tmp - listEnhanceItems[i].CenterOffSet)));
            if (dis < min)
            {
                closestIndex = i;
                min = dis;
            }
        }
        originHorizontalValue = curHorizontalValue;
        float target = ((int)curHorizontalValue + (tmp - listEnhanceItems[closestIndex].CenterOffSet));
        preCenterItem = curCenterItem;
        curCenterItem = listEnhanceItems[closestIndex];
        LerpTweenToTarget(originHorizontalValue, target, true);
        canChangeItem = false;
    }
}