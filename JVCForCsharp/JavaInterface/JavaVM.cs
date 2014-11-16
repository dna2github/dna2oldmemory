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
    class JavaVM
    {
        private int reserved0;
        private int reserved1;
        private int reserved2;

        private int jvmpval;

        private delegate int m_DestroyJavaVM(int javavm);
        private delegate int m_AttachCurrentThread(int javavm, int envptr, IntPtr args);
        private delegate int m_DetachCurrentThread(int javavm);
        private delegate int m_GetEnv(int javavm, int envptr, int version);
        private delegate int m_AttachCurrentThreadAsDaemon(int javavm, int envptr, IntPtr args);

        private m_DestroyJavaVM DestroyJavaVM_;
        private m_AttachCurrentThread AttachCurrentThread_;
        private m_DetachCurrentThread DetachCurrentThread_;
        private m_GetEnv GetEnv_;
        private m_AttachCurrentThreadAsDaemon AttachCurrentThreadAsDaemon_;

        public JavaVM(int jvmp,int[] flist)
        {
            jvmpval = jvmp;
            LoadJavaVM(flist);
        }

        public void LoadJavaVM(int[] flist)
        {
            reserved0 = flist[0];
            reserved1 = flist[1];
            reserved2 = flist[2];
            DestroyJavaVM_ = (m_DestroyJavaVM)NativeAPI.GetProcDelegate(flist[3], typeof(m_DestroyJavaVM));
            AttachCurrentThread_ = (m_AttachCurrentThread)NativeAPI.GetProcDelegate(flist[4], typeof(m_AttachCurrentThread));
            DetachCurrentThread_ = (m_DetachCurrentThread)NativeAPI.GetProcDelegate(flist[5], typeof(m_DetachCurrentThread));
            GetEnv_ = (m_GetEnv)NativeAPI.GetProcDelegate(flist[6], typeof(m_GetEnv));
            AttachCurrentThreadAsDaemon_ = (m_AttachCurrentThreadAsDaemon)NativeAPI.GetProcDelegate(flist[7], typeof(m_AttachCurrentThreadAsDaemon));
        }

        public int GetJavaVM()
        {
            return jvmpval;
        }

        public int DestroyJavaVM()
        {
            return DestroyJavaVM_(jvmpval);
        }

        public int AttachCurrentThread(int envptr, IntPtr args)
        {
            return AttachCurrentThread_(jvmpval, envptr, args);
        }

        public int DetachCurrentThread()
        {
            return DetachCurrentThread_(jvmpval);
        }

        public int GetEnv(int envptr, int version)
        {
            return GetEnv_(jvmpval, envptr, version);
        }

        public int AttachCurrentThreadAsDaemon(int envptr, IntPtr args)
        {
            return AttachCurrentThreadAsDaemon_(jvmpval, envptr, args);
        }
    }
}
