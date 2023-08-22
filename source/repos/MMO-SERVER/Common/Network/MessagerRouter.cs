using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network
{

    class MsgUnit
    {
        public NetConnection sender;
        public Google.Protobuf.IMessage message;
    }

    // 消息转发器
    public class MessageRouter : Singleton<MessageRouter>
    {
        
        int ThreadCount = 1;  // 工作线程数
        int WorkerCount = 0;  // 正在工作的线程数
        bool Running = false;  // 是否在运行
        
        // 消息队列，所有的客户端发来的消息都暂存在这里
        private Queue<MsgUnit> messageQueue = new Queue<MsgUnit>();

        // 消息处理器
        public delegate void MessageHandler<T>(NetConnection sender,T msg);
        // 消息频道字典
        private Dictionary<string, Delegate> delegateMap = new Dictionary<string, Delegate>();

        // 订阅
        public void On<T>(MessageHandler<T> handler) where T : Google.Protobuf.IMessage;
        {
            string = typeof(T).Name;
            if(!delegateMap.ContainsKey(type))
            {
                delegateMap[type] = null;
            }
            delegateMap[type] = (MessageHandler<T>)delegateMap[type] + handler;
        }

        // 退订
        public void Off<T>(MessageHandler<T> handler) where T : Google.Protobuf.IMessage;
        {
            string type = typeof(T).Name;
            if(!delegateMap.ContainsKey(type))
            {
                delegateMap[type] = null;
            }
            delegateMap[type] = (MessageHandler<T>)delegateMap[type] - handler;
        }

        // 添加新的消息到队列
        // sender 消息发送者
        // message 消息对象
        public void AddMessage(NetConnection sender,Google.Protobuf.IMessage message)
        {
            messageQueue.Enqueue(new MsgUnit() {sender= sender,message = message});
            Console.WriteLine(messageQueue.Count);
        } 

        public void Start(int ThreadCount)
        {
            Running = true;
            this.ThreadCount = Math.Max(ThreadCount,1);
            this.ThreadCount = Math.Min(ThreadCount,200);
            for(int i = 0; i < this.ThreadCount; i++)
            {
                ThreadPool.QueueUserWprkItem(new WaitCallback(MessageWork));
            }
        }

        public void Stop()
        {
            Running = false;
            messageQueue.Clear();
            while (WorkerCount > 0)
            {
                Thread.Sleep(30);
            }
        }
        private void MessageWork(object? state)
        {
            Console.WriteLine("worker thread start")
            try
            {
                WorkerCount = Interlocked.Increment(ref WorkerCount); 
                while (Running)
                {
                    messageQueue
                }
            }
            catch
            { }
            finally
            {
                WorkerCount = Interlocked.Decrement(ref WorkerCount); 
            }
            Console.WriteLine("worker thread end")
        }
    }
}
