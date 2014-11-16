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
    abstract class JNIObject
    {
        protected JavaENV env;
        protected int id;

        public int GetID()
        {
            return id;
        }

        private static jvalue m_jvalue = new jvalue();
        public static ulong ToJNISingleObject(object _obj)
        {
            m_jvalue.l = 0;
            if (_obj == null) return m_jvalue.l;
            if (_obj is JavaObject)
            {
                m_jvalue.i = ((JavaObject)_obj).GetID();
            }
            else if (_obj is IntPtr)
            {
                m_jvalue.j = ((IntPtr)_obj).ToInt64();
            }
            else if (_obj is int)
            {
                m_jvalue.i = (int)_obj;
            }
            else if (_obj is long)
            {
                m_jvalue.j = (long)_obj;
            }
            else if (_obj is short)
            {
                m_jvalue.s = (short)_obj;
            }
            else if (_obj is char)
            {
                m_jvalue.c = (char)_obj;
            }
            else if (_obj is byte)
            {
                m_jvalue.b = (byte)_obj;
            }
            else if (_obj is bool)
            {
                m_jvalue.z = (bool)_obj;
            }
            else if (_obj is float)
            {
                m_jvalue.f = (float)_obj;
            }
            else if (_obj is double)
            {
                m_jvalue.d = (double)_obj;
            }
            return m_jvalue.l;
        }

        public static ulong[] ToJNIObjects(object[] _objs)
        {
            if (_objs == null) return new ulong[] { 0 };
            if (_objs.Length == 0) return new ulong[] { 0 };
            ulong[] r = new ulong[_objs.Length];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = ToJNISingleObject(_objs[i]);
            }
            return r;
        }
    }
}
