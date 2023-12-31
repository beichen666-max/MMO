using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network
{

    class Msg
    {
        public NetConnection sender;
        public Proto.Package message;
    }

    // 消息转发器
    public class MessageRouter : Singleton<MessageRouter>
    {
        
        int ThreadCount = 1;  // 工作线程数
        int WorkerCount = 0;  // 正在工作的线程数
        bool Running = false;  // 是否在运行
        AutoResetEvent threadEvent = new AutoResetEvent(true); // 通过set每次可以唤醒一个线程
        
        // 消息队列，所有的客户端发来的消息都暂存在这里
        private Queue<Msg> messageQueue = new Queue<Msg>();

        // 消息处理器
        public delegate void MessageHandler<T>(NetConnection sender,T msg);
        // 消息频道字典
        private Dictionary<string, Delegate> delegateMap = new Dictionary<string, Delegate>();

        // 订阅
        public void On<T>(MessageHandler<T> handler) where T : Google.Protobuf.IMessage
        {
            string type = typeof(T).Name;
            if(!delegateMap.ContainsKey(type))
            {
                delegateMap[type] = null;
            }
            delegateMap[type] = (MessageHandler<T>)delegateMap[type] + handler;
        }

        // 退订
        public void Off<T>(MessageHandler<T> handler) where T : Google.Protobuf.IMessage
        {
            string type = typeof(T).Name;
            if(!delegateMap.ContainsKey(type))
            {
                delegateMap[type] = null;
            }
            delegateMap[type] = (MessageHandler<T>)delegateMap[type] - handler;
        }

        // 添加新的消息到队列的方法
        // sender 消息发送者
        // message 消息对象
        public void AddMessage(NetConnection sender, Proto.Package message)
        {
            messageQueue.Enqueue(new Msg() {sender= sender,message = message});
            Console.WriteLine(messageQueue.Count);
            threadEvent.Set();  // 唤醒一个worker
        } 

        // 启动分发器
        public void Start(int ThreadCount)
        {
            Running = true;
            this.ThreadCount = Math.Max(ThreadCount,1);
            this.ThreadCount = Math.Min(ThreadCount,200);
            for(int i = 0; i < this.ThreadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MessageWork));
            }
        }

        //停止分发器
        public void Stop()
        {
            Running = false;
            // 任务队列也要清空
            messageQueue.Clear();
            // 多线程防止不同步的问题 等待所有程序下线
            while (WorkerCount > 0)
            {
                threadEvent.Set();
            }
            Thread.Sleep(50); // 线程停30秒
        }
        private void MessageWork(object? state)
        {
            Console.WriteLine("worker thread start");
            try
            {
                WorkerCount = Interlocked.Increment(ref WorkerCount); 
                while (Running)
                {
                    if(messageQueue.Count == 0)
                    {
                        threadEvent.WaitOne(); // 可通过Set()唤醒
                        continue; // 被唤醒之后还要进行下一次判断，不一定可靠，相当于抢货，再来一次
                    }
                    // 从消息队列取出一个元素
                    Msg msg = messageQueue.Dequeue();
                    Proto.Package package = msg.message;
                    if(package != null)
                    {
                        if(package.Request != null)
                        {
                            if(package.Request.UserRegister != null){
                                Console.WriteLine("用户注册" + package.Request.UserRegister.Username);
                            }
                        }
                        if(package.Response.UserLogin != null)
                        {
                                Console.WriteLine("用户登录" + package.Request.UserLogin.Username);
                        }
                    }
                }
            }
            catch
            { }
            finally
            {
                WorkerCount = Interlocked.Decrement(ref WorkerCount); 
            }
            Console.WriteLine("worker thread end");
        }
    }
}
