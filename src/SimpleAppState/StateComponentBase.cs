using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace SimpleAppState
{
    /// <summary>
    /// <para>Inherits the <see cref="ComponentBase"/> class.</para>
    /// <para>Auto-subscribes injected properties that implements <see cref="IState"/>.</para>
    /// </summary>
    public abstract class StateComponentBase : ComponentBase, IDisposable
    {
        private bool _disposed;
        private IEnumerable<PropertyInfo>? _properties;

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttributes<InjectAttribute>().Any()) ?? Enumerable.Empty<PropertyInfo>();
            foreach (var property in _properties)
            {
                var value = property.GetValue(this);
                if (value is IState state)
                {
                    state.StateChanged += StateHasChanged;
                }
            }
        }

        /// <summary>
        /// <para>Unsubscribes states when called.</para>
        /// <para>Make sure to call the base implementation if you intend to override this to retain original function.</para>
        /// <example>
        /// <para>Example:</para>
        /// <code>
        /// base.Dispose(<paramref name="disposing"/>);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var property in _properties!)
                {
                    var value = property.GetValue(this);
                    if (value is IState state)
                    {
                        state.StateChanged -= StateHasChanged;
                    }
                }
            }

            _properties = null;

            _disposed = true;
        }

        /// <summary>
        /// Unsubscribes states when called.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}