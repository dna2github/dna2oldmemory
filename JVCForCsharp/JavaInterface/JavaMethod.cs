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
 * L.Y.Liu 2012.07.31
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace JavaInterface
{
    class JavaMethod : JavaMethodPrototype
    {
        public JavaMethod(JavaENV _env, JavaClass _cls, string _name, string _returntype, string[] _paramtypes)
            : base(_env,_cls,_name,_returntype,_paramtypes)
        {
            id = env.GetMethodID(klass.GetID(), name, GetMethodSig());
            if (id <= 0) throw new Exception("method[" + name + "] is not found");
        }

        public void VoidInvoke(JavaObject _obj, params object[] paramlist)
        {
            env.CallVoidMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public int IntInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallIntMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public long LongInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallLongMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public short ShortInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallShortMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public byte ByteInvoke(JavaObject _obj, params object[] paramlist)
        {
            return (byte)env.CallByteMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public bool BooleanInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallBooleanMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public char CharInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallCharMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public float FloatInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallFloatMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public double DoubleInvoke(JavaObject _obj, params object[] paramlist)
        {
            return env.CallDoubleMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
        }
        public JavaObject ObjectInvoke(JavaObject _obj, params object[] paramlist)
        {
            int objid = env.CallObjectMethod(_obj.GetID(), id, ToJNIObjects(paramlist));
            if (objid <= 0) return JavaObject.nullobject;
            JavaClass returntypeclass = new JavaClass(env, returntype);
            return new JavaObject(env, returntypeclass, objid);
        }
    }
}
