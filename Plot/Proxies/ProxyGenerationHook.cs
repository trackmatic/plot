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
            throw new InvalidOperationException($"The method {memberInfo.Name} must be marked as virtual");
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return true;
        }
    }
}
