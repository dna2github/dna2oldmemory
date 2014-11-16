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

    class JavaENV
    {
        private int envpval;

        private int reserved0;
        private int reserved1;
        private int reserved2;
        private int reserved3;

        private delegate int m_GetVersion(int env);
        private delegate int m_DefineClass(int env, int name, int loader, int buf, int len);
        private delegate int m_FindClass(int env, int name);
        private delegate int m_FromReflectedMethod(int env, int method);
        private delegate int m_FromReflectedField(int env, int field);
        private delegate int m_ToReflectedMethod(int env, int cls, int methodID, bool isStatic);
        private delegate int m_GetSuperclass(int env, int sub);
        private delegate bool m_IsAssignableFrom(int env, int sub, int sup);
        private delegate int m_ToReflectedField(int env, int cls, int fieldID, bool isStatic);
        private delegate int m_Throw(int env, int obj);
        private delegate int m_ThrowNew(int env, int clazz, int msg);
        private delegate int m_ExceptionOccurred(int env);
        private delegate void m_ExceptionDescribe(int env);
        private delegate void m_ExceptionClear(int env);
        private delegate void m_FatalError(int env, int msg);
        private delegate int m_PushLocalFrame(int env, int capacity);
        private delegate int m_PopLocalFrame(int env, int result);
        private delegate int m_NewGlobalRef(int env, int lobj);
        private delegate void m_DeleteGlobalRef(int env, int gref);
        private delegate void m_DeleteLocalRef(int env, int obj);
        private delegate bool m_IsSameObject(int env, int obj1, int obj2);
        private delegate int m_NewLocalRef(int env, int ref0);
        private delegate int m_EnsureLocalCapacity(int env, int capacity);
        private delegate int m_AllocObject(int env, int clazz);
        private delegate int m_NewObjectA(int env, int clazz, int methodID, ulong[] args);
        private delegate int m_GetObjectClass(int env, int obj);
        private delegate bool m_IsInstanceOf(int env, int obj, int clazz);
        private delegate int m_GetMethodID(int env, int clazz, int name, int sig);
        private delegate int m_CallObjectMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate bool m_CallBooleanMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate char m_CallByteMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate char m_CallCharMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate short m_CallShortMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate int m_CallIntMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate long m_CallLongMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate float m_CallFloatMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate double m_CallDoubleMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate void m_CallVoidMethodA(int env, int obj, int methodID, ulong[] args);
        private delegate int m_CallNonvirtualObjectMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate bool m_CallNonvirtualBooleanMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate char m_CallNonvirtualByteMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate char m_CallNonvirtualCharMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate short m_CallNonvirtualShortMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate int m_CallNonvirtualIntMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate long m_CallNonvirtualLongMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate float m_CallNonvirtualFloatMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate double m_CallNonvirtualDoubleMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate void m_CallNonvirtualVoidMethodA(int env, int obj, int clazz, int methodID, ulong[] args);
        private delegate int m_GetFieldID(int env, int clazz, int name, int sig);
        private delegate int m_GetObjectField(int env, int obj, int fieldID);
        private delegate bool m_GetBooleanField(int env, int obj, int fieldID);
        private delegate char m_GetByteField(int env, int obj, int fieldID);
        private delegate char m_GetCharField(int env, int obj, int fieldID);
        private delegate short m_GetShortField(int env, int obj, int fieldID);
        private delegate int m_GetIntField(int env, int obj, int fieldID);
        private delegate long m_GetLongField(int env, int obj, int fieldID);
        private delegate float m_GetFloatField(int env, int obj, int fieldID);
        private delegate double m_GetDoubleField(int env, int obj, int fieldID);
        private delegate void m_SetObjectField(int env, int obj, int fieldID, int val);
        private delegate void m_SetBooleanField(int env, int obj, int fieldID, bool val);
        private delegate void m_SetByteField(int env, int obj, int fieldID, char val);
        private delegate void m_SetCharField(int env, int obj, int fieldID, char val);
        private delegate void m_SetShortField(int env, int obj, int fieldID, short val);
        private delegate void m_SetIntField(int env, int obj, int fieldID, int val);
        private delegate void m_SetLongField(int env, int obj, int fieldID, long val);
        private delegate void m_SetFloatField(int env, int obj, int fieldID, float val);
        private delegate void m_SetDoubleField(int env, int obj, int fieldID, double val);
        private delegate int m_GetStaticMethodID(int env, int clazz, int  name, int sig);
        private delegate int m_CallStaticObjectMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate bool m_CallStaticBooleanMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate char m_CallStaticByteMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate char m_CallStaticCharMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate short m_CallStaticShortMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate int m_CallStaticIntMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate long m_CallStaticLongMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate float m_CallStaticFloatMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate double m_CallStaticDoubleMethodA(int env, int clazz, int methodID, ulong[] args);
        private delegate void m_CallStaticVoidMethodA(int env, int cls, int methodID, ulong[] args);
        private delegate int m_GetStaticFieldID(int env, int clazz, int  name, int  sig);
        private delegate int m_GetStaticObjectField(int env, int clazz, int fieldID);
        private delegate bool m_GetStaticBooleanField(int env, int clazz, int fieldID);
        private delegate char m_GetStaticByteField(int env, int clazz, int fieldID);
        private delegate char m_GetStaticCharField(int env, int clazz, int fieldID);
        private delegate short m_GetStaticShortField(int env, int clazz, int fieldID);
        private delegate int m_GetStaticIntField(int env, int clazz, int fieldID);
        private delegate long m_GetStaticLongField(int env, int clazz, int fieldID);
        private delegate float m_GetStaticFloatField(int env, int clazz, int fieldID);
        private delegate double m_GetStaticDoubleField(int env, int clazz, int fieldID);
        private delegate void m_SetStaticObjectField(int env, int clazz, int fieldID, int value);
        private delegate void m_SetStaticBooleanField(int env, int clazz, int fieldID, bool value);
        private delegate void m_SetStaticByteField(int env, int clazz, int fieldID, char value);
        private delegate void m_SetStaticCharField(int env, int clazz, int fieldID, char value);
        private delegate void m_SetStaticShortField(int env, int clazz, int fieldID, short value);
        private delegate void m_SetStaticIntField(int env, int clazz, int fieldID, int value);
        private delegate void m_SetStaticLongField(int env, int clazz, int fieldID, long value);
        private delegate void m_SetStaticFloatField(int env, int clazz, int fieldID, float value);
        private delegate void m_SetStaticDoubleField(int env, int clazz, int fieldID, double value);
        private delegate int m_NewString(int env, int  unicode, int len);
        private delegate int m_GetStringLength(int env, int str);
        private delegate char m_GetStringChars(int env, int str, bool[] isCopy);
        private delegate void m_ReleaseStringChars(int env, int str, int  chars);
        private delegate int m_NewStringUTF(int env, int  utf);
        private delegate int m_GetStringUTFLength(int env, int str);
        private delegate string m_GetStringUTFChars(int env, int str, bool[] isCopy);
        private delegate void m_ReleaseStringUTFChars(int env, int str, int  chars);
        private delegate int m_GetArrayLength(int env, int array);
        private delegate int m_NewObjectArray(int env, int len, int clazz, int init);
        private delegate int m_GetObjectArrayElement(int env, int array, int index);
        private delegate void m_SetObjectArrayElement(int env, int array, int index, int val);
        private delegate int m_NewBooleanArray(int env, int len);
        private delegate int m_NewByteArray(int env, int len);
        private delegate int m_NewCharArray(int env, int len);
        private delegate int m_NewShortArray(int env, int len);
        private delegate int m_NewIntArray(int env, int len);
        private delegate int m_NewLongArray(int env, int len);
        private delegate int m_NewFloatArray(int env, int len);
        private delegate int m_NewDoubleArray(int env, int len);
        private delegate bool m_GetBooleanArrayElements(int env, int array, bool[] isCopy);
        private delegate char m_GetByteArrayElements(int env, int array, bool[] isCopy);
        private delegate char m_GetCharArrayElements(int env, int array, bool[] isCopy);
        private delegate short m_GetShortArrayElements(int env, int array, bool[] isCopy);
        private delegate int m_GetIntArrayElements(int env, int array, bool[] isCopy);
        private delegate long m_GetLongArrayElements(int env, int array, bool[] isCopy);
        private delegate float m_GetFloatArrayElements(int env, int array, bool[] isCopy);
        private delegate double m_GetDoubleArrayElements(int env, int array, bool[] isCopy);
        private delegate void m_ReleaseBooleanArrayElements(int env, int array, bool[] elems, int mode);
        private delegate void m_ReleaseByteArrayElements(int env, int array, char[] elems, int mode);
        private delegate void m_ReleaseCharArrayElements(int env, int array, char[] elems, int mode);
        private delegate void m_ReleaseShortArrayElements(int env, int array, short[] elems, int mode);
        private delegate void m_ReleaseIntArrayElements(int env, int array, int[] elems, int mode);
        private delegate void m_ReleaseLongArrayElements(int env, int array, long[] elems, int mode);
        private delegate void m_ReleaseFloatArrayElements(int env, int array, float[] elems, int mode);
        private delegate void m_ReleaseDoubleArrayElements(int env, int array, double[] elems, int mode);
        private delegate void m_GetBooleanArrayRegion(int env, int array, int start, int l, bool[] buf);
        private delegate void m_GetByteArrayRegion(int env, int array, int start, int len, char[] buf);
        private delegate void m_GetCharArrayRegion(int env, int array, int start, int len, char[] buf);
        private delegate void m_GetShortArrayRegion(int env, int array, int start, int len, short[] buf);
        private delegate void m_GetIntArrayRegion(int env, int array, int start, int len, int[] buf);
        private delegate void m_GetLongArrayRegion(int env, int array, int start, int len, long[] buf);
        private delegate void m_GetFloatArrayRegion(int env, int array, int start, int len, float[] buf);
        private delegate void m_GetDoubleArrayRegion(int env, int array, int start, int len, double[] buf);
        private delegate void m_SetBooleanArrayRegion(int env, int array, int start, int l, bool[] buf);
        private delegate void m_SetByteArrayRegion(int env, int array, int start, int len, char[] buf);
        private delegate void m_SetCharArrayRegion(int env, int array, int start, int len, char[] buf);
        private delegate void m_SetShortArrayRegion(int env, int array, int start, int len, short[] buf);
        private delegate void m_SetIntArrayRegion(int env, int array, int start, int len, int[] buf);
        private delegate void m_SetLongArrayRegion(int env, int array, int start, int len, long[] buf);
        private delegate void m_SetFloatArrayRegion(int env, int array, int start, int len, float[] buf);
        private delegate void m_SetDoubleArrayRegion(int env, int array, int start, int len, double[] buf);
        private delegate int m_RegisterNatives(int env, int clazz, ref JNINativeMethod methods, int nMethods);
        private delegate int m_UnregisterNatives(int env, int clazz);
        private delegate int m_MonitorEnter(int env, int obj);
        private delegate int m_MonitorExit(int env, int obj);
        private delegate int m_GetJavaVM(int env, JavaVM[] vm);
        private delegate void m_GetStringRegion(int env, int str, int start, int len, char[] buf);
        private delegate void m_GetStringUTFRegion(int env, int str, int start, int len, char[] buf);
        private delegate int m_GetPrimitiveArrayCritical(int env, int array, bool[] isCopy);
        private delegate void m_ReleasePrimitiveArrayCritical(int env, int array, IntPtr[] carray, int mode);
        private delegate string m_GetStringCritical(int env, int str, bool[] isCopy);
        private delegate void m_ReleaseStringCritical(int env, int str, int  cstring);
        private delegate int m_NewWeakGlobalRef(int env, int obj);
        private delegate void m_DeleteWeakGlobalRef(int env, int ref0);
        private delegate bool m_ExceptionCheck(int env);
        private delegate int m_NewDirectByteBuffer(int env, int address, long capacity);
        private delegate int m_GetDirectBufferAddress(int env, int buf);
        private delegate long m_GetDirectBufferCapacity(int env, int buf);
        private delegate int m_GetObjectRefType(int env, int obj);

        private m_GetVersion GetVersion_;
        private m_DefineClass DefineClass_;
        private m_FindClass FindClass_;
        private m_FromReflectedMethod FromReflectedMethod_;
        private m_FromReflectedField FromReflectedField_;
        private m_ToReflectedMethod ToReflectedMethod_;
        private m_GetSuperclass GetSuperclass_;
        private m_IsAssignableFrom IsAssignableFrom_;
        private m_ToReflectedField ToReflectedField_;
        private m_Throw Throw_;
        private m_ThrowNew ThrowNew_;
        private m_ExceptionOccurred ExceptionOccurred_;
        private m_ExceptionDescribe ExceptionDescribe_;
        private m_ExceptionClear ExceptionClear_;
        private m_FatalError FatalError_;
        private m_PushLocalFrame PushLocalFrame_;
        private m_PopLocalFrame PopLocalFrame_;
        private m_NewGlobalRef NewGlobalRef_;
        private m_DeleteGlobalRef DeleteGlobalRef_;
        private m_DeleteLocalRef DeleteLocalRef_;
        private m_IsSameObject IsSameObject_;
        private m_NewLocalRef NewLocalRef_;
        private m_EnsureLocalCapacity EnsureLocalCapacity_;
        private m_AllocObject AllocObject_;
        private m_NewObjectA NewObjectA_;
        private m_GetObjectClass GetObjectClass_;
        private m_IsInstanceOf IsInstanceOf_;
        private m_GetMethodID GetMethodID_;
        private m_CallObjectMethodA CallObjectMethodA_;
        private m_CallBooleanMethodA CallBooleanMethodA_;
        private m_CallByteMethodA CallByteMethodA_;
        private m_CallCharMethodA CallCharMethodA_;
        private m_CallShortMethodA CallShortMethodA_;
        private m_CallIntMethodA CallIntMethodA_;
        private m_CallLongMethodA CallLongMethodA_;
        private m_CallFloatMethodA CallFloatMethodA_;
        private m_CallDoubleMethodA CallDoubleMethodA_;
        private m_CallVoidMethodA CallVoidMethodA_;
        private m_CallNonvirtualObjectMethodA CallNonvirtualObjectMethodA_;
        private m_CallNonvirtualBooleanMethodA CallNonvirtualBooleanMethodA_;
        private m_CallNonvirtualByteMethodA CallNonvirtualByteMethodA_;
        private m_CallNonvirtualCharMethodA CallNonvirtualCharMethodA_;
        private m_CallNonvirtualShortMethodA CallNonvirtualShortMethodA_;
        private m_CallNonvirtualIntMethodA CallNonvirtualIntMethodA_;
        private m_CallNonvirtualLongMethodA CallNonvirtualLongMethodA_;
        private m_CallNonvirtualFloatMethodA CallNonvirtualFloatMethodA_;
        private m_CallNonvirtualDoubleMethodA CallNonvirtualDoubleMethodA_;
        private m_CallNonvirtualVoidMethodA CallNonvirtualVoidMethodA_;
        private m_GetFieldID GetFieldID_;
        private m_GetObjectField GetObjectField_;
        private m_GetBooleanField GetBooleanField_;
        private m_GetByteField GetByteField_;
        private m_GetCharField GetCharField_;
        private m_GetShortField GetShortField_;
        private m_GetIntField GetIntField_;
        private m_GetLongField GetLongField_;
        private m_GetFloatField GetFloatField_;
        private m_GetDoubleField GetDoubleField_;
        private m_SetObjectField SetObjectField_;
        private m_SetBooleanField SetBooleanField_;
        private m_SetByteField SetByteField_;
        private m_SetCharField SetCharField_;
        private m_SetShortField SetShortField_;
        private m_SetIntField SetIntField_;
        private m_SetLongField SetLongField_;
        private m_SetFloatField SetFloatField_;
        private m_SetDoubleField SetDoubleField_;
        private m_GetStaticMethodID GetStaticMethodID_;
        private m_CallStaticObjectMethodA CallStaticObjectMethodA_;
        private m_CallStaticBooleanMethodA CallStaticBooleanMethodA_;
        private m_CallStaticByteMethodA CallStaticByteMethodA_;
        private m_CallStaticCharMethodA CallStaticCharMethodA_;
        private m_CallStaticShortMethodA CallStaticShortMethodA_;
        private m_CallStaticIntMethodA CallStaticIntMethodA_;
        private m_CallStaticLongMethodA CallStaticLongMethodA_;
        private m_CallStaticFloatMethodA CallStaticFloatMethodA_;
        private m_CallStaticDoubleMethodA CallStaticDoubleMethodA_;
        private m_CallStaticVoidMethodA CallStaticVoidMethodA_;
        private m_GetStaticFieldID GetStaticFieldID_;
        private m_GetStaticObjectField GetStaticObjectField_;
        private m_GetStaticBooleanField GetStaticBooleanField_;
        private m_GetStaticByteField GetStaticByteField_;
        private m_GetStaticCharField GetStaticCharField_;
        private m_GetStaticShortField GetStaticShortField_;
        private m_GetStaticIntField GetStaticIntField_;
        private m_GetStaticLongField GetStaticLongField_;
        private m_GetStaticFloatField GetStaticFloatField_;
        private m_GetStaticDoubleField GetStaticDoubleField_;
        private m_SetStaticObjectField SetStaticObjectField_;
        private m_SetStaticBooleanField SetStaticBooleanField_;
        private m_SetStaticByteField SetStaticByteField_;
        private m_SetStaticCharField SetStaticCharField_;
        private m_SetStaticShortField SetStaticShortField_;
        private m_SetStaticIntField SetStaticIntField_;
        private m_SetStaticLongField SetStaticLongField_;
        private m_SetStaticFloatField SetStaticFloatField_;
        private m_SetStaticDoubleField SetStaticDoubleField_;
        private m_NewString NewString_;
        private m_GetStringLength GetStringLength_;
        private m_GetStringChars GetStringChars_;
        private m_ReleaseStringChars ReleaseStringChars_;
        private m_NewStringUTF NewStringUTF_;
        private m_GetStringUTFLength GetStringUTFLength_;
        private m_GetStringUTFChars GetStringUTFChars_;
        private m_ReleaseStringUTFChars ReleaseStringUTFChars_;
        private m_GetArrayLength GetArrayLength_;
        private m_NewObjectArray NewObjectArray_;
        private m_GetObjectArrayElement GetObjectArrayElement_;
        private m_SetObjectArrayElement SetObjectArrayElement_;
        private m_NewBooleanArray NewBooleanArray_;
        private m_NewByteArray NewByteArray_;
        private m_NewCharArray NewCharArray_;
        private m_NewShortArray NewShortArray_;
        private m_NewIntArray NewIntArray_;
        private m_NewLongArray NewLongArray_;
        private m_NewFloatArray NewFloatArray_;
        private m_NewDoubleArray NewDoubleArray_;
        private m_GetBooleanArrayElements GetBooleanArrayElements_;
        private m_GetByteArrayElements GetByteArrayElements_;
        private m_GetCharArrayElements GetCharArrayElements_;
        private m_GetShortArrayElements GetShortArrayElements_;
        private m_GetIntArrayElements GetIntArrayElements_;
        private m_GetLongArrayElements GetLongArrayElements_;
        private m_GetFloatArrayElements GetFloatArrayElements_;
        private m_GetDoubleArrayElements GetDoubleArrayElements_;
        private m_ReleaseBooleanArrayElements ReleaseBooleanArrayElements_;
        private m_ReleaseByteArrayElements ReleaseByteArrayElements_;
        private m_ReleaseCharArrayElements ReleaseCharArrayElements_;
        private m_ReleaseShortArrayElements ReleaseShortArrayElements_;
        private m_ReleaseIntArrayElements ReleaseIntArrayElements_;
        private m_ReleaseLongArrayElements ReleaseLongArrayElements_;
        private m_ReleaseFloatArrayElements ReleaseFloatArrayElements_;
        private m_ReleaseDoubleArrayElements ReleaseDoubleArrayElements_;
        private m_GetBooleanArrayRegion GetBooleanArrayRegion_;
        private m_GetByteArrayRegion GetByteArrayRegion_;
        private m_GetCharArrayRegion GetCharArrayRegion_;
        private m_GetShortArrayRegion GetShortArrayRegion_;
        private m_GetIntArrayRegion GetIntArrayRegion_;
        private m_GetLongArrayRegion GetLongArrayRegion_;
        private m_GetFloatArrayRegion GetFloatArrayRegion_;
        private m_GetDoubleArrayRegion GetDoubleArrayRegion_;
        private m_SetBooleanArrayRegion SetBooleanArrayRegion_;
        private m_SetByteArrayRegion SetByteArrayRegion_;
        private m_SetCharArrayRegion SetCharArrayRegion_;
        private m_SetShortArrayRegion SetShortArrayRegion_;
        private m_SetIntArrayRegion SetIntArrayRegion_;
        private m_SetLongArrayRegion SetLongArrayRegion_;
        private m_SetFloatArrayRegion SetFloatArrayRegion_;
        private m_SetDoubleArrayRegion SetDoubleArrayRegion_;
        private m_RegisterNatives RegisterNatives_;
        private m_UnregisterNatives UnregisterNatives_;
        private m_MonitorEnter MonitorEnter_;
        private m_MonitorExit MonitorExit_;
        private m_GetJavaVM GetJavaVM_;
        private m_GetStringRegion GetStringRegion_;
        private m_GetStringUTFRegion GetStringUTFRegion_;
        private m_GetPrimitiveArrayCritical GetPrimitiveArrayCritical_;
        private m_ReleasePrimitiveArrayCritical ReleasePrimitiveArrayCritical_;
        private m_GetStringCritical GetStringCritical_;
        private m_ReleaseStringCritical ReleaseStringCritical_;
        private m_NewWeakGlobalRef NewWeakGlobalRef_;
        private m_DeleteWeakGlobalRef DeleteWeakGlobalRef_;
        private m_ExceptionCheck ExceptionCheck_;
        private m_NewDirectByteBuffer NewDirectByteBuffer_;
        private m_GetDirectBufferAddress GetDirectBufferAddress_;
        private m_GetDirectBufferCapacity GetDirectBufferCapacity_;
        private m_GetObjectRefType GetObjectRefType_;

        public JavaENV(int envp, int[] flist)
        {
            envpval = envp;
            LoadJavaEEN(flist);
        }
        public void LoadJavaEEN(int[] flist)
        {
            reserved0 = flist[0];
            reserved1 = flist[1];
            reserved2 = flist[2];
            reserved3 = flist[3];

            GetVersion_ = (m_GetVersion)NativeAPI.GetProcDelegate(flist[4], typeof(m_GetVersion));
            DefineClass_ = (m_DefineClass)NativeAPI.GetProcDelegate(flist[5], typeof(m_DefineClass));
            FindClass_ = (m_FindClass)NativeAPI.GetProcDelegate(flist[6], typeof(m_FindClass));
            FromReflectedMethod_ = (m_FromReflectedMethod)NativeAPI.GetProcDelegate(flist[7], typeof(m_FromReflectedMethod));
            FromReflectedField_ = (m_FromReflectedField)NativeAPI.GetProcDelegate(flist[8], typeof(m_FromReflectedField));
            ToReflectedMethod_ = (m_ToReflectedMethod)NativeAPI.GetProcDelegate(flist[9], typeof(m_ToReflectedMethod));
            GetSuperclass_ = (m_GetSuperclass)NativeAPI.GetProcDelegate(flist[10], typeof(m_GetSuperclass));
            IsAssignableFrom_ = (m_IsAssignableFrom)NativeAPI.GetProcDelegate(flist[11], typeof(m_IsAssignableFrom));
            ToReflectedField_ = (m_ToReflectedField)NativeAPI.GetProcDelegate(flist[12], typeof(m_ToReflectedField));
            Throw_ = (m_Throw)NativeAPI.GetProcDelegate(flist[13], typeof(m_Throw));
            ThrowNew_ = (m_ThrowNew)NativeAPI.GetProcDelegate(flist[14], typeof(m_ThrowNew));
            ExceptionOccurred_ = (m_ExceptionOccurred)NativeAPI.GetProcDelegate(flist[15], typeof(m_ExceptionOccurred));
            ExceptionDescribe_ = (m_ExceptionDescribe)NativeAPI.GetProcDelegate(flist[16], typeof(m_ExceptionDescribe));
            ExceptionClear_ = (m_ExceptionClear)NativeAPI.GetProcDelegate(flist[17], typeof(m_ExceptionClear));
            FatalError_ = (m_FatalError)NativeAPI.GetProcDelegate(flist[18], typeof(m_FatalError));
            PushLocalFrame_ = (m_PushLocalFrame)NativeAPI.GetProcDelegate(flist[19], typeof(m_PushLocalFrame));
            PopLocalFrame_ = (m_PopLocalFrame)NativeAPI.GetProcDelegate(flist[20], typeof(m_PopLocalFrame));
            NewGlobalRef_ = (m_NewGlobalRef)NativeAPI.GetProcDelegate(flist[21], typeof(m_NewGlobalRef));
            DeleteGlobalRef_ = (m_DeleteGlobalRef)NativeAPI.GetProcDelegate(flist[22], typeof(m_DeleteGlobalRef));
            DeleteLocalRef_ = (m_DeleteLocalRef)NativeAPI.GetProcDelegate(flist[23], typeof(m_DeleteLocalRef));
            IsSameObject_ = (m_IsSameObject)NativeAPI.GetProcDelegate(flist[24], typeof(m_IsSameObject));
            NewLocalRef_ = (m_NewLocalRef)NativeAPI.GetProcDelegate(flist[25], typeof(m_NewLocalRef));
            EnsureLocalCapacity_ = (m_EnsureLocalCapacity)NativeAPI.GetProcDelegate(flist[26], typeof(m_EnsureLocalCapacity));
            AllocObject_ = (m_AllocObject)NativeAPI.GetProcDelegate(flist[27], typeof(m_AllocObject));
            NewObjectA_ = (m_NewObjectA)NativeAPI.GetProcDelegate(flist[30], typeof(m_NewObjectA));
            GetObjectClass_ = (m_GetObjectClass)NativeAPI.GetProcDelegate(flist[31], typeof(m_GetObjectClass));
            IsInstanceOf_ = (m_IsInstanceOf)NativeAPI.GetProcDelegate(flist[32], typeof(m_IsInstanceOf));
            GetMethodID_ = (m_GetMethodID)NativeAPI.GetProcDelegate(flist[33], typeof(m_GetMethodID));
            CallObjectMethodA_ = (m_CallObjectMethodA)NativeAPI.GetProcDelegate(flist[36], typeof(m_CallObjectMethodA));
            CallBooleanMethodA_ = (m_CallBooleanMethodA)NativeAPI.GetProcDelegate(flist[39], typeof(m_CallBooleanMethodA));
            CallByteMethodA_ = (m_CallByteMethodA)NativeAPI.GetProcDelegate(flist[42], typeof(m_CallByteMethodA));
            CallCharMethodA_ = (m_CallCharMethodA)NativeAPI.GetProcDelegate(flist[45], typeof(m_CallCharMethodA));
            CallShortMethodA_ = (m_CallShortMethodA)NativeAPI.GetProcDelegate(flist[48], typeof(m_CallShortMethodA));
            CallIntMethodA_ = (m_CallIntMethodA)NativeAPI.GetProcDelegate(flist[51], typeof(m_CallIntMethodA));
            CallLongMethodA_ = (m_CallLongMethodA)NativeAPI.GetProcDelegate(flist[54], typeof(m_CallLongMethodA));
            CallFloatMethodA_ = (m_CallFloatMethodA)NativeAPI.GetProcDelegate(flist[57], typeof(m_CallFloatMethodA));
            CallDoubleMethodA_ = (m_CallDoubleMethodA)NativeAPI.GetProcDelegate(flist[60], typeof(m_CallDoubleMethodA));
            CallVoidMethodA_ = (m_CallVoidMethodA)NativeAPI.GetProcDelegate(flist[63], typeof(m_CallVoidMethodA));
            CallNonvirtualObjectMethodA_ = (m_CallNonvirtualObjectMethodA)NativeAPI.GetProcDelegate(flist[66], typeof(m_CallNonvirtualObjectMethodA));
            CallNonvirtualBooleanMethodA_ = (m_CallNonvirtualBooleanMethodA)NativeAPI.GetProcDelegate(flist[69], typeof(m_CallNonvirtualBooleanMethodA));
            CallNonvirtualByteMethodA_ = (m_CallNonvirtualByteMethodA)NativeAPI.GetProcDelegate(flist[72], typeof(m_CallNonvirtualByteMethodA));
            CallNonvirtualCharMethodA_ = (m_CallNonvirtualCharMethodA)NativeAPI.GetProcDelegate(flist[75], typeof(m_CallNonvirtualCharMethodA));
            CallNonvirtualShortMethodA_ = (m_CallNonvirtualShortMethodA)NativeAPI.GetProcDelegate(flist[78], typeof(m_CallNonvirtualShortMethodA));
            CallNonvirtualIntMethodA_ = (m_CallNonvirtualIntMethodA)NativeAPI.GetProcDelegate(flist[81], typeof(m_CallNonvirtualIntMethodA));
            CallNonvirtualLongMethodA_ = (m_CallNonvirtualLongMethodA)NativeAPI.GetProcDelegate(flist[84], typeof(m_CallNonvirtualLongMethodA));
            CallNonvirtualFloatMethodA_ = (m_CallNonvirtualFloatMethodA)NativeAPI.GetProcDelegate(flist[87], typeof(m_CallNonvirtualFloatMethodA));
            CallNonvirtualDoubleMethodA_ = (m_CallNonvirtualDoubleMethodA)NativeAPI.GetProcDelegate(flist[90], typeof(m_CallNonvirtualDoubleMethodA));
            CallNonvirtualVoidMethodA_ = (m_CallNonvirtualVoidMethodA)NativeAPI.GetProcDelegate(flist[93], typeof(m_CallNonvirtualVoidMethodA));
            GetFieldID_ = (m_GetFieldID)NativeAPI.GetProcDelegate(flist[94], typeof(m_GetFieldID));
            GetObjectField_ = (m_GetObjectField)NativeAPI.GetProcDelegate(flist[95], typeof(m_GetObjectField));
            GetBooleanField_ = (m_GetBooleanField)NativeAPI.GetProcDelegate(flist[96], typeof(m_GetBooleanField));
            GetByteField_ = (m_GetByteField)NativeAPI.GetProcDelegate(flist[97], typeof(m_GetByteField));
            GetCharField_ = (m_GetCharField)NativeAPI.GetProcDelegate(flist[98], typeof(m_GetCharField));
            GetShortField_ = (m_GetShortField)NativeAPI.GetProcDelegate(flist[99], typeof(m_GetShortField));
            GetIntField_ = (m_GetIntField)NativeAPI.GetProcDelegate(flist[100], typeof(m_GetIntField));
            GetLongField_ = (m_GetLongField)NativeAPI.GetProcDelegate(flist[101], typeof(m_GetLongField));
            GetFloatField_ = (m_GetFloatField)NativeAPI.GetProcDelegate(flist[102], typeof(m_GetFloatField));
            GetDoubleField_ = (m_GetDoubleField)NativeAPI.GetProcDelegate(flist[103], typeof(m_GetDoubleField));
            SetObjectField_ = (m_SetObjectField)NativeAPI.GetProcDelegate(flist[104], typeof(m_SetObjectField));
            SetBooleanField_ = (m_SetBooleanField)NativeAPI.GetProcDelegate(flist[105], typeof(m_SetBooleanField));
            SetByteField_ = (m_SetByteField)NativeAPI.GetProcDelegate(flist[106], typeof(m_SetByteField));
            SetCharField_ = (m_SetCharField)NativeAPI.GetProcDelegate(flist[107], typeof(m_SetCharField));
            SetShortField_ = (m_SetShortField)NativeAPI.GetProcDelegate(flist[108], typeof(m_SetShortField));
            SetIntField_ = (m_SetIntField)NativeAPI.GetProcDelegate(flist[109], typeof(m_SetIntField));
            SetLongField_ = (m_SetLongField)NativeAPI.GetProcDelegate(flist[110], typeof(m_SetLongField));
            SetFloatField_ = (m_SetFloatField)NativeAPI.GetProcDelegate(flist[111], typeof(m_SetFloatField));
            SetDoubleField_ = (m_SetDoubleField)NativeAPI.GetProcDelegate(flist[112], typeof(m_SetDoubleField));
            GetStaticMethodID_ = (m_GetStaticMethodID)NativeAPI.GetProcDelegate(flist[113], typeof(m_GetStaticMethodID));
            CallStaticObjectMethodA_ = (m_CallStaticObjectMethodA)NativeAPI.GetProcDelegate(flist[116], typeof(m_CallStaticObjectMethodA));
            CallStaticBooleanMethodA_ = (m_CallStaticBooleanMethodA)NativeAPI.GetProcDelegate(flist[119], typeof(m_CallStaticBooleanMethodA));
            CallStaticByteMethodA_ = (m_CallStaticByteMethodA)NativeAPI.GetProcDelegate(flist[122], typeof(m_CallStaticByteMethodA));
            CallStaticCharMethodA_ = (m_CallStaticCharMethodA)NativeAPI.GetProcDelegate(flist[125], typeof(m_CallStaticCharMethodA));
            CallStaticShortMethodA_ = (m_CallStaticShortMethodA)NativeAPI.GetProcDelegate(flist[128], typeof(m_CallStaticShortMethodA));
            CallStaticIntMethodA_ = (m_CallStaticIntMethodA)NativeAPI.GetProcDelegate(flist[131], typeof(m_CallStaticIntMethodA));
            CallStaticLongMethodA_ = (m_CallStaticLongMethodA)NativeAPI.GetProcDelegate(flist[134], typeof(m_CallStaticLongMethodA));
            CallStaticFloatMethodA_ = (m_CallStaticFloatMethodA)NativeAPI.GetProcDelegate(flist[137], typeof(m_CallStaticFloatMethodA));
            CallStaticDoubleMethodA_ = (m_CallStaticDoubleMethodA)NativeAPI.GetProcDelegate(flist[140], typeof(m_CallStaticDoubleMethodA));
            CallStaticVoidMethodA_ = (m_CallStaticVoidMethodA)NativeAPI.GetProcDelegate(flist[143], typeof(m_CallStaticVoidMethodA));
            GetStaticFieldID_ = (m_GetStaticFieldID)NativeAPI.GetProcDelegate(flist[144], typeof(m_GetStaticFieldID));
            GetStaticObjectField_ = (m_GetStaticObjectField)NativeAPI.GetProcDelegate(flist[145], typeof(m_GetStaticObjectField));
            GetStaticBooleanField_ = (m_GetStaticBooleanField)NativeAPI.GetProcDelegate(flist[146], typeof(m_GetStaticBooleanField));
            GetStaticByteField_ = (m_GetStaticByteField)NativeAPI.GetProcDelegate(flist[147], typeof(m_GetStaticByteField));
            GetStaticCharField_ = (m_GetStaticCharField)NativeAPI.GetProcDelegate(flist[148], typeof(m_GetStaticCharField));
            GetStaticShortField_ = (m_GetStaticShortField)NativeAPI.GetProcDelegate(flist[149], typeof(m_GetStaticShortField));
            GetStaticIntField_ = (m_GetStaticIntField)NativeAPI.GetProcDelegate(flist[150], typeof(m_GetStaticIntField));
            GetStaticLongField_ = (m_GetStaticLongField)NativeAPI.GetProcDelegate(flist[151], typeof(m_GetStaticLongField));
            GetStaticFloatField_ = (m_GetStaticFloatField)NativeAPI.GetProcDelegate(flist[152], typeof(m_GetStaticFloatField));
            GetStaticDoubleField_ = (m_GetStaticDoubleField)NativeAPI.GetProcDelegate(flist[153], typeof(m_GetStaticDoubleField));
            SetStaticObjectField_ = (m_SetStaticObjectField)NativeAPI.GetProcDelegate(flist[154], typeof(m_SetStaticObjectField));
            SetStaticBooleanField_ = (m_SetStaticBooleanField)NativeAPI.GetProcDelegate(flist[155], typeof(m_SetStaticBooleanField));
            SetStaticByteField_ = (m_SetStaticByteField)NativeAPI.GetProcDelegate(flist[156], typeof(m_SetStaticByteField));
            SetStaticCharField_ = (m_SetStaticCharField)NativeAPI.GetProcDelegate(flist[157], typeof(m_SetStaticCharField));
            SetStaticShortField_ = (m_SetStaticShortField)NativeAPI.GetProcDelegate(flist[158], typeof(m_SetStaticShortField));
            SetStaticIntField_ = (m_SetStaticIntField)NativeAPI.GetProcDelegate(flist[159], typeof(m_SetStaticIntField));
            SetStaticLongField_ = (m_SetStaticLongField)NativeAPI.GetProcDelegate(flist[160], typeof(m_SetStaticLongField));
            SetStaticFloatField_ = (m_SetStaticFloatField)NativeAPI.GetProcDelegate(flist[161], typeof(m_SetStaticFloatField));
            SetStaticDoubleField_ = (m_SetStaticDoubleField)NativeAPI.GetProcDelegate(flist[162], typeof(m_SetStaticDoubleField));
            NewString_ = (m_NewString)NativeAPI.GetProcDelegate(flist[163], typeof(m_NewString));
            GetStringLength_ = (m_GetStringLength)NativeAPI.GetProcDelegate(flist[164], typeof(m_GetStringLength));
            GetStringChars_ = (m_GetStringChars)NativeAPI.GetProcDelegate(flist[165], typeof(m_GetStringChars));
            ReleaseStringChars_ = (m_ReleaseStringChars)NativeAPI.GetProcDelegate(flist[166], typeof(m_ReleaseStringChars));
            NewStringUTF_ = (m_NewStringUTF)NativeAPI.GetProcDelegate(flist[167], typeof(m_NewStringUTF));
            GetStringUTFLength_ = (m_GetStringUTFLength)NativeAPI.GetProcDelegate(flist[168], typeof(m_GetStringUTFLength));
            GetStringUTFChars_ = (m_GetStringUTFChars)NativeAPI.GetProcDelegate(flist[169], typeof(m_GetStringUTFChars));
            ReleaseStringUTFChars_ = (m_ReleaseStringUTFChars)NativeAPI.GetProcDelegate(flist[170], typeof(m_ReleaseStringUTFChars));
            GetArrayLength_ = (m_GetArrayLength)NativeAPI.GetProcDelegate(flist[171], typeof(m_GetArrayLength));
            NewObjectArray_ = (m_NewObjectArray)NativeAPI.GetProcDelegate(flist[172], typeof(m_NewObjectArray));
            GetObjectArrayElement_ = (m_GetObjectArrayElement)NativeAPI.GetProcDelegate(flist[173], typeof(m_GetObjectArrayElement));
            SetObjectArrayElement_ = (m_SetObjectArrayElement)NativeAPI.GetProcDelegate(flist[174], typeof(m_SetObjectArrayElement));
            NewBooleanArray_ = (m_NewBooleanArray)NativeAPI.GetProcDelegate(flist[175], typeof(m_NewBooleanArray));
            NewByteArray_ = (m_NewByteArray)NativeAPI.GetProcDelegate(flist[176], typeof(m_NewByteArray));
            NewCharArray_ = (m_NewCharArray)NativeAPI.GetProcDelegate(flist[177], typeof(m_NewCharArray));
            NewShortArray_ = (m_NewShortArray)NativeAPI.GetProcDelegate(flist[178], typeof(m_NewShortArray));
            NewIntArray_ = (m_NewIntArray)NativeAPI.GetProcDelegate(flist[179], typeof(m_NewIntArray));
            NewLongArray_ = (m_NewLongArray)NativeAPI.GetProcDelegate(flist[180], typeof(m_NewLongArray));
            NewFloatArray_ = (m_NewFloatArray)NativeAPI.GetProcDelegate(flist[181], typeof(m_NewFloatArray));
            NewDoubleArray_ = (m_NewDoubleArray)NativeAPI.GetProcDelegate(flist[182], typeof(m_NewDoubleArray));
            GetBooleanArrayElements_ = (m_GetBooleanArrayElements)NativeAPI.GetProcDelegate(flist[183], typeof(m_GetBooleanArrayElements));
            GetByteArrayElements_ = (m_GetByteArrayElements)NativeAPI.GetProcDelegate(flist[184], typeof(m_GetByteArrayElements));
            GetCharArrayElements_ = (m_GetCharArrayElements)NativeAPI.GetProcDelegate(flist[185], typeof(m_GetCharArrayElements));
            GetShortArrayElements_ = (m_GetShortArrayElements)NativeAPI.GetProcDelegate(flist[186], typeof(m_GetShortArrayElements));
            GetIntArrayElements_ = (m_GetIntArrayElements)NativeAPI.GetProcDelegate(flist[187], typeof(m_GetIntArrayElements));
            GetLongArrayElements_ = (m_GetLongArrayElements)NativeAPI.GetProcDelegate(flist[188], typeof(m_GetLongArrayElements));
            GetFloatArrayElements_ = (m_GetFloatArrayElements)NativeAPI.GetProcDelegate(flist[189], typeof(m_GetFloatArrayElements));
            GetDoubleArrayElements_ = (m_GetDoubleArrayElements)NativeAPI.GetProcDelegate(flist[190], typeof(m_GetDoubleArrayElements));
            ReleaseBooleanArrayElements_ = (m_ReleaseBooleanArrayElements)NativeAPI.GetProcDelegate(flist[191], typeof(m_ReleaseBooleanArrayElements));
            ReleaseByteArrayElements_ = (m_ReleaseByteArrayElements)NativeAPI.GetProcDelegate(flist[192], typeof(m_ReleaseByteArrayElements));
            ReleaseCharArrayElements_ = (m_ReleaseCharArrayElements)NativeAPI.GetProcDelegate(flist[193], typeof(m_ReleaseCharArrayElements));
            ReleaseShortArrayElements_ = (m_ReleaseShortArrayElements)NativeAPI.GetProcDelegate(flist[194], typeof(m_ReleaseShortArrayElements));
            ReleaseIntArrayElements_ = (m_ReleaseIntArrayElements)NativeAPI.GetProcDelegate(flist[195], typeof(m_ReleaseIntArrayElements));
            ReleaseLongArrayElements_ = (m_ReleaseLongArrayElements)NativeAPI.GetProcDelegate(flist[196], typeof(m_ReleaseLongArrayElements));
            ReleaseFloatArrayElements_ = (m_ReleaseFloatArrayElements)NativeAPI.GetProcDelegate(flist[197], typeof(m_ReleaseFloatArrayElements));
            ReleaseDoubleArrayElements_ = (m_ReleaseDoubleArrayElements)NativeAPI.GetProcDelegate(flist[198], typeof(m_ReleaseDoubleArrayElements));
            GetBooleanArrayRegion_ = (m_GetBooleanArrayRegion)NativeAPI.GetProcDelegate(flist[199], typeof(m_GetBooleanArrayRegion));
            GetByteArrayRegion_ = (m_GetByteArrayRegion)NativeAPI.GetProcDelegate(flist[200], typeof(m_GetByteArrayRegion));
            GetCharArrayRegion_ = (m_GetCharArrayRegion)NativeAPI.GetProcDelegate(flist[201], typeof(m_GetCharArrayRegion));
            GetShortArrayRegion_ = (m_GetShortArrayRegion)NativeAPI.GetProcDelegate(flist[202], typeof(m_GetShortArrayRegion));
            GetIntArrayRegion_ = (m_GetIntArrayRegion)NativeAPI.GetProcDelegate(flist[203], typeof(m_GetIntArrayRegion));
            GetLongArrayRegion_ = (m_GetLongArrayRegion)NativeAPI.GetProcDelegate(flist[204], typeof(m_GetLongArrayRegion));
            GetFloatArrayRegion_ = (m_GetFloatArrayRegion)NativeAPI.GetProcDelegate(flist[205], typeof(m_GetFloatArrayRegion));
            GetDoubleArrayRegion_ = (m_GetDoubleArrayRegion)NativeAPI.GetProcDelegate(flist[206], typeof(m_GetDoubleArrayRegion));
            SetBooleanArrayRegion_ = (m_SetBooleanArrayRegion)NativeAPI.GetProcDelegate(flist[207], typeof(m_SetBooleanArrayRegion));
            SetByteArrayRegion_ = (m_SetByteArrayRegion)NativeAPI.GetProcDelegate(flist[208], typeof(m_SetByteArrayRegion));
            SetCharArrayRegion_ = (m_SetCharArrayRegion)NativeAPI.GetProcDelegate(flist[209], typeof(m_SetCharArrayRegion));
            SetShortArrayRegion_ = (m_SetShortArrayRegion)NativeAPI.GetProcDelegate(flist[210], typeof(m_SetShortArrayRegion));
            SetIntArrayRegion_ = (m_SetIntArrayRegion)NativeAPI.GetProcDelegate(flist[211], typeof(m_SetIntArrayRegion));
            SetLongArrayRegion_ = (m_SetLongArrayRegion)NativeAPI.GetProcDelegate(flist[212], typeof(m_SetLongArrayRegion));
            SetFloatArrayRegion_ = (m_SetFloatArrayRegion)NativeAPI.GetProcDelegate(flist[213], typeof(m_SetFloatArrayRegion));
            SetDoubleArrayRegion_ = (m_SetDoubleArrayRegion)NativeAPI.GetProcDelegate(flist[214], typeof(m_SetDoubleArrayRegion));
            RegisterNatives_ = (m_RegisterNatives)NativeAPI.GetProcDelegate(flist[215], typeof(m_RegisterNatives));
            UnregisterNatives_ = (m_UnregisterNatives)NativeAPI.GetProcDelegate(flist[216], typeof(m_UnregisterNatives));
            MonitorEnter_ = (m_MonitorEnter)NativeAPI.GetProcDelegate(flist[217], typeof(m_MonitorEnter));
            MonitorExit_ = (m_MonitorExit)NativeAPI.GetProcDelegate(flist[218], typeof(m_MonitorExit));
            GetJavaVM_ = (m_GetJavaVM)NativeAPI.GetProcDelegate(flist[219], typeof(m_GetJavaVM));
            GetStringRegion_ = (m_GetStringRegion)NativeAPI.GetProcDelegate(flist[220], typeof(m_GetStringRegion));
            GetStringUTFRegion_ = (m_GetStringUTFRegion)NativeAPI.GetProcDelegate(flist[221], typeof(m_GetStringUTFRegion));
            GetPrimitiveArrayCritical_ = (m_GetPrimitiveArrayCritical)NativeAPI.GetProcDelegate(flist[222], typeof(m_GetPrimitiveArrayCritical));
            ReleasePrimitiveArrayCritical_ = (m_ReleasePrimitiveArrayCritical)NativeAPI.GetProcDelegate(flist[223], typeof(m_ReleasePrimitiveArrayCritical));
            GetStringCritical_ = (m_GetStringCritical)NativeAPI.GetProcDelegate(flist[224], typeof(m_GetStringCritical));
            ReleaseStringCritical_ = (m_ReleaseStringCritical)NativeAPI.GetProcDelegate(flist[225], typeof(m_ReleaseStringCritical));
            NewWeakGlobalRef_ = (m_NewWeakGlobalRef)NativeAPI.GetProcDelegate(flist[226], typeof(m_NewWeakGlobalRef));
            DeleteWeakGlobalRef_ = (m_DeleteWeakGlobalRef)NativeAPI.GetProcDelegate(flist[227], typeof(m_DeleteWeakGlobalRef));
            ExceptionCheck_ = (m_ExceptionCheck)NativeAPI.GetProcDelegate(flist[228], typeof(m_ExceptionCheck));
            NewDirectByteBuffer_ = (m_NewDirectByteBuffer)NativeAPI.GetProcDelegate(flist[229], typeof(m_NewDirectByteBuffer));
            GetDirectBufferAddress_ = (m_GetDirectBufferAddress)NativeAPI.GetProcDelegate(flist[230], typeof(m_GetDirectBufferAddress));
            GetDirectBufferCapacity_ = (m_GetDirectBufferCapacity)NativeAPI.GetProcDelegate(flist[231], typeof(m_GetDirectBufferCapacity));
            GetObjectRefType_ = (m_GetObjectRefType)NativeAPI.GetProcDelegate(flist[232], typeof(m_GetObjectRefType));
        }

        public int GetVersion()
        {
            return GetVersion_(envpval);
        }
        public int DefineClass( string name, int loader, string buf, int len)
        {
            IntPtr _name = NativeMemory.NewString(name);
            IntPtr _buf = NativeMemory.NewString(name);
            int _r = DefineClass_(envpval, _name.ToInt32(), loader, _buf.ToInt32(), len);
            NativeMemory.Dispose(_name);
            NativeMemory.Dispose(_buf);
            return _r;
        }
        public int FindClass( string name)
        {
            IntPtr _name = NativeMemory.NewString(name);
            int _r = FindClass_(envpval, _name.ToInt32());
            NativeMemory.Dispose(_name);
            return _r;
        }
        public int FromReflectedMethod( int method)
        {
            return FromReflectedMethod_(envpval, method);
        }
        public int FromReflectedField( int field)
        {
            return FromReflectedField_(envpval, field);
        }
        public int ToReflectedMethod( int cls, int methodID, bool isStatic)
        {
            return ToReflectedMethod_(envpval, cls, methodID, isStatic);
        }
        public int GetSuperclass( int sub)
        {
            return GetSuperclass_(envpval, sub);
        }
        public bool IsAssignableFrom( int sub, int sup)
        {
            return IsAssignableFrom_(envpval, sub, sup);
        }
        public int ToReflectedField( int cls, int fieldID, bool isStatic)
        {
            return ToReflectedField_(envpval, cls, fieldID, isStatic);
        }
        public int Throw( int obj)
        {
            return Throw_(envpval, obj);
        }
        public int ThrowNew( int clazz, string msg)
        {
            IntPtr _msg = NativeMemory.NewString(msg);
            int _r = ThrowNew_(envpval, clazz, _msg.ToInt32());
            NativeMemory.Dispose(_msg);
            return _r;
        }
        public int ExceptionOccurred()
        {
            return ExceptionOccurred_(envpval);
        }
        public void ExceptionDescribe()
        {
            ExceptionDescribe_(envpval);
        }
        public void ExceptionClear()
        {
            ExceptionClear_(envpval);
        }
        public void FatalError( string msg)
        {
            IntPtr _msg = NativeMemory.NewString(msg);
            FatalError_(envpval, _msg.ToInt32());
            NativeMemory.Dispose(_msg);
        }
        public int PushLocalFrame( int capacity)
        {
            return PushLocalFrame_(envpval, capacity);
        }
        public int PopLocalFrame( int result)
        {
            return PopLocalFrame_(envpval, result);
        }
        public int NewGlobalRef( int lobj)
        {
            return NewGlobalRef_(envpval, lobj);
        }
        public void DeleteGlobalRef( int gref)
        {
            DeleteGlobalRef_(envpval, gref);
        }
        public void DeleteLocalRef( int obj)
        {
            DeleteLocalRef_(envpval, obj);
        }
        public bool IsSameObject( int obj1, int obj2)
        {
            return IsSameObject_(envpval, obj1, obj2);
        }
        public int NewLocalRef( int ref0)
        {
            return NewLocalRef_(envpval, ref0);
        }
        public int EnsureLocalCapacity( int capacity)
        {
            return EnsureLocalCapacity_(envpval, capacity);
        }
        public int AllocObject( int clazz)
        {
            return AllocObject_(envpval, clazz);
        }
        public int NewObject(int clazz, int methodID, ulong[] args)
        {
            return NewObjectA_(envpval, clazz, methodID, args);
        }
        public int GetObjectClass( int obj)
        {
            return GetObjectClass_(envpval, obj);
        }
        public bool IsInstanceOf( int obj, int clazz)
        {
            return IsInstanceOf_(envpval, obj, clazz);
        }
        public int GetMethodID( int clazz, string name, string sig)
        {
            IntPtr _name = NativeMemory.NewString(name);
            IntPtr _sig = NativeMemory.NewString(sig);
            int _r = GetMethodID_(envpval, clazz, _name.ToInt32(), _sig.ToInt32());
            NativeMemory.Dispose(_sig);
            NativeMemory.Dispose(_name);
            return _r;
        }
        public int CallObjectMethod(int obj, int methodID, ulong[] args)
        {
            return CallObjectMethodA_(envpval, obj, methodID, args);
        }
        public bool CallBooleanMethod(int obj, int methodID, ulong[] args)
        {
            return CallBooleanMethodA_(envpval, obj, methodID, args);
        }
        public char CallByteMethod(int obj, int methodID, ulong[] args)
        {
            return CallByteMethodA_(envpval, obj, methodID, args);
        }
        public char CallCharMethod(int obj, int methodID, ulong[] args)
        {
            return CallCharMethodA_(envpval, obj, methodID, args);
        }
        public short CallShortMethod(int obj, int methodID, ulong[] args)
        {
            return CallShortMethodA_(envpval, obj, methodID, args);
        }
        public int CallIntMethod(int obj, int methodID, ulong[] args)
        {
            return CallIntMethodA_(envpval, obj, methodID, args);
        }
        public long CallLongMethod(int obj, int methodID, ulong[] args)
        {
            return CallLongMethodA_(envpval, obj, methodID, args);
        }
        public float CallFloatMethod(int obj, int methodID, ulong[] args)
        {
            return CallFloatMethodA_(envpval, obj, methodID, args);
        }
        public double CallDoubleMethod(int obj, int methodID, ulong[] args)
        {
            return CallDoubleMethodA_(envpval, obj, methodID, args);
        }
        public void CallVoidMethod(int obj, int methodID, ulong[] args)
        {
            CallVoidMethodA_(envpval, obj, methodID, args);
        }
        public int CallNonvirtualObjectMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualObjectMethodA_(envpval, obj, clazz, methodID, args);
        }
        public bool CallNonvirtualBooleanMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualBooleanMethodA_(envpval, obj, clazz, methodID, args);
        }
        public char CallNonvirtualByteMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualByteMethodA_(envpval, obj, clazz, methodID, args);
        }
        public char CallNonvirtualCharMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualCharMethodA_(envpval, obj, clazz, methodID, args);
        }
        public short CallNonvirtualShortMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualShortMethodA_(envpval, obj, clazz, methodID, args);
        }
        public int CallNonvirtualIntMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualIntMethodA_(envpval, obj, clazz, methodID, args);
        }
        public long CallNonvirtualLongMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualLongMethodA_(envpval, obj, clazz, methodID, args);
        }
        public float CallNonvirtualFloatMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualFloatMethodA_(envpval, obj, clazz, methodID, args);
        }
        public double CallNonvirtualDoubleMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            return CallNonvirtualDoubleMethodA_(envpval, obj, clazz, methodID, args);
        }
        public void CallNonvirtualVoidMethod(int obj, int clazz, int methodID, ulong[] args)
        {
            CallNonvirtualVoidMethodA_(envpval, obj, clazz, methodID, args);
        }
        public int GetFieldID( int clazz, string name, string sig)
        {
            IntPtr _name = NativeMemory.NewString(name);
            IntPtr _sig = NativeMemory.NewString(sig);
            int _r = GetFieldID_(envpval, clazz, _name.ToInt32(), _sig.ToInt32());
            NativeMemory.Dispose(_sig);
            NativeMemory.Dispose(_name);
            return _r;
        }
        public int GetObjectField( int obj, int fieldID)
        {
            return GetObjectField_(envpval, obj, fieldID);
        }
        public bool GetBooleanField( int obj, int fieldID)
        {
            return GetBooleanField_(envpval, obj, fieldID);
        }
        public char GetByteField( int obj, int fieldID)
        {
            return GetByteField_(envpval, obj, fieldID);
        }
        public char GetCharField( int obj, int fieldID)
        {
            return GetCharField_(envpval, obj, fieldID);
        }
        public short GetShortField( int obj, int fieldID)
        {
            return GetShortField_(envpval, obj, fieldID);
        }
        public int GetIntField( int obj, int fieldID)
        {
            return GetIntField_(envpval, obj, fieldID);
        }
        public long GetLongField( int obj, int fieldID)
        {
            return GetLongField_(envpval, obj, fieldID);
        }
        public float GetFloatField( int obj, int fieldID)
        {
            return GetFloatField_(envpval, obj, fieldID);
        }
        public double GetDoubleField( int obj, int fieldID)
        {
            return GetDoubleField_(envpval, obj, fieldID);
        }
        public void SetObjectField( int obj, int fieldID, int val)
        {
            SetObjectField_(envpval, obj, fieldID, val);
        }
        public void SetBooleanField( int obj, int fieldID, bool val)
        {
            SetBooleanField_(envpval, obj, fieldID, val);
        }
        public void SetByteField( int obj, int fieldID, char val)
        {
            SetByteField_(envpval, obj, fieldID, val);
        }
        public void SetCharField( int obj, int fieldID, char val)
        {
            SetCharField_(envpval, obj, fieldID, val);
        }
        public void SetShortField( int obj, int fieldID, short val)
        {
            SetShortField_(envpval, obj, fieldID, val);
        }
        public void SetIntField( int obj, int fieldID, int val)
        {
            SetIntField_(envpval, obj, fieldID, val);
        }
        public void SetLongField( int obj, int fieldID, long val)
        {
            SetLongField_(envpval, obj, fieldID, val);
        }
        public void SetFloatField( int obj, int fieldID, float val)
        {
            SetFloatField_(envpval, obj, fieldID, val);
        }
        public void SetDoubleField( int obj, int fieldID, double val)
        {
            SetDoubleField_(envpval, obj, fieldID, val);
        }
        public int GetStaticMethodID( int clazz, string name, string sig)
        {
            IntPtr _name = NativeMemory.NewString(name);
            IntPtr _sig = NativeMemory.NewString(sig);
            int _r = GetStaticMethodID_(envpval, clazz, _name.ToInt32(), _sig.ToInt32());
            NativeMemory.Dispose(_sig);
            NativeMemory.Dispose(_name);
            return _r;
        }
        public int CallStaticObjectMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticObjectMethodA_(envpval, clazz, methodID, args);
        }
        public bool CallStaticBooleanMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticBooleanMethodA_(envpval, clazz, methodID, args);
        }
        public char CallStaticByteMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticByteMethodA_(envpval, clazz, methodID, args);
        }
        public char CallStaticCharMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticCharMethodA_(envpval, clazz, methodID, args);
        }
        public short CallStaticShortMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticShortMethodA_(envpval, clazz, methodID, args);
        }
        public int CallStaticIntMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticIntMethodA_(envpval, clazz, methodID, args);
        }
        public long CallStaticLongMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticLongMethodA_(envpval, clazz, methodID, args);
        }
        public float CallStaticFloatMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticFloatMethodA_(envpval, clazz, methodID, args);
        }
        public double CallStaticDoubleMethod(int clazz, int methodID, ulong[] args)
        {
            return CallStaticDoubleMethodA_(envpval, clazz, methodID, args);
        }
        public void CallStaticVoidMethod(int cls, int methodID, ulong[] args)
        {
            CallStaticVoidMethodA_(envpval, cls, methodID, args);
        }
        public int GetStaticFieldID( int clazz, string name, string sig)
        {
            IntPtr _name = NativeMemory.NewString(name);
            IntPtr _sig = NativeMemory.NewString(sig);
            int _r = GetStaticFieldID_(envpval, clazz, _name.ToInt32(), _sig.ToInt32());
            NativeMemory.Dispose(_sig);
            NativeMemory.Dispose(_name);
            return _r;
        }
        public int GetStaticObjectField( int clazz, int fieldID)
        {
            return GetStaticObjectField_(envpval, clazz, fieldID);
        }
        public bool GetStaticBooleanField( int clazz, int fieldID)
        {
            return GetStaticBooleanField_(envpval, clazz, fieldID);
        }
        public char GetStaticByteField( int clazz, int fieldID)
        {
            return GetStaticByteField_(envpval, clazz, fieldID);
        }
        public char GetStaticCharField( int clazz, int fieldID)
        {
            return GetStaticCharField_(envpval, clazz, fieldID);
        }
        public short GetStaticShortField( int clazz, int fieldID)
        {
            return GetStaticShortField_(envpval, clazz, fieldID);
        }
        public int GetStaticIntField( int clazz, int fieldID)
        {
            return GetStaticIntField_(envpval, clazz, fieldID);
        }
        public long GetStaticLongField( int clazz, int fieldID)
        {
            return GetStaticLongField_(envpval, clazz, fieldID);
        }
        public float GetStaticFloatField( int clazz, int fieldID)
        {
            return GetStaticFloatField_(envpval, clazz, fieldID);
        }
        public double GetStaticDoubleField( int clazz, int fieldID)
        {
            return GetStaticDoubleField_(envpval, clazz, fieldID);
        }
        public void SetStaticObjectField( int clazz, int fieldID, int value)
        {
            SetStaticObjectField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticBooleanField( int clazz, int fieldID, bool value)
        {
            SetStaticBooleanField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticByteField( int clazz, int fieldID, char value)
        {
            SetStaticByteField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticCharField( int clazz, int fieldID, char value)
        {
            SetStaticCharField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticShortField( int clazz, int fieldID, short value)
        {
            SetStaticShortField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticIntField( int clazz, int fieldID, int value)
        {
            SetStaticIntField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticLongField( int clazz, int fieldID, long value)
        {
            SetStaticLongField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticFloatField( int clazz, int fieldID, float value)
        {
            SetStaticFloatField_(envpval, clazz, fieldID, value);
        }
        public void SetStaticDoubleField( int clazz, int fieldID, double value)
        {
            SetStaticDoubleField_(envpval, clazz, fieldID, value);
        }
        public int NewString( string unicode, int len)
        {
            IntPtr _unicode = NativeMemory.NewString(unicode);
            int _r = NewString_(envpval, _unicode.ToInt32(), len);
            NativeMemory.Dispose(_unicode);
            return _r;
        }
        public int GetStringLength( int str)
        {
            return GetStringLength_(envpval, str);
        }
        public char GetStringChars( int str, bool[] isCopy)
        {
            return GetStringChars_(envpval, str, isCopy);
        }
        public void ReleaseStringChars( int str, string chars)
        {
            IntPtr _chars = NativeMemory.NewString(chars);
            ReleaseStringChars_(envpval, str, _chars.ToInt32());
            NativeMemory.Dispose(_chars);
        }
        public int NewStringUTF( string utf)
        {
            IntPtr _utf = NativeMemory.NewString(utf);
            int _r = NewStringUTF_(envpval, _utf.ToInt32());
            NativeMemory.Dispose(_utf);
            return _r;
        }
        public int GetStringUTFLength( int str)
        {
            return GetStringUTFLength_(envpval, str);
        }
        public string GetStringUTFChars( int str, bool[] isCopy)
        {
            return GetStringUTFChars_(envpval, str, isCopy);
        }
        public void ReleaseStringUTFChars( int str, string chars)
        {
            IntPtr _chars = NativeMemory.NewString(chars);
            ReleaseStringUTFChars_(envpval, str, _chars.ToInt32());
            NativeMemory.Dispose(_chars);
        }
        public int GetArrayLength( int array)
        {
            return GetArrayLength_(envpval, array);
        }
        public int NewObjectArray( int len, int clazz, int init)
        {
            return NewObjectArray_(envpval, len, clazz, init);
        }
        public int GetObjectArrayElement( int array, int index)
        {
            return GetObjectArrayElement_(envpval, array, index);
        }
        public void SetObjectArrayElement( int array, int index, int val)
        {
            SetObjectArrayElement_(envpval, array, index, val);
        }
        public int NewBooleanArray( int len)
        {
            return NewBooleanArray_(envpval, len);
        }
        public int NewByteArray( int len)
        {
            return NewByteArray_(envpval, len);
        }
        public int NewCharArray( int len)
        {
            return NewCharArray_(envpval, len);
        }
        public int NewShortArray( int len)
        {
            return NewShortArray_(envpval, len);
        }
        public int NewIntArray( int len)
        {
            return NewIntArray_(envpval, len);
        }
        public int NewLongArray( int len)
        {
            return NewLongArray_(envpval, len);
        }
        public int NewFloatArray( int len)
        {
            return NewFloatArray_(envpval, len);
        }
        public int NewDoubleArray( int len)
        {
            return NewDoubleArray_(envpval, len);
        }
        public bool GetBooleanArrayElements( int array, bool[] isCopy)
        {
            return GetBooleanArrayElements_(envpval, array, isCopy);
        }
        public char GetByteArrayElements( int array, bool[] isCopy)
        {
            return GetByteArrayElements_(envpval, array, isCopy);
        }
        public char GetCharArrayElements( int array, bool[] isCopy)
        {
            return GetCharArrayElements_(envpval, array, isCopy);
        }
        public short GetShortArrayElements( int array, bool[] isCopy)
        {
            return GetShortArrayElements_(envpval, array, isCopy);
        }
        public int GetIntArrayElements( int array, bool[] isCopy)
        {
            return GetIntArrayElements_(envpval, array, isCopy);
        }
        public long GetLongArrayElements( int array, bool[] isCopy)
        {
            return GetLongArrayElements_(envpval, array, isCopy);
        }
        public float GetFloatArrayElements( int array, bool[] isCopy)
        {
            return GetFloatArrayElements_(envpval, array, isCopy);
        }
        public double GetDoubleArrayElements( int array, bool[] isCopy)
        {
            return GetDoubleArrayElements_(envpval, array, isCopy);
        }
        public void ReleaseBooleanArrayElements( int array, bool[] elems, int mode)
        {
            ReleaseBooleanArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseByteArrayElements( int array, char[] elems, int mode)
        {
            ReleaseByteArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseCharArrayElements( int array, char[] elems, int mode)
        {
            ReleaseCharArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseShortArrayElements( int array, short[] elems, int mode)
        {
            ReleaseShortArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseIntArrayElements( int array, int[] elems, int mode)
        {
            ReleaseIntArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseLongArrayElements( int array, long[] elems, int mode)
        {
            ReleaseLongArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseFloatArrayElements( int array, float[] elems, int mode)
        {
            ReleaseFloatArrayElements_(envpval, array, elems, mode);
        }
        public void ReleaseDoubleArrayElements( int array, double[] elems, int mode)
        {
            ReleaseDoubleArrayElements_(envpval, array, elems, mode);
        }
        public void GetBooleanArrayRegion( int array, int start, int l, bool[] buf)
        {
            GetBooleanArrayRegion_(envpval, array, start, l, buf);
        }
        public void GetByteArrayRegion( int array, int start, int len, char[] buf)
        {
            GetByteArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetCharArrayRegion( int array, int start, int len, char[] buf)
        {
            GetCharArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetShortArrayRegion( int array, int start, int len, short[] buf)
        {
            GetShortArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetIntArrayRegion( int array, int start, int len, int[] buf)
        {
            GetIntArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetLongArrayRegion( int array, int start, int len, long[] buf)
        {
            GetLongArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetFloatArrayRegion( int array, int start, int len, float[] buf)
        {
            GetFloatArrayRegion_(envpval, array, start, len, buf);
        }
        public void GetDoubleArrayRegion( int array, int start, int len, double[] buf)
        {
            GetDoubleArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetBooleanArrayRegion( int array, int start, int l, bool[] buf)
        {
            SetBooleanArrayRegion_(envpval, array, start, l, buf);
        }
        public void SetByteArrayRegion( int array, int start, int len, byte[] buf)
        {
            char[] tmpbuf = new char[buf.Length];
            for (int i = 0; i < tmpbuf.Length; i++)
            {
                tmpbuf[i] = (char)buf[i];
            }
            SetByteArrayRegion_(envpval, array, start, len, tmpbuf);
        }
        public void SetCharArrayRegion( int array, int start, int len, char[] buf)
        {
            SetCharArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetShortArrayRegion( int array, int start, int len, short[] buf)
        {
            SetShortArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetIntArrayRegion( int array, int start, int len, int[] buf)
        {
            SetIntArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetLongArrayRegion( int array, int start, int len, long[] buf)
        {
            SetLongArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetFloatArrayRegion( int array, int start, int len, float[] buf)
        {
            SetFloatArrayRegion_(envpval, array, start, len, buf);
        }
        public void SetDoubleArrayRegion( int array, int start, int len, double[] buf)
        {
            SetDoubleArrayRegion_(envpval, array, start, len, buf);
        }
        public int RegisterNatives( int clazz, ref JNINativeMethod methods, int nMethods)
        {
            return RegisterNatives_(envpval, clazz,ref methods, nMethods);
        }
        public int UnregisterNatives( int clazz)
        {
            return UnregisterNatives_(envpval, clazz);
        }
        public int MonitorEnter( int obj)
        {
            return MonitorEnter_(envpval, obj);
        }
        public int MonitorExit( int obj)
        {
            return MonitorExit_(envpval, obj);
        }
        public int GetJavaVM( JavaVM[] vm)
        {
            return GetJavaVM_(envpval, vm);
        }
        public void GetStringRegion( int str, int start, int len, char[] buf)
        {
            GetStringRegion_(envpval, str, start, len, buf);
        }
        public void GetStringUTFRegion( int str, int start, int len, char[] buf)
        {
            GetStringUTFRegion_(envpval, str, start, len, buf);
        }
        public int GetPrimitiveArrayCritical( int array, bool[] isCopy)
        {
            return GetPrimitiveArrayCritical_(envpval, array, isCopy);
        }
        public void ReleasePrimitiveArrayCritical( int array, IntPtr[] carray, int mode)
        {
            ReleasePrimitiveArrayCritical_(envpval, array, carray, mode);
        }
        public string GetStringCritical( int str, bool[] isCopy)
        {
            return GetStringCritical_(envpval, str, isCopy);
        }
        public void ReleaseStringCritical( int str, string cstring)
        {
            IntPtr _cstring = NativeMemory.NewString(cstring);
            ReleaseStringCritical_(envpval, str, _cstring.ToInt32());
            NativeMemory.Dispose(_cstring);
        }
        public int NewWeakGlobalRef( int obj)
        {
            return NewWeakGlobalRef_(envpval, obj);
        }
        public void DeleteWeakGlobalRef( int ref0)
        {
            DeleteWeakGlobalRef_(envpval, ref0);
        }
        public bool ExceptionCheck()
        {
            return ExceptionCheck_(envpval);
        }
        public int NewDirectByteBuffer( int address, long capacity)
        {
            return NewDirectByteBuffer_(envpval, address, capacity);
        }
        public int GetDirectBufferAddress( int buf)
        {
            return GetDirectBufferAddress_(envpval, buf);
        }
        public long GetDirectBufferCapacity( int buf)
        {
            return GetDirectBufferCapacity_(envpval, buf);
        }
        public int GetObjectRefType( int obj)
        {
            return GetObjectRefType_(envpval, obj);
        }
    }
}
