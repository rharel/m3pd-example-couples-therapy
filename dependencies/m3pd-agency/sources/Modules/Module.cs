using rharel.Debug;
using rharel.M3PD.Agency.State;
using System;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// Modules are a way to isolate implementations of operations used in the 
    /// agency process from each other. Each module is responsible for the 
    /// execution of a single operation, with well-defined input and output.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// Indicates whether this module is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Gets the system's state accessor.
        /// </summary>
        public StateAccessor State { get; private set; }

        /// <summary>
        /// Initializes this module to reference the specified state.
        /// </summary>
        /// <param name="state">The state to reference.</param>
        /// <exception cref="InvalidOperationException">
        /// When initializing an already initialized module.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="state"/> is null.
        /// </exception>
        internal void Initialize(StateAccessor state)
        {
            Require.IsFalse<InvalidOperationException>(IsInitialized);
            Require.IsNotNull(state);

            State = state;
            IsInitialized = true;

            Setup();
        }

        /// <summary>
        /// Sets up this module for operation.
        /// </summary>
        /// <remarks>
        /// This is called once during module initialization.
        /// </remarks>
        public virtual void Setup() { }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Module)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
