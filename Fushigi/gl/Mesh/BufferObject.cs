﻿using Silk.NET.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Fushigi.gl
{
    public class BufferObject : GLObject, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public int DataCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int DataStride { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int DataSizeInBytes => DataStride * DataCount;

        public BufferTargetARB Target { get; private set; }

        private GL _gl;

        public  BufferObject(GL gl, BufferTargetARB target) : base(gl.GenBuffer())
        {
            _gl = gl;
            Target = target;
        }

        public unsafe void SetData(uint[] data, BufferUsageARB hint = BufferUsageARB.StaticDraw) 
        {
            DataCount = data.Length;
            DataStride = sizeof(uint);

            Bind();
            fixed (uint* d = data)
            {
                _gl.BufferData(Target, (nuint)DataSizeInBytes, d, BufferUsageARB.StaticDraw);
            }
            Unbind();
        }

        public unsafe void SetData(byte[] data, BufferUsageARB hint = BufferUsageARB.StaticDraw)
        {
            DataCount = data.Length;
            DataStride = sizeof(byte);

            Bind();
            fixed (byte* d = data)
            {
                _gl.BufferData(Target, (nuint)DataSizeInBytes, d, BufferUsageARB.StaticDraw);
            }
            Unbind();
        }

        public unsafe void SetData<T>(T[] data, BufferUsageARB hint = BufferUsageARB.StaticDraw) where T : struct
        {
            DataCount = data.Length;
            DataStride = Marshal.SizeOf(typeof(T));

            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(Target, (nuint)DataSizeInBytes, d, BufferUsageARB.StaticDraw);
            }
            Unbind();
        }

        public void Bind()
        {
            _gl.BindBuffer(Target, ID);
        }

        public void Unbind()
        {
            _gl.BindBuffer(Target, 0);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(ID);
        }
    }
}