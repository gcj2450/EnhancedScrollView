using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollItem : EnhanceItem
{
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

    public override void UpdateScrollViewItems(
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
        GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(scaleValue);
        // Set the "depth" of item
        // targetPos.z = depthValue;
        SetItemDepth(depthCurveValue, depthFactor, itemCount);
        // scale
        targetScale.x = targetScale.y = scaleValue;
        mTrs.localScale = targetScale;
    }

}
