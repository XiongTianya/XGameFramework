/*
 * @Author: xiongtianya 
 * @Date: 2023-01-12 16:54:08 
 * @Last Modified by: xiongtianya
 * @Last Modified time: 2023-01-12 16:56:29
 */

using System;
namespace XGameFramework
{
    public abstract class GameFrameworkEventArgs : EventArgs, IReference
    {
        public GameFrameworkEventArgs()
        {

        }

        public abstract void Clear();
    }
}