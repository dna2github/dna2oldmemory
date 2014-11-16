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
    class JavaClass : JNIObject
    {
        public static JavaClass intclass = new JavaClass("int");
        public static JavaClass longclass = new JavaClass("long");
        public static JavaClass shortclass = new JavaClass("short");
        public static JavaClass charclass = new JavaClass("char");
        public static JavaClass byteclass = new JavaClass("byte");
        public static JavaClass boolclass = new JavaClass("boolean");
        public static JavaClass floatclass = new JavaClass("float");
        public static JavaClass doubleclass = new JavaClass("double");
        public static JavaClass voidclass = new JavaClass("void");
        public static JavaClass arrayclass = new JavaClass("array");

        private string name;

        private JavaClass(string _name)
        {
            env = Java.GetEnvironment(); name = _name; id = -1;
        }

        public JavaClass(JavaENV _env, string _name)
        {
            name = _name;
            env = _env;
            switch (name)
            {
                case "int": name = "java.lang.Integer"; break;
                case "long": name = "java.lang.Long"; break;
                case "boolean": name = "java.lang.Boolean"; break;
                case "float": name = "java.lang.Float"; break;
                case "double": name = "java.lang.Double"; break;
                case "char": name = "java.lang.Character"; break;
                case "short": name = "java.lang.Short"; break;
                case "byte": name = "java.lang.Byte"; break;
                case "array": name = "java.lang.Array"; break;
                case "void": name = "java.lang.Void"; break;
            }
            id = env.FindClass(JavaBasicConst.ToJNIClassFullName(name));
            name = _name;

            if (id <= 0) throw new Exception("class[" + name + "] is not found");
        }

        public string GetClassFullName()
        {
            return name;
        }

        public string GetClassName()
        {
            int p = name.LastIndexOf('.');
            if (p < 0) return name;
            p++;
            return name.Substring(p, name.Length - p);
        }

        public JavaConstructor GetConstructor(params string[] paramlist)
        {
            return new JavaConstructor(env, this, paramlist);
        }

        public JavaStaticField GetStaticField(string type, string name)
        {
            return new JavaStaticField(env, this, name, type);
        }

        public JavaStaticMethod GetStaticMethod(string returntype, string name, params string[] paramlist)
        {
            return new JavaStaticMethod(env, this, name, returntype, paramlist);
        }

        public JavaField GetField(string type, string name)
        {
            return new JavaField(env, this, name, type);
        }

        public JavaMethod GetMethod(string returntype, string name, params string[] paramlist)
        {
            return new JavaMethod(env, this, name, returntype, paramlist);
        }
    }
}
