using System;
using Castle.DynamicProxy;

namespace Plot.Proxies
{
    public class EntityStateInterceptor : IInterceptor
    {
        private readonly IEntityStateCache _state;

        public EntityStateInterceptor(IEntityStateCache state)
        {
            _state = state;
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsSetter(invocation))
            {
                _state.Get(invocation.InvocationTarget).Dirty();
            }

            if (invocation.InvocationTarget == null)
            {
                return;
            }

            invocation.Proceed();
        }

        private bool IsSetter(IInvocation invocation)
        {
            return invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && _state.Contains(invocation.InvocationTarget);
        }
    }
}
