using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Plot.Proxies
{
    public class ProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {

        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return;
            }

            throw new InvalidOperationException($"The method {memberInfo.Name} must be marked as virtual");
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return true;
        }
    }
}
