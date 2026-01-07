//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-09-02 17:07:28
//
//----Desc:         Create By BM
//
//**************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UiEffect
{
    [AddComponentMenu("UI/Effects/UITrail Line"), RequireComponent(typeof(Graphic))]
    public class UITrail_Line : UITrail
    {

        [SerializeField] protected Color StartColor = Color.white;

        [SerializeField] protected Color EndColor = Color.white;

        [SerializeField] private float Duration = 0.5f;

        [SerializeField] private float MinDistance = 10;

        [SerializeField] private AnimationCurve LineWidth = AnimationCurve.Linear(0, 1, 1, 1);

        private float tick = 0;
        private List<Vector4> SegmentMidPoint = new List<Vector4>();
        void Start()
        {
            Positions = new List<Vector3>();
            tick = 0;
        }

        public override void Clear()
        {
            Positions.Clear();
            tick = 0;
            Refresh();
        }

        protected override void PositionCollect()
        {
            tick += Time.deltaTime;
            if (tick < Duration)
            {
                Positions.Add(transform.position);
            }
            else
            {
                tick = Duration;
                Positions.RemoveAt(0);
                Positions.Add(transform.position);
            }

            Refresh();
        }

        protected override void BuildTrail(VertexHelper vh)
        {
            if (Positions.Count <= 0)
            {
                return;
            }

            RectTransform rt = transform as RectTransform;
            Rect rect = rt.rect;


            SegmentMidPoint.Clear();


            Vector3 last = transform.position;
            float _distance_cumulative = 0;

            SegmentMidPoint.Add(new Vector4(last.x, last.y, last.z, 0));
            float length=0;
            for (int i = 0; i < Positions.Count; i++)
            {
                Vector3 pos = Positions[Positions.Count - i - 1];
                float distance = Vector3.Distance(last, pos);
                if (distance>=MinDistance)
                {
                    length += distance;
                    SegmentMidPoint.Add(new Vector4(last.x, last.y, last.z, length));
                    last = pos;
                }
            }


            if (SegmentMidPoint.Count < 2) return;

            Color First_C = StartColor;
            float First_Width = LineWidth.Evaluate(0) * rect.width * 0.5f;
            float First_UV_Y = 0;

            {
                Vector3 Now = SegmentMidPoint[0];
                Vector3 Next = SegmentMidPoint[1];

                Now = transform.InverseTransformPoint(Now);
                Next = transform.InverseTransformPoint(Next);

                Vector3 Dir = (Next - Now).normalized;

                UGUIEffectHelper.AddUIVertex(vh, UGUIEffectHelper.CalculatePointLeft(Now, Dir, First_Width), First_C, new Vector2(0, First_UV_Y));
                UGUIEffectHelper.AddUIVertex(vh, UGUIEffectHelper.CalculatePointRight(Now, Dir, First_Width), First_C, new Vector2(1, First_UV_Y));
            }
            int tringleIndex = 0;
            for (int i = 1; i < SegmentMidPoint.Count - 1; i++)
            {
                Vector3 Now = SegmentMidPoint[i];
                Vector3 Next = SegmentMidPoint[i + 1];
                Now = transform.InverseTransformPoint(Now);
                Next = transform.InverseTransformPoint(Next);
                Vector3 Dir = (Next - Now).normalized;
                
                float p = SegmentMidPoint[i].w/length;
                float width = LineWidth.Evaluate(p) * rect.width * 0.5f;
                Color c = Color.Lerp(StartColor, EndColor, p);
               

                tringleIndex = vh.currentVertCount - 2;

                UGUIEffectHelper.AddUIVertex(vh, UGUIEffectHelper.CalculatePointLeft(Now, Dir, width), c, new Vector2(0, p));
                UGUIEffectHelper.AddUIVertex(vh, UGUIEffectHelper.CalculatePointRight(Now, Dir, width), c, new Vector2(1, p));

                vh.AddTriangle(tringleIndex, tringleIndex + 2, tringleIndex + 1);
                vh.AddTriangle(tringleIndex + 1, tringleIndex + 2, tringleIndex + 3);
            }

        }

    }
}
