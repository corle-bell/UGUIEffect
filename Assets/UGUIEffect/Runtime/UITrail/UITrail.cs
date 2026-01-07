//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-08-28 16:16:10
//
//----Desc:         Create By BM
//
//**************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UiEffect;
using UnityEngine.UI;

namespace UiEffect
{
    public class UITrail : BaseMeshEffect
    {
        public List<Vector3> Positions = new List<Vector3>();

        private void Update()
        {
            PositionCollect();
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive() == false)
            {
                return;
            }

            vh.Clear();

            BuildTrail(vh);
        }

        protected virtual void PositionCollect()
        {

        }

        protected virtual void BuildTrail(VertexHelper vh)
        {

        }

        protected void Refresh()
        {
            if (graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }

        public virtual void Clear()
        {
            
        }

        

       
    }
}
