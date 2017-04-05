using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.IOC
{
    public class Locator
    {
        private static readonly Dictionary<Type, Type> _RegisteredTypeDictionary = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, object> _RegisteredObjectDictionary = new Dictionary<Type, object>();
        private static readonly object[] _emptyArguments = new object[0];
        private static readonly object _syncLock = new object();

        private static object Resolve(Type type, bool createNew)
        {
            lock (_syncLock)
            {
                if (!_RegisteredObjectDictionary.ContainsKey(type) || createNew)
                {
                    if (!_RegisteredTypeDictionary.ContainsKey(type))
                    {
                        throw new Exception("Type not registered.");
                    }


                    var resolveTo = _RegisteredTypeDictionary[type] ?? type;
                    _RegisteredObjectDictionary[type] = Activator.CreateInstance(resolveTo);

                }
                return _RegisteredObjectDictionary[type];
            }
        }



        public static T Get<T>(bool createIfNotFound = false)
        {
            Type type = typeof(T);
            if (!_RegisteredTypeDictionary.ContainsKey(type))
            {
                throw new Exception("Type not registered.");
            }

            if (_RegisteredObjectDictionary.ContainsKey(type))
            {
                return (T)_RegisteredObjectDictionary[type];
            }

            if (createIfNotFound)
            {
                Resolve(type, createIfNotFound);
                return (T)_RegisteredObjectDictionary[type];
            }
            return default(T);
        }

        public static T GetNewInstance<T>()
        {
            Type type = typeof(T);
            Resolve(type, true);
            return (T)_RegisteredObjectDictionary[type];
        }

        public static void Register<T>(T createdInstance)
        {
            lock (_syncLock)
            {
                if (_RegisteredTypeDictionary.ContainsKey(typeof(T))) return;
                _RegisteredTypeDictionary.Add(typeof(T), null);
                _RegisteredObjectDictionary.Add(typeof(T), createdInstance);
            }
        }

        /// <summary>
        /// Used to register an interface key, that has different implementations for different platforms.
        /// </summary>
        /// <typeparam name="I">Interface type</typeparam>
        /// <typeparam name="O">Concrete type</typeparam>
        public static void Register<I, O>()
        {
            lock (_syncLock)
            {
                _RegisteredTypeDictionary.Add(typeof(I), typeof(O));
            }
        }

        // Register imnplementation without an interface
        public static void Register<C>(bool createImmediately) where C : class
        {
            lock (_syncLock)
            {
                _RegisteredTypeDictionary.Add(typeof(C), null);
                if (createImmediately)
                {
                    Resolve(typeof(C), createImmediately);
                }
            }
        }

        public static void ClearCacheForTests()
        {
            _RegisteredObjectDictionary.Clear();
            _RegisteredTypeDictionary.Clear();
        }

        public static void Add<T>(object o)
        {
            Type type = typeof(T);
            if (!_RegisteredTypeDictionary.Keys.Contains(type))
            {
                _RegisteredTypeDictionary.Add(type, type);
            }

            if (_RegisteredObjectDictionary.ContainsKey(type))
            {
                _RegisteredObjectDictionary[type] = o;
                return;
            }
            _RegisteredObjectDictionary.Add(type, o);
        }

    }
}
