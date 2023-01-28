/*
 * @Author: xiongtianya 
 * @Date: 2023-01-12 17:00:51 
 * @Last Modified by: xiongtianya
 * @Last Modified time: 2023-01-12 17:27:58
 */

using System;
namespace XGameFramework
{
    internal enum EventPoolMode : byte
    {
        /// <summary>
        /// 默认事件池模式，即必须存在有且只有一个事件处理函数。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 允许不存在事件处理函数。
        /// </summary>
        AllowNoHandler = 1,

        /// <summary>
        /// 允许存在多个事件处理函数。
        /// </summary>
        AllowMultiHandler = 2,

        /// <summary>
        /// 允许存在重复的事件处理函数。
        /// </summary>
        AllowDuplicateHandler = 4
    }
}