using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    // 单例模式基础类 T 泛型
    // where T : new() 参数类型约束，T必须有一个无参构造函数
    public class Singleton<T> where T : new()
    {
        // 全局唯一 使用静态属性，并且不希望被外界访问
        // 问号代表可空类型
        private static T? instance;
        // 供外界访问
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
