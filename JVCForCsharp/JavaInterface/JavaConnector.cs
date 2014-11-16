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
    class JavaConnector
    {
        private int hJvmDll;
        private int[] hNecessaryDll;
        private string jvm_path;

        private JavaVM jvm;
        private JavaENV env;
        private Java java;

        private delegate int m_JNI_CreateJavaVM(IntPtr _jvmpp, IntPtr _envpp, ref JavaVMInitArgs _args);
        public JavaConnector()
        {
            init_java_vm_intron();
        }
        public JavaConnector(string _jvmpath)
        {
            init_java_vm_intron();
            jvm_path = _jvmpath;
        }
        private void init_java_vm_intron()
        {
            jvm = null;
            java = null; env = null;
            hJvmDll = 0;
            hNecessaryDll = null;
            jvm_path = JavaBasicConst.JVM_PATH;
        }

        public JavaVM GetJavaVM()
        {
            return jvm;
        }
        public JavaENV GetJavaENV()
        {
            return env;
        }

        public Java Connect()
        {
            if (java == null) java = new Java(this);
            return java;
        }

        private void LoadNecessaryDll()
        {
            hNecessaryDll = new int[JavaBasicConst.NECESSARY_DLL_NUMBER];
            hNecessaryDll[0] = NativeAPI.LoadLibrary(JavaBasicConst.N1_DLL);
        }
        private void UnloadNecessaryDll()
        {
            for (int i = 0; i < hNecessaryDll.Length; i++)
            {
                NativeAPI.FreeLibrary(hNecessaryDll[i]);
            }
        }

        public bool InitializeJVM(string jar_class_path)
        {
            string[] opts = { "-Djava.compiler=NONE", "-Djava.class.path=.;" + jar_class_path + ";", "-verbose:NONE" };
            return InitializeJVM(opts);
        }
        public bool InitializeJVM(string[] jvmoptions)
        {
            //LoadNecessaryDll();

            hJvmDll = NativeAPI.LoadLibrary(jvm_path);
            if (0==hJvmDll) return false;
            m_JNI_CreateJavaVM pCreateJavaVM = 
                (m_JNI_CreateJavaVM)NativeAPI.GetProcDelegateEx(hJvmDll, "JNI_CreateJavaVM", typeof(m_JNI_CreateJavaVM));
            if (null == pCreateJavaVM) return false;

            IntPtr[] optsptr = new IntPtr[jvmoptions.Length];
            for (int i = 0; i < jvmoptions.Length; i++)
            {
                optsptr[i] = IntPtr.Zero;
            }
            int[] optstmp = new int[2 * jvmoptions.Length];
            IntPtr options = IntPtr.Zero;

            int[] jvm_p = { 0 };
            int[] env_p = { 0 };
            int jvmp = 0, envp = 0;
            int[] jvmflist = new int[JavaBasicConst.JNI_JVM_INT];
            int[] envflist = new int[JavaBasicConst.JNI_ENV_INT];
            IntPtr jvm_p_p = IntPtr.Zero, env_p_p = IntPtr.Zero;

            // init options
            try
            {
                int i;
                for (i = 0; i < jvmoptions.Length; i++)
                {
                    optsptr[i] = NativeMemory.NewString(jvmoptions[i]);
                    optstmp[i * 2] = optsptr[i].ToInt32();
                    optstmp[i * 2 + 1] = 0;
                }
                options = NativeMemory.NewSpace(sizeof(int) * optstmp.Length);
                NativeMemory.CopyInt(options, optstmp);

                JavaVMInitArgs args = new JavaVMInitArgs();
                args.version = JavaBasicConst.JNI_VERSION_1_6;
                //args.version = JavaBasicConst.JNI_VERSION_1_7;
                args.nOptions = 3;
                args.options = options.ToInt32();
                args.ignoreUnrecognized = JavaBasicConst.JNI_TRUE;

                // init jvm,env
                for (i = 0; i < jvmflist.Length; i++) jvmflist[i] = 0;
                for (i = 0; i < envflist.Length; i++) envflist[i] = 0;

                jvm_p_p = NativeMemory.NewSpace(sizeof(int));
                env_p_p = NativeMemory.NewSpace(sizeof(int));

                // create a jvm
                int _res = pCreateJavaVM(jvm_p_p, env_p_p, ref args);
                if (_res < 0) throw new Exception("failed to create a jvm");

                NativeMemory.CopyIntBack(jvm_p_p, jvm_p);
                NativeMemory.CopyIntBack(env_p_p, env_p);
                jvmp = jvm_p[0];
                envp = env_p[0];
                NativeMemory.CopyIntBack(new IntPtr(jvmp), jvm_p);
                NativeMemory.CopyIntBack(new IntPtr(envp), env_p);
                NativeMemory.CopyIntBack(new IntPtr(jvm_p[0]), jvmflist);
                NativeMemory.CopyIntBack(new IntPtr(env_p[0]), envflist);

                // TODO: process  jvmflist, envflist
                jvm = new JavaVM(jvmp, jvmflist);
                env = new JavaENV(envp, envflist);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // dispose IntPtr
            NativeMemory.Dispose(options);
            foreach(IntPtr x in optsptr) NativeMemory.Dispose(x);
            NativeMemory.Dispose(jvm_p_p);
            NativeMemory.Dispose(env_p_p);

            return true;
        }

        public bool FinalizeJVM()
        {
            if (jvm != null) jvm.DestroyJavaVM();
            NativeAPI.FreeLibrary(hJvmDll);

            //UnloadNecessaryDll();

            jvm = null;
            env = null;
            hJvmDll = 0;
            hNecessaryDll = null;
            return true;
        }
    }
}
