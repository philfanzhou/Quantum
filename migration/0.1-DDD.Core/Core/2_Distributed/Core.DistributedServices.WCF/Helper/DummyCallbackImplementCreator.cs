using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.DistributedServices.WCF
{
    public class DummyCallbackImplementCreator
    {
        private static Dictionary<Type, object> dummyImpCached = new Dictionary<Type, object>();

        public static object Create(Type callbackType)
        {
            object instance = null;
            if (!dummyImpCached.ContainsKey(callbackType))
            {
                lock (dummyImpCached)
                {
                    if (!dummyImpCached.ContainsKey(callbackType))
                    {
                        var dummyImpType = GenerateDummyType(callbackType);
                        instance = Activator.CreateInstance(dummyImpType);
                        dummyImpCached.Add(callbackType, instance);
                    }
                }
            }

            instance = dummyImpCached[callbackType];

            return instance;
        }

        private static Type GenerateDummyType(Type callbackType)
        {
            AppDomain currentAppDomain = AppDomain.CurrentDomain;
            AssemblyName assyName = new AssemblyName("STEE.ACES.CADS.Service.DummmyCallback." + callbackType.Name);
            AssemblyBuilder assyBuilder = currentAppDomain.DefineDynamicAssembly(assyName, AssemblyBuilderAccess.Run);
            ModuleBuilder modBuilder = assyBuilder.DefineDynamicModule("ACES_Module_" + callbackType.Name);
            string newTypeName = "DummyCallbackImp_" + callbackType.Name;
            TypeAttributes newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;
            Type newTypeParent;
            Type[] newTypeInterfaces;

            if (callbackType.IsInterface)
            {
                newTypeParent = null;
                newTypeInterfaces = new Type[] { callbackType };
            }
            else
            {
                newTypeParent = callbackType;
                newTypeInterfaces = new Type[0];
            }

            TypeBuilder typeBuilder = modBuilder.DefineType(newTypeName, newTypeAttribute, newTypeParent, newTypeInterfaces);

            List<MethodInfo> targetMethods = new List<MethodInfo>();
            targetMethods.AddRange(GetMethodsRecursive(callbackType));

            foreach (MethodInfo targetMethod in targetMethods)
            {
                ParameterInfo[] paramInfo = targetMethod.GetParameters();
                Type[] paramType = new Type[paramInfo.Length];
                for (int i = 0; i < paramInfo.Length; i++)
                {
                    paramType[i] = paramInfo[i].ParameterType;
                }

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    targetMethod.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    targetMethod.ReturnType, 
                    paramType);
                ILGenerator generator = methodBuilder.GetILGenerator();
                generator.Emit(OpCodes.Ldstr);
                generator.Emit(OpCodes.Call);
                generator.Emit(OpCodes.Ret);
            }

            return typeBuilder.CreateType();
        }

        private static List<MethodInfo> GetMethodsRecursive(Type callbackType)
        {
            List<MethodInfo> targetMethods = new List<MethodInfo>();
            targetMethods.AddRange(callbackType.GetMethods());
            var interfaces = callbackType.GetInterfaces();
            
            foreach (var item in interfaces)
            {
                targetMethods.AddRange(GetMethodsRecursive(item));
            }

            return targetMethods;
        }
    }
}
