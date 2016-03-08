using System;
using Castle.DynamicProxy;

namespace Plot.Proxies
{
    public class EntityStateInterceptor : IInterceptor
    {
        private readonly IEntityStateCache _entityStateCache;

        public EntityStateInterceptor(IEntityStateCache entityStateCache)
        {
            _entityStateCache = entityStateCache;
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsSetter(invocation))
            {
                _entityStateCache.Get(invocation.InvocationTarget).Dirty();
            }

            if (invocation.InvocationTarget == null)
            {
                return;
            }

            invocation.Proceed();
        }

        private bool IsSetter(IInvocation invocation)
        {
            return invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && _entityStateCache.Contains(invocation.InvocationTarget);
        }
    }
}
