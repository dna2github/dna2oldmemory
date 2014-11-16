dna2oldmemory
=============

Migrate my old code from SourceForge

1. MobileASM (2009): A simple asm interpreter in J2ME

I developped this application with
Java(TM) ME platform SDK 3.0, EA.
And it is very simple software to
execute assembly as script. However,
I make it in a haste and the FLAGS
is confusing and I have not corrected
the error yet.

```
   // 1+2+3+...+100
   mov eax,0
   mov ecx,100
   add eax,ecx
   dec ecx
   ja  3
   print "1+2+3+...+100
   print eax
   end
```

2. ExpressionCalc (2011): A simple expression calculator lib in Java

I write it without using regex of Java
to build an own code interpreter. And
I use the CalcTree to do calculation
for a complicated expression and SymbolTable
supports to hold varibles.

```
   SymbolTableNode stn;
   SymbolTable st;
   CalcTree ct;
   LineCodeStream lcs;

   st = new SymbolTable();
   ct = new CalcTree();

   // declare variables
   stn = st.add("hello", null);
   stn.data = Double.valueOf(20.0);
   stn.type = 0;
   stn = st.add("world", null);
   stn.data = Double.valueOf(10.0);
   stn.type = 0;

   // declare functions
   stn = st.add("sqr", null);
   stn.data = Integer.valueOf(1); // function id
   stn.type = 1;
   stn = st.add("cube", null);
   stn.data = Integer.valueOf(2);
   stn.type = 1;
   stn = st.add("double", null);
   stn.data = Integer.valueOf(3);
   stn.type = 1;

   // try an expression
   lcs = new LineCodeStream(
      "hello+cube(world) - (world + 2)*sqr(2) - sqr(cube(double(2)))/hello");
   // TODO: proccess every token from lcs.getWord()
   // TODO: and add tokens to calculating tree
   double _val = 0.0;
   _val = calcTreeValue(ct.getRoot(),st);
   System.out.println(lcs.getLineCode() + " = " + _val);
```

  For example, in SyCoExp3in1.java:

  1+3.0-((((hello+f(x+5))*2+1.0)/3-3)+9)/4-3

  analysis:

  [ parent:(left | right) ]

  -:(-:(+:(1|3.0) | /:(+:(-:(/:(+:(*:(+:(hello | ,:(f,+:(x,5))) | 2) | 1.0) | 3) | 3) | 9) | 4) ) | 3)

           -
         /   \
        -     3
       / \
      +   /
     / \  | \
     1 3  +  4
         / \
        -   9
        |\
        / 3
        |\
        + 3
       / \
      *  1.0
      |\
      + 2
     / \
 hello  ,
       / \
      f   +
         / \
        x   5



3. JVCForCsharp (2012)

New Style:

```
   // load jvm
   JavaConnector jvc = new JavaConnector();
   // now options can be customized.
   // jvc.InitializeJVM(new string[] { "-Djava.compiler=NONE", "-Djava.class.path=.;test.jar;", "-verbose:NONE" });
   jvc.InitializeJVM("test.jar");
   // work with jvm
   Java java = jvc.Connect();
   /*
    *   package ljy.test;
    *   public class TestMain {
    *   	public static int x = 0;
    *   	public static int a, b, c;
    *   	public static float d;
    *   	public static void main(String[] args) {
    *   		x = Integer.valueOf(args[0]) + 4;
    *   	}
    *   	public static void test(int[] args, float extra) {
    *   		a = args[0]; b = args[1]; c = (int)extra; d = extra;
    *   		x = a + b + c;
    *   	}
    *   }
    */
   JavaClass TestMainClass = java.LoadClass("ljy.test.TestMain");

   JavaStaticMethod methodMain = TestMainClass.GetStaticMethod("void", "main", "java.lang.String[]");
   JavaStaticField mInt = TestMainClass.GetStaticField("int", "x");
   JavaArray strarr = new JavaArray(new string[] { "2" });
   methodMain.VoidInvoke(strarr);
   int mVal = mInt.GetIntValue(); // get 6

   JavaStaticMethod methodTest = TestMainClass.GetStaticMethod("void", "test", "int[]", "float");
   JavaArray intarr = new JavaArray(new int[] { 5, 6 });
   methodTest.VoidInvoke(intarr, 4.4f);

   JavaStaticField aInt = TestMainClass.GetStaticField("int", "a");
   JavaStaticField bInt = TestMainClass.GetStaticField("int", "b");
   JavaStaticField cInt = TestMainClass.GetStaticField("int", "c");
   JavaStaticField dFloat = TestMainClass.GetStaticField("float", "d");
   Console.WriteLine(string.Format("x = {0}, a = {1}, b = {2}, c = {3}, d = {4}",
      mInt.GetIntValue(), aInt.GetIntValue(), bInt.GetIntValue(), cInt.GetIntValue(), 
      dFloat.GetFloatValue())); // x = 15, a = 5, b = 6, c = 4, d = 4.4

   jvc.FinalizeJVM();
```

Old Style:

```
   // load jvm
   JavaInterface.JavaConnector jvc = new JavaInterface.JavaConnector();
   jvc.InitializeJVM("testjvm.jar");
   // work with jvm
   JavaInterface.JavaENV env;
   env = jvc.GetJavaENV();
   /*
    * package ljy.csharp.jvm;
    * class Hello {
    *    public static int m = 9;
    * }
    */
   int HelloClass = env.FindClass("ljy/csharp/jvm/Hello");
   int mInt = env.GetStaticFieldID(HelloClass, "m", "I");
   // mVal will get 9
   int mVal = env.GetStaticIntField(HelloClass, mInt);
   // unload jvm
   jvc.FinalizeJVM();

   // other examples:
   // create an Integer entity
   int intclass = env.FindClass("java/lang/Integer");
   int intinitmethod = env.GetMethodID(intclass, "<init>", "(I)V");
   int test = 5;
   IntPtr testptr = NativeMemory.NewIntObject(test);
   int intentity = env.NewObjectA(intclass, intinitmethod, testptr);

   // create a Date entity to get cureent time
   int dateclass = env.FindClass("java/util/Date");
   int dateinitmethod = env.GetMethodID(dateclass, "<init>", "()V");
   int dateentity = env.NewObject(dateclass, dateinitmethod, null);
   int date_gettime = env.GetMethodID(dateclass, "getTime", "()J");
   MessageBox.Show(string.Format("Time: {0}", 
      env.CallLongMethod(dateentity, date_gettime, null)));
```
