/*
 * Copyright (c) 2012, J.Y.Liu
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
 * L.Y.Liu 2012.08.02
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace JavaInterface
{
    class JavaArray : JavaObject
    {
        public JavaArray(int[] _arr)
            : base(Java.GetEnvironment(), JavaClass.intclass, 0)
        {
            id = env.NewIntArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of int");
            env.SetIntArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(long[] _arr)
            : base(Java.GetEnvironment(), JavaClass.longclass, 0)
        {
            id = env.NewLongArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of long");
            env.SetLongArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(short[] _arr)
            : base(Java.GetEnvironment(), JavaClass.shortclass, 0)
        {
            id = env.NewShortArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of short");
            env.SetShortArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(bool[] _arr)
            : base(Java.GetEnvironment(), JavaClass.boolclass, 0)
        {
            id = env.NewBooleanArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of boolean");
            env.SetBooleanArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(byte[] _arr)
            : base(Java.GetEnvironment(), JavaClass.byteclass, 0)
        {
            id = env.NewByteArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of byte");
            env.SetByteArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(char[] _arr)
            : base(Java.GetEnvironment(), JavaClass.charclass, 0)
        {
            id = env.NewCharArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of char");
            env.SetCharArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(float[] _arr)
            : base(Java.GetEnvironment(), JavaClass.floatclass, 0)
        {
            id = env.NewFloatArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of float");
            env.SetFloatArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(double[] _arr)
            : base(Java.GetEnvironment(), JavaClass.intclass, 0)
        {
            id = env.NewDoubleArray(_arr.Length);
            if (id <= 0) throw new Exception("fail to create an array of double");
            env.SetDoubleArrayRegion(id, 0, _arr.Length, _arr);
        }

        public JavaArray(string[] _arr)
            : base(Java.GetEnvironment(), null, 0)
        {
            klass = new JavaClass(env, "java.lang.String");
            JavaConstructor constructor = klass.GetConstructor();
            id = env.NewObjectArray(_arr.Length, klass.GetID(), constructor.GetID());
            if (id <= 0) throw new Exception("fail to create an array of object[" + klass.GetClassFullName() + "]");
            for (int i = 0; i < _arr.Length; i++)
            {
                int strtmpid = env.NewStringUTF(_arr[i]);
                env.SetObjectArrayElement(id, i, strtmpid);
                env.DeleteLocalRef(strtmpid);
            }
        }

        public JavaArray(JavaClass _cls, JavaConstructor _constructor, JavaObject[] _arr)
            : base(Java.GetEnvironment(), _cls, 0)
        {
            id = env.NewObjectArray(_arr.Length, _cls.GetID(), _constructor.GetID());
            if (id <= 0) throw new Exception("fail to create an array of object[" + klass.GetClassFullName() + "]");
            for (int i = 0; i < _arr.Length; i++)
            {
                env.SetObjectArrayElement(id, i, _arr[i].GetID());
            }
        }
    }
}
