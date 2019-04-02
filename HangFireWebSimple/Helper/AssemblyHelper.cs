using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HangFireWebSimple.Helper
{
    /// <summary>
    /// 反射DLL方法Helper
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// 反射Action方法
        /// 无参无返回值
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="dLLPath">dll完整路径</param>
        /// <param name="nameSpaceName">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名称</param>
        public static void DLLAction(out string errMsg,string dLLPath,string nameSpaceName,string className,string methodName)
        {
            try
            {
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(dLLPath);
                //获取类型，参数（名称空间+类）   
                Type type = assembly.GetType($"{nameSpaceName}.{className}");
                //创建该对象的实例，object类型，参数（名称空间+类）   
                object instance = assembly.CreateInstance($"{nameSpaceName}.{className}");
                //执行方法   
                object value = type.GetMethod(methodName).Invoke(instance, null);
                errMsg = "";
            }
            catch (Exception err)
            {
                errMsg = err.Message;
            }

        }
        /// <summary>
        /// 反射Func方法
        /// 无参有返回值
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="dLLPath">Dll完整路径</param>
        /// <param name="nameSpaceName">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public static object DLLFunc(out string errMsg,string dLLPath, string nameSpaceName, string className,string methodName)
        {
            try
            {            
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(dLLPath);
                //获取类型，参数（名称空间+类）   
                Type type = assembly.GetType($"{nameSpaceName}.{className}");
                //创建该对象的实例，object类型，参数（名称空间+类）   
                object instance = assembly.CreateInstance($"{nameSpaceName}.{className}");
                //执行无参方法   
                object value = type.GetMethod(methodName,new Type[] { }).Invoke(instance, null);
                errMsg = "";
                return value;

            }
            catch (Exception err)
            {
                errMsg = err.Message;
                return null;
            }

        }
        /// <summary>
        /// 反射Func方法
        /// 有参有返回值
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="dLLPath">dll完整路径</param>
        /// <param name="nameSpaceName">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paramType">方法参数类型</param>
        /// <param name="methodParams">方法参数</param>
        /// <returns></returns>
        public static object DLLFuncWithParam(out string errMsg,string dLLPath, string nameSpaceName, string className,string methodName,Type[] paramType, params object[] methodParams)
        {
            try
            {
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(dLLPath);
                //获取类型，参数（名称空间+类）   
                Type type = assembly.GetType($"{nameSpaceName}.{className}");
                //创建该对象的实例，object类型，参数（名称空间+类）   
                object instance = assembly.CreateInstance($"{nameSpaceName}.{className}");
                //执行方法   
                object value = type.GetMethod(methodName, paramType).Invoke(instance, methodParams);
                errMsg = "";
                return value;
            }
            catch (Exception err)
            {
                errMsg = err.Message;
                return null;
            }

        }
    }
}