using System;
using Microsoft.Bot.Builder;

namespace HackedBrain.BotBuilder.State.Conditional
{
    public class ConditionalStatePropertyAccessorBuilder<T>
    {
        private readonly ConditionalStatePropertyAccessor<T> _conditionalStatePropertyAccessor = new ConditionalStatePropertyAccessor<T>();

        public ConditionalStatePropertyAccessorBuilder()
        {
        }

        public ConditionalStatePropertyAccessorBuilder<T> Default(IStatePropertyAccessor<T> defaultStatePropertyAccessor)
        {
            _conditionalStatePropertyAccessor.DefaultAccessor = defaultStatePropertyAccessor;

            return this;
        }

        public ConditionalStatePropertyAccessorBuilder<T> When(Func<ITurnContext, bool> condition, IStatePropertyAccessor<T> conditionalStatePropertyAccessor)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (conditionalStatePropertyAccessor == null)
            {
                throw new ArgumentNullException(nameof(conditionalStatePropertyAccessor));
            }

            _conditionalStatePropertyAccessor.ConditionalAccessors.Add((condition, conditionalStatePropertyAccessor));

            return this;
        }

        public IStatePropertyAccessor<T> Build()
        {
            if (_conditionalStatePropertyAccessor.DefaultAccessor == null
                        &&
                _conditionalStatePropertyAccessor.ConditionalAccessors.Count == 0)
            {
                throw new InvalidOperationException("No conditional accessors have been configured nor has any default accessor has been specified.");
            }

            return _conditionalStatePropertyAccessor;

        }
    }
}