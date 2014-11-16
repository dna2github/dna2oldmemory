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

namespace JavaInterface
{
    class JavaBasicConst
    {
        /* java virtual machine */
        //public static string JVM_PATH = "C:\\Program Files\\Java\\jre6\\bin\\client\\jvm.dll";
        public static string JVM_PATH = "C:\\Program Files\\Java\\jre7\\bin\\client\\jvm.dll";

        public const int NECESSARY_DLL_NUMBER = 1;
        public const string N1_DLL = "MSVCR100.dll";

        /* bool constants */
        public const int JNI_FALSE = 0;
        public const int JNI_TRUE = 1;

        /* possible return values for JNI functions.*/
        public const int JNI_OK = 0;           /* success */
        public const int JNI_ERR = (-1);       /* unknown error */
        public const int JNI_EDETACHED = (-2); /* thread detached from the VM */
        public const int JNI_EVERSION = (-3);  /* JNI version error */
        public const int JNI_ENOMEM = (-4);    /* not enough memory */
        public const int JNI_EEXIST = (-5);    /* VM already created */
        public const int JNI_EINVAL = (-6);    /* invalid arguments */

        /* used in ReleaseScalarArrayElements */
        public const int JNI_COMMIT = 1;
        public const int JNI_ABORT = 2;

        /* java version */
        public const int JNI_VERSION_1_1 = 0x00010001;
        public const int JNI_VERSION_1_2 = 0x00010002;
        public const int JNI_VERSION_1_4 = 0x00010004;
        public const int JNI_VERSION_1_6 = 0x00010006;
        public const int JNI_VERSION_1_7 = 0x00010007;

        /* java struct basic size */
        public const int JNI_JVM_INT = 8;
        public const int JNI_ENV_INT = 233;

        public static string ToJNIClassFullName(string classname)
        {
            // convert a.b.c.d to a/b/c/d
            return classname.Replace('.', '/');
        }

        public static string ToJNISigName(string classname)
        {
            string signame = "";
            if (classname == null) return signame;

            int nameLength = classname.Length;

            // array like int[][][], java.lang.Boolean[]
            while (nameLength > 2)
            {
                if (classname[nameLength - 2] == '[' && classname[nameLength - 1] == ']')
                {
                    // array like int[], java.lang.Object[]
                    classname = classname.Substring(0, nameLength - 2);
                    nameLength -= 2;
                    signame += "[";
                }
                else break;
            }

            switch (classname)
            {
                case "void":
                    signame += "V";
                    break;
                case "int":
                    signame += "I";
                    break;
                case "char":
                    signame += "C";
                    break;
                case "boolean":
                    signame += "Z";
                    break;
                case "float":
                    signame += "F";
                    break;
                case "double":
                    signame += "D";
                    break;
                case "byte":
                    signame += "B";
                    break;
                case "long":
                    signame += "J";
                    break;
                case "short":
                    signame += "S";
                    break;
                default:
                    signame += "L" + ToJNIClassFullName(classname) + ";";
                    break;
            }
            return signame;
        }
    }
}
