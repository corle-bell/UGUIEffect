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

        [SerializeField] protected bool autoPlay=true;
        
        private protected float tick = 0;
        
        private int status;
        void Awake()
        {
            FrameSize.x = 1.0f / Col;
            FrameSize.y = 1.0f / Row;
            lastId = -1;

            if (autoPlay)
            {
                Play();
            }
        }

        public UISpriteSheetAnimation Play()
        {
            status = 1;
            return this;
        }
        
        public UISpriteSheetAnimation Stop()
        {
            status = 0;
            return this;
        }

        public UISpriteSheetAnimation Clear()
        {
            ReStart();
            Update(0);
            Refresh();
            return this;
        }

        private void Update()
        {
            switch (status)
            {
                case 1:
                    Update(Time.deltaTime);
                    break;
            }
        }

        protected virtual void Update(float _delta)
        {
            float d = tick % Duration;
            int frameCount = Row * Col - EmptyFrame;
            int frameId = (int)((d / Duration) * frameCount);
            
            tick += _delta;

            if (frameId == lastId) return;

            lastId = frameId;
            FrameUV.x = frameId % Col * FrameSize.x;
            FrameUV.y = (Row-1-frameId / Col) * FrameSize.y;
            
            Refresh();
        }

        public UISpriteSheetAnimation ReStart()
        {
            tick = 0;
            return this;
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
        
        [UnityEditor.MenuItem("CONTEXT/UISpriteSheetAnimation/Clear")]
        public static void _Clear(UnityEditor.MenuCommand command)
        {
            var data = command.context as UISpriteSheetAnimation;
            data.Clear();
        }
        
        [UnityEditor.MenuItem("CONTEXT/UISpriteSheetAnimation/Stop")]
        public static void _Stop(UnityEditor.MenuCommand command)
        {
            var data = command.context as UISpriteSheetAnimation;
            data.Stop();
        }
        
        [UnityEditor.MenuItem("CONTEXT/UISpriteSheetAnimation/Play")]
        public static void _Play(UnityEditor.MenuCommand command)
        {
            var data = command.context as UISpriteSheetAnimation;
            data.Play();
        }
#endif
    }
}
