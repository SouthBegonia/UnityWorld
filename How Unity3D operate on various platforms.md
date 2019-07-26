> Unity3D的跨平台原理核心在于对指令集[CIL(通用中间语言)](das)的应用。

## 机理
首先需要知道，Unity中的Mono是基于**通用语言框架CLI**和C#的ECMA标准实现的，与微软的.NET框架有着诸多类似之处，因此分析Unity的跨平台性，本质即为分析.NET框架下C#语言从编译到运行的过程。首先抛出几个重要概念：
- **CIL**：属于通用语言架构和.NET 框架的低阶编程语言，完全基于堆栈，**运行在CLR上**。
- **CLR**：**公共语言运行库**，和Java虚拟机一样也是一个运行时环境，可由面向CLR的所有语言使用，是.NET Framework的主要执行引擎。CIL在CLR内的存在形式为IL(中间语言)。
- **JIT**：**即时编译**，在程序执行时才编译代码，逐条将IL语句翻译为及其语句，然后执行。

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190726122812184-2124255795.jpg)

**机理概述**：在编译.NET编程语言如C#时，源代码首先被Unity中Mono内的C#编译器编译成CIL码，经过CLR中JIT编译器将其翻译为本地代码，最终运行于操作系统上。因此Unity可在众多支持.NET框架的平台上工作。


## 其他
- **托管代码**：由公共语言运行库环境CLR执行的代码，由多种支持.NET的语言编写而来，实质是中间代码。例如本例C#编写的代码被编译为CIL，运行于CLR中的形式为IL，故IL既是中间语言，又是托管代码

## 参考
- [C#、.NET Framework、CLR的关系](https://blog.csdn.net/lidandan2016/article/details/77868043)
- [Unity3D学习：简单梳理下Unity跨平台的机制原理](https://www.cnblogs.com/0kk470/p/7468054.html)
- [Unity将来时：IL2CPP是什么？](https://zhuanlan.zhihu.com/p/19972689)
- [托管代码与非托管代码](https://www.cnblogs.com/ykgbk/p/7771238.html)
- 《Unity3D脚本编程》- 陈嘉栋