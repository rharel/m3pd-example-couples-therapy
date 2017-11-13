using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.System;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.DesignPatterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rharel.M3PD.Agency.State
{
    /// <summary>
    /// The information state is an object that is shared among all operation
    /// modules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The information state is to be used as a container for any data that is 
    /// read or written by more than one module.
    /// </para>
    /// <para>
    /// The state organizes its data into "components". Each component is 
    /// associated with a type and an identifier. Which components are 
    /// available on a given state object is specified during construction, and 
    /// can be queried through the <see cref="ComponentIDs"/> property.
    /// </para>
    /// <para>
    /// Components identifiers prefixed with an _underscore should be 
    /// considered reserved.
    /// </para>
    /// </remarks>
    public sealed class InformationState: StateAccessor, StateMutator
    {
        /// <summary>
        /// Builds instances of <see cref="InformationState"/>. 
        /// </summary>
        public sealed class Builder: ObjectBuilder<InformationState>
        {
            /// <summary>
            /// Specifies that the state should include a component for the 
            /// specified data type and with the specified identifier.
            /// </summary>
            /// <typeparam name="T">
            /// The type of the component's value.
            /// </typeparam>
            /// <param name="id">The component's identifier.</param>
            /// <param name="initial_value">
            /// The component's initial value.
            /// </param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When either <paramref name="id"/> or 
            /// <paramref name="initial_value"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="id"/> is blank or already taken.
            /// </exception>
            public Builder WithComponent<T>(string id, T initial_value)
            {
                if (IsBuilt)
                {
                    throw new InvalidOperationException(
                        "Object is already built."
                    );
                }
                if (id == null)
                {
                    throw new ArgumentNullException(nameof(id));
                }
                if (id.Trim().Length == 0 || 
                    components.ContainsKey(id))
                {
                    throw new ArgumentException(nameof(id));
                }

                components.Add(id, new StateComponent<T>(initial_value));

                return this;
            }

            /// <summary>
            /// Creates the object.
            /// </summary>
            /// <returns>
            /// The built object.
            /// </returns>
            protected override InformationState CreateObject()
            {
                return new InformationState(components);
            }

            private readonly Dictionary<string, StateComponent> components = ( 
                new Dictionary<string, StateComponent>()
            );
        }

        /// <summary>
        /// Creates a new state with the specified components.
        /// </summary>
        /// <param name="components">The components to hold.</param>
        private InformationState(Dictionary<string, StateComponent> components)
        {
            _components = components;

            ComponentIDs = new CollectionView<string>(_components.Keys);
        }

        /// <summary>
        /// The output of <see cref="RAPModule"/>.
        /// </summary>
        /// <remarks>Set by the <see cref="AgencySystem"/>.</remarks>
        public RecentActivity RecentActivity { get; internal set; }
        /// <summary>
        /// The output of <see cref="CAPModule"/>.
        /// </summary>
        /// <remarks>Set by the <see cref="AgencySystem"/>.</remarks>
        public CurrentActivity CurrentActivity { get; internal set; }
        /// <summary>
        /// The system's own activity status.
        /// </summary>
        /// <remarks>Set by the <see cref="AgencySystem"/>.</remarks>
        public SystemActivity SystemActivity { get; internal set; }
        
        /// <summary>
        /// Gets supported component identifiers.
        /// </summary>
        public ImmutableCollection<string> ComponentIDs { get; private set; }

        /// <summary>
        /// Gets the component corresponding to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <param name="id">The component's identifier.</param>
        /// <returns>
        /// The component corresponding to the specified type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is not supported or when 
        /// <typeparamref name="T"/> is not compatible with the type of 
        /// component associated with <paramref name="id"/>.
        /// </exception>
        public T Get<T>(string id)
        {
            if (!_components.ContainsKey(id))
            {
                throw new ArgumentException(nameof(id));
            }

            var component = _components[id];
            if (component.Type != typeof(T))
            {
                throw new ArgumentException(nameof(T));
            }

            return (T)component.Value;
        }
        /// <summary>
        /// Sets the component corresponding to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of component to set.</typeparam>
        /// <param name="id">The component's identifier.</param>
        /// <param name="value">The value to assign.</param>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is not supported or when
        /// <typeparamref name="T"/> is not compatible with the type of 
        /// component associated with <paramref name="id"/>.
        /// </exception>
        public void Set<T>(string id, T value)
        {
            if (!_components.ContainsKey(id))
            {
                throw new ArgumentException(nameof(id));
            }
            if (_components[id].Type != typeof(T))
            {
                throw new ArgumentException(nameof(T));
            }
            _components[id] = new StateComponent<T>(value);
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A human-readable string.
        /// </returns>
        public override string ToString()
        {
            string ids = string.Join(", ", ComponentIDs.ToArray());

            return $"{nameof(InformationState)}{{ " +
                   $"{nameof(ComponentIDs)} = [{ids}] }}";
        }

        private readonly Dictionary<string, StateComponent> _components;
    }
}
