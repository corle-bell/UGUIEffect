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


        protected Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle)
        {
            // 将角度转换为弧度
            float angleInRadians = angle * Mathf.Deg2Rad;

            // 平移到以 pivot 为原点的坐标
            float xNew = (point.x - pivot.x) * Mathf.Cos(angleInRadians) -
                (point.y - pivot.y) * Mathf.Sin(angleInRadians) + pivot.x;
            float yNew = (point.x - pivot.x) * Mathf.Sin(angleInRadians) +
                         (point.y - pivot.y) * Mathf.Cos(angleInRadians) + pivot.y;

            return new Vector2(xNew, yNew);
        }


        protected Vector2 CalculatePointLeft(Vector2 origin, Vector2 dir, float dist)
        {
            // 标准化方向向量
            Vector2 normalizedDir = dir.normalized;

            // 计算垂直向量（指向左侧）
            Vector2 perpendicularLeft = new Vector2(-normalizedDir.y, normalizedDir.x);

            // 计算左侧点
            Vector2 pointLeft = origin + perpendicularLeft * dist;

            return pointLeft;
        }

        protected Vector2 CalculatePointRight(Vector2 origin, Vector2 dir, float dist)
        {
            // 标准化方向向量
            Vector2 normalizedDir = dir.normalized;

            // 计算垂直向量（指向右侧）
            Vector2 perpendicularRight = new Vector2(normalizedDir.y, -normalizedDir.x);

            // 计算右侧点
            Vector2 pointRight = origin + perpendicularRight * dist;

            return pointRight;
        }

        protected void AddUIVertex(VertexHelper vList, Vector2 pos, Color _color, Vector2 uv)
        {
            vList.AddVert(CreateUIVertex(pos, _color, uv));
        }

        protected UIVertex CreateUIVertex(Vector2 pos, Color _color, Vector2 uv)
        {
            UIVertex vert0 = new UIVertex();
            vert0.position = new Vector3(pos.x, pos.y);
            vert0.color = _color;
            vert0.uv0 = new Vector4(uv.x, Mathf.Clamp01(1 - uv.y), 0, 0);
            return vert0;
        }
    }
}
