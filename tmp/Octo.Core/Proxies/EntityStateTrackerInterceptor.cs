using System;
using Castle.DynamicProxy;

namespace Octo.Core.Proxies
{
    public class EntityStateTrackerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && EntityStateTracker.Contains(invocation.InvocationTarget))
            {
                EntityStateTracker.Get(invocation.InvocationTarget).Dirty();
            }

            invocation.Proceed();
        }
    }
}
