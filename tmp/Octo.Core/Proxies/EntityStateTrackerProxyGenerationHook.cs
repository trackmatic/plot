using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Octo.Core.Proxies
{
    public class EntityStateTrackerProxyGenerationHook : IProxyGenerationHook
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
