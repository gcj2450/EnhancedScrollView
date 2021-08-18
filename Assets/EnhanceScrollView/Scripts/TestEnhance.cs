using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnhance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<PageItemData> datas = new List<PageItemData>();
        for (int i = 0; i < 1000; i++)
        {
            datas.Add(new PageItemData(i, OnClickHandler));
        }
        GetComponent<EnhanceScrollView>(). SetPageDatas(datas);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {

            List<PageItemData> datas = new List<PageItemData>();
            for (int i = 0; i < 1000; i++)
            {
                datas.Add(new PageItemData(i, OnClickHandler));
            }
            GetComponent<EnhanceScrollView>().SetPageDatas(datas, UnityEngine.Random.Range(0, datas.Count));
        }
    }

    private void OnClickHandler(object arg1, PageItemData arg2)
    {
        PageItem pid = (PageItem)arg1;
        Debug.Log(pid.GlobalIndex);
    }
}
