/*
 * @Author: xiongtianya 
 * @Date: 2023-01-12 16:58:53 
 * @Last Modified by: xiongtianya
 * @Last Modified time: 2023-01-12 16:59:30
 */

namespace XGameFramework
{
    public abstract class BaseEventArgs : GameFrameworkEventArgs
    {

        public abstract int Id
        {
            get;
        }
    }
}