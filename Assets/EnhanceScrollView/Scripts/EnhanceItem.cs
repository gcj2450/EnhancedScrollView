using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class EnhanceItem : PageItem
{
    public Text NameText;
    public Text DurationText;

    // Curve center offset 
    private float dCurveCenterOffset = 0.0f;
    public float CenterOffSet
    {
        get { return this.dCurveCenterOffset; }
        set { dCurveCenterOffset = value; }
    }

    [HideInInspector]
    public Transform mTrs;

    protected override void Awake()
    {
        mTrs = this.transform;
    }

    public override void SetData(PageItemData data)
    {
        if (Data == data)
        {
            return;
        }
        else
        {
            Data = data;
            OnClickHandler = data.OnClickHandler;

            int id = (int)data.ItemData;
            NameText.text = id.ToString();
        }
    }

    // Update Item's status
    // 1. position
    // 2. scale
    // 3. "depth" is 2D or z Position in 3D to set the front and back item
    public virtual void UpdateScrollViewItems(
        float xValue,
        float depthCurveValue,
        int depthFactor,
        float itemCount,
        float yValue,
        float scaleValue)
    {
        Vector3 targetPos = Vector3.one;
        Vector3 targetScale = Vector3.one;
        // position
        targetPos.x = xValue;
        targetPos.y = yValue;

        mTrs.localPosition = targetPos;
        
        // Set the "depth" of item
        // targetPos.z = depthValue;
        SetItemDepth(depthCurveValue, depthFactor, itemCount);
        // scale
        targetScale.x = targetScale.y = scaleValue;
        mTrs.localScale = targetScale;
    }

    protected virtual void SetItemDepth(float depthCurveValue, int depthFactor, float itemCount)
    {
        int newDepth = (int)(depthCurveValue * itemCount);
        this.transform.SetSiblingIndex(newDepth);
    }

}
