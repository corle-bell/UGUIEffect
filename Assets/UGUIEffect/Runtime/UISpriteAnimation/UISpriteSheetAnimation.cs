//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-08-29 10:37:50
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
    [AddComponentMenu("UI/Effects/SpriteSheetAnimation"), RequireComponent(typeof(Image))]
    public class UISpriteSheetAnimation : BaseMeshEffect
    {
        [SerializeField] protected float Duration=2;
        [SerializeField] protected int Row = 2;
        [SerializeField] protected int Col = 2;

        [SerializeField] protected int EmptyFrame = 0;

        [SerializeField] protected Vector2 FrameUV;

        [SerializeField] protected Vector2 FrameSize;

        [SerializeField] protected int lastId;

        private protected float tick = 0;
        void Awake()
        {
            FrameSize.x = 1.0f / Col;
            FrameSize.y = 1.0f / Row;
            lastId = -1;
        }

        private void Update()
        {
            float d = tick % Duration;
            int frameCount = Row * Col - EmptyFrame;
            int frameId = (int)((d / Duration) * frameCount);
            
            tick += Time.deltaTime;

            if (frameId == lastId) return;

            lastId = frameId;
            FrameUV.x = frameId % Col * FrameSize.x;
            FrameUV.y = (Row-1-frameId / Col) * FrameSize.y;
            
            Refresh();

        }

        public void ReStart()
        {
            tick = 0;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive() == false)
            {
                return;
            }

            List<UIVertex> vList = UiEffectListPool<UIVertex>.Get();

            vh.GetUIVertexStream(vList);

            ModifyVertices(vList);

            vh.Clear();
            vh.AddUIVertexTriangleStream(vList);

            UiEffectListPool<UIVertex>.Release(vList);
        }

        private void ModifyVertices(List<UIVertex> vList)
        {
            if (IsActive() == false || vList == null || vList.Count == 0)
            {
                return;
            }

            UIVertex newVertex;
            for (int i = 0; i < vList.Count; i++)
            {
                newVertex = vList[i];
                newVertex.uv0.x = FrameSize.x * newVertex.uv0.x + FrameUV.x;
                newVertex.uv0.y = FrameSize.y * newVertex.uv0.y + FrameUV.y;
                vList[i] = newVertex;
            }
        }

        private void Refresh()
        {
            if (graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("CONTEXT/UISpriteSheetAnimation/Restart")]
        public static void _Restart(UnityEditor.MenuCommand command)
        {
            var data = command.context as UISpriteSheetAnimation;
            data.ReStart();
        }
#endif
    }
}
