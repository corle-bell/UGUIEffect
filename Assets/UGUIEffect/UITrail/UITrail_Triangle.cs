//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-08-28 16:30:31
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
    [AddComponentMenu("UI/Effects/UITrail Triangle"), RequireComponent(typeof(Graphic))]
    public class UITrail_Triangle : UITrail
    {
        [SerializeField] protected Color _startColor;

        [SerializeField] protected Color _endColor;

        [SerializeField] private float delay = 0.15f;

        [SerializeField] protected int CollectFrame = 3;

        private float tick = 0;

        // Start is called before the first frame update
        void Start()
        {
            //code here
            Positions = new List<Vector3>();
            tick = 0;
        }

        public void Clear()
        {
            Positions.Clear();
            Refresh();
        }

        protected override void PositionCollect()
        {
            tick += Time.deltaTime;
            if (tick < delay) return;

            tick = 0;

            if (Positions.Count < CollectFrame)
            {
                Positions.Add(transform.position);
            }
            else
            {
                Positions.RemoveAt(0);
                Positions.Add(transform.position);
            }

            Refresh();
        }

        protected override void BuildTrail(VertexHelper vh)
        {
            if (Positions.Count < 2)
            {
                return;
            }

            RectTransform rt = transform as RectTransform;
            Rect rect = rt.rect;

            Vector3 Now = Positions[Positions.Count-1];
            Vector3 Last = Positions[0];

            float distance = Vector3.Distance(Now, Last);
            Vector3 Dir = (Last - Now).normalized;
            vh.Clear();


            // left 0
            AddUIVertex(vh, CalculatePointLeft(Vector2.zero, Dir, rect.width * 0.5f), _startColor, Vector2.zero);
            // right 1
            AddUIVertex(vh, CalculatePointRight(Vector2.zero, Dir, rect.width * 0.5f), _startColor, new Vector2(1, 0));

            // mid 2
            AddUIVertex(vh, Dir * distance, _endColor, new Vector2(0.5f, 1));

            vh.AddTriangle(0, 2, 1);
        }
    }
}
