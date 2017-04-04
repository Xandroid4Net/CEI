using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.IOC.Bus
{
    public class EventBus
    {
        protected static Dictionary<Type, Action<object>> handlers = new Dictionary<Type, Action<object>>();

        public static void Subscribe(Type type, Action<object> handler)
        {
            if (handlers.ContainsKey(type))
            {
                handlers[type] = handler;
                return;
            }
            handlers.Add(type, handler);
        }

        public static void UnSubscribe(Type type)
        {
            handlers.Remove(type);
        }

        public static void Publish(object e)
        {
            Publish(null, e);
        }

        private static void Publish(Type type, object e)
        {
            if (type == null)
            {
                foreach (KeyValuePair<Type, Action<object>> kvp in handlers)
                {
                    kvp.Value(e);
                }
                return;
            }
            if (!handlers.ContainsKey(type)) return;
            handlers[type](e);
        }

        public static void PublishError(Exception e)
        {
            Publish(null, e);
        }
    }
}
