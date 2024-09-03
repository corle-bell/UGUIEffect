//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-09-03 11:51:51
//
//----Desc:         Create By BM
//
//**************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UiEffect;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private bool isUpdate = false;
    private UITrail_Line TrailLine;
    private void Awake()
    {
        TrailLine = GetComponent<UITrail_Line>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isUpdate = true;
            TrailLine.Clear();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isUpdate = false;
        }

        if (isUpdate)
        {
            transform.position = Input.mousePosition;
        }
    }
}
