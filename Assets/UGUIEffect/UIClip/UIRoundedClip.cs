//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-09-24 14:49:36
//
//----Desc:         Create By BM
//
//**************************************************

using UnityEngine;
using UnityEngine.UI;

namespace UiEffect
{
    public class UIRoundedClip : BaseMeshEffect
    {
        public int CornerSegments = 8;

        public float TopLeftRadius = 10;
        public float TopRightRadius = 10;
        public float BottomLeftRadius = 10;
        public float BottomRightRadius = 10;

        float halfWidth;
        float halfHeight;
        private Color verColor;
        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive() == false)
            {
                return;
            }
            verColor = graphic.color;
            
            vh.Clear();

            var rectTrans = transform as RectTransform;
            float width = rectTrans.sizeDelta.x;
            float height = rectTrans.sizeDelta.y;
           

            CreateRoundedRectMesh(vh,width, height, TopLeftRadius, TopRightRadius, BottomLeftRadius, BottomRightRadius,
                CornerSegments);
        }
        
        void CreateRoundedRectMesh(VertexHelper vh,float width, float height, float _T_L_radius, float _T_R_radius, float _B_L_radius, float _B_R_radius, int segments)
        {
            halfWidth = width *0.5f;
            halfHeight = height *0.5f;
            
            // 添加圆心
            UGUIEffectHelper.AddUIVertex(vh, Vector2.zero, verColor, new Vector2(0.5f, 0.5f));

            // 记录圆弧开始和结束的顶点ID
            Vector2Int v0 = Vector2Int.zero;  // 左下角
            Vector2Int v1 = Vector2Int.zero;  // 右下角
            Vector2Int v2 = Vector2Int.zero;  // 右上角
            Vector2Int v3 = Vector2Int.zero;  // 左上角

            // 添加四个圆角的顶点

            v0.x = vh.currentVertCount;
            v0.y = AddRoundedCorner(vh,new Vector2(-halfWidth + _B_L_radius,  -halfHeight + _B_L_radius), _B_L_radius, segments, 180); // 左下
            
            v1.x =vh.currentVertCount;
            v1.y = AddRoundedCorner(vh,new Vector2(halfWidth - _B_R_radius, -halfHeight + _B_R_radius), _B_R_radius, segments, 270);  // 右下
            
            v2.x = vh.currentVertCount;
            v2.y = AddRoundedCorner(vh,new Vector2(halfWidth - _T_R_radius, halfHeight - _T_R_radius), _T_R_radius, segments, 0);    // 右上
            
            v3.x = vh.currentVertCount;
            v3.y = AddRoundedCorner(vh,new Vector2(-halfWidth + _T_L_radius, halfHeight - _T_L_radius), _T_L_radius, segments, 90);  // 左上
            
            // 添加添加非圆弧部分的三角形索引
            vh.AddTriangle(0, v0.x, v3.y);
            vh.AddTriangle(0, v1.x, v0.y);
            vh.AddTriangle(0, v2.x, v1.y);
            vh.AddTriangle(0, v3.x, v2.y);
        }
        
        // 添加圆角的方法
        int AddRoundedCorner(VertexHelper vh, Vector2 center, float radius, int segments, float startAngle)
        {
            if (radius > 0)
            {
                segments = radius <= 0 ? 3 : segments;
        
                int startIndex = vh.currentVertCount;
                float angleStep = 90f / segments;
        
                // 添加圆弧顶点
                for (int i = 0; i <= segments; i++)
                {
                    float angle = startAngle + i * angleStep;
                    float rad = Mathf.Deg2Rad * angle;
                    Vector2 point = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius + center;
              
                    float uv_x = Mathf.InverseLerp(-halfWidth, halfWidth, point.x);
                    float uv_y = 1-Mathf.InverseLerp(-halfHeight, halfHeight, point.y);
                    UGUIEffectHelper.AddUIVertex(vh, point, verColor, new Vector2(uv_x, uv_y));
                }

                // 创建圆角的三角形索引  从中心到弧形顶点和下一个弧形顶点构成
                for (int i = 0; i < segments; i++)
                {
                    vh.AddTriangle(0, startIndex + i + 1, startIndex+ i);
                }
                return vh.currentVertCount-1;
            }
            else
            {
                Vector2 point = center;
                float uv_x = Mathf.InverseLerp(-halfWidth, halfWidth, point.x);
                float uv_y = 1-Mathf.InverseLerp(-halfHeight, halfHeight, point.y);
                UGUIEffectHelper.AddUIVertex(vh, point, verColor, new Vector2(uv_x, uv_y));
                return vh.currentVertCount-1;
            }
        }
    }
}
