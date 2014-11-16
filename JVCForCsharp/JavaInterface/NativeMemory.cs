/*
 * Copyright (c) 2011-2012, J.Y.Liu
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
/*
 * L.Y.Liu 2011.04.29
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace JavaInterface
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct jvalue
    {
        [FieldOffset(0)]
        public bool z;
        [FieldOffset(0)]
        public byte b;
        [FieldOffset(0)]
        public char c;
        [FieldOffset(0)]
        public short s;
        [FieldOffset(0)]
        public int i;
        [FieldOffset(0)]
        public long j;
        [FieldOffset(0)]
        public float f;
        [FieldOffset(0)]
        public double d;
        [FieldOffset(0)]
        public ulong l; // object
    }

    static class NativeMemory
    {
        public static IntPtr NewString(string _str)
        {
            return Marshal.StringToHGlobalAnsi(_str);
        }

        public static IntPtr NewStringEx(string _str)
        {
            IntPtr _r = IntPtr.Zero;
            char[] _c = _str.ToCharArray();
            _r = Marshal.AllocHGlobal(_c.Length);
            Marshal.Copy(_c, 0, _r, _c.Length);
            return _r;
        }

        public static IntPtr NewIntArray(int[] _x)
        {
            IntPtr _p = NewSpace(sizeof(int) * _x.Length);
            CopyInt(_p, _x);
            return _p;
        }

        public static IntPtr NewIntPtrArray(IntPtr[] _x)
        {
            if (_x == null) return IntPtr.Zero;
            IntPtr _p = NewSpace(IntPtr.Size * _x.Length);
            /*
                int[] _pint = new int[_x.Length];
                for (int i = 0; i < _x.Length; i++)
                {
                    _pint[i] = _x[i].ToInt32();
                }
                CopyInt(_p, _pint);
            */
            Marshal.Copy(_x, 0, _p, _x.Length);
            return _p;
        }

        public static IntPtr NewSpace(int _size)
        {
            return Marshal.AllocHGlobal(_size);
        }

        public static void CopyInt(IntPtr _des,int[] _x)
        {
            Marshal.Copy(_x, 0, _des, _x.Length);
        }

        public static void CopyIntBack(IntPtr _src, int[] _x)
        {
            Marshal.Copy(_src, _x, 0, _x.Length);
        }

        public static void Dispose(IntPtr _ptr)
        {
            if (IntPtr.Zero.Equals(_ptr)) return;
            Marshal.FreeHGlobal(_ptr);
        }

        public static IntPtr NewIntObject(int _x)
        {
            IntPtr _p = NewSpace(sizeof(int));
            Marshal.WriteInt32(_p, _x);
            return _p;
        }

        public static IntPtr NewLongObject(long _x)
        {
            IntPtr _p = NewSpace(sizeof(long));
            Marshal.WriteInt64(_p, _x);
            return _p;
        }

        public static IntPtr NewFloatObject(float _x)
        {
            byte[] _float = BitConverter.GetBytes(_x);
            IntPtr _p = NewSpace(sizeof(byte) * _float.Length);
            Marshal.Copy(_float, 0, _p, _float.Length);
            return _p;
        }

        public static IntPtr NewDoubleObject(double _x)
        {
            IntPtr _p = NewSpace(sizeof(long));
            Marshal.WriteInt64(_p, BitConverter.DoubleToInt64Bits(_x));
            return _p;
        }

        public static IntPtr NewObject(object _x)
        {
            if (_x == null) return IntPtr.Zero;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _bf =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream _ms =
                new System.IO.MemoryStream();
            _bf.Serialize(_ms, _x);
            byte[] _obj = _ms.ToArray();
            IntPtr _p = NewSpace(sizeof(byte) * _obj.Length);
            Marshal.Copy(_obj, 0, _p, _obj.Length);
            return _p;
        }

        public static IntPtr AllocPointer(object _obj)
        {
            GCHandle handle = GCHandle.Alloc(_obj);
            return GCHandle.ToIntPtr(handle);
        }
        public static void DisposePointer(IntPtr _ptr)
        {
            GCHandle handle = GCHandle.FromIntPtr(_ptr);
            handle.Free();
        }
    }
}
