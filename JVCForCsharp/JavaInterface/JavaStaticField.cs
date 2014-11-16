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
    class JavaStaticField : JavaFieldPrototype
    {
        public JavaStaticField(JavaENV _env, JavaClass _cls, string _name, string _type)
            : base(_env, _cls, _name, _type)
        {
            id = env.GetStaticFieldID(klass.GetID(), name, JavaBasicConst.ToJNISigName(type));
            if (id <= 0) throw new Exception("static field[" + name + "] is not found");
        }

        public int GetIntValue() { return env.GetStaticIntField(klass.GetID(), id); }
        public long GetLongValue() { return env.GetStaticLongField(klass.GetID(), id); }
        public float GetFloatValue() { return env.GetStaticFloatField(klass.GetID(), id); }
        public double GetDoubleValue() { return env.GetStaticDoubleField(klass.GetID(), id); }
        public bool GetBooleanValue() { return env.GetStaticBooleanField(klass.GetID(), id); }
        public byte GetByteValue() { return (byte)env.GetStaticByteField(klass.GetID(), id); }
        public char GetCharValue() { return env.GetStaticCharField(klass.GetID(), id); }
        public short GetShortValue() { return env.GetStaticShortField(klass.GetID(), id); }
        public JavaObject GetObjectValue(JavaClass valclass)
        {
            int objid = env.GetStaticObjectField(klass.GetID(), id);
            if (objid <= 0) return null;
            JavaObject obj = new JavaObject(env, valclass, objid);
            return obj;
        }
    }
}
