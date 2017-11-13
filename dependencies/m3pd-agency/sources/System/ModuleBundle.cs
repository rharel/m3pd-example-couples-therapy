using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.DesignPatterns;
using System;
using System.Collections;
using System.Collections.Generic;

namespace rharel.M3PD.Agency.System
{
    /// <summary>
    /// Represents a bundle of modules that may be used together to form a 
    /// complete agency system.
    /// </summary>
    public struct ModuleBundle: IEnumerable<Module>
    {
        /// <summary>
        /// Builds instances of <see cref="ModuleBundle"/>.
        /// </summary>
        public sealed class Builder: ObjectBuilder<ModuleBundle>
        {
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// recent activity perception.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithRecentActivityPerceptionBy(RAPModule module)
            {
                ValidateModule(module);

                _RAP = module;

                return this;
            }
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// current activity perception.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithCurrentActivityPerceptionBy(CAPModule module)
            {
                ValidateModule(module);

                _CAP = module;

                return this;
            }
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// infomation state update.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithStateUpdateBy(SUModule module)
            {
                ValidateModule(module);

                _SU = module;

                return this;
            }
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// action selection.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithActionSelectionBy(ASModule module)
            {
                ValidateModule(module);

                _AS = module;

                return this;
            }
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// action timing.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithActionTimingBy(ATModule module)
            {
                ValidateModule(module);

                _AT = module;

                return this;
            }
            /// <summary>
            /// Assigns the specified module to be the one responsible for 
            /// action realization.
            /// </summary>
            /// <param name="module">The module to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            public Builder WithActionRealizationBy(ARModule module)
            {
                ValidateModule(module);

                _AR = module;

                return this;
            }

            /// <summary>
            /// Creates the object.
            /// </summary>
            /// <returns>The built object.</returns>
            protected override ModuleBundle CreateObject()
            {
                return new ModuleBundle(_RAP, _CAP, _SU, _AS, _AT, _AR);
            }

            /// <summary>
            /// Validates the specified module for inclusion in the bundle.
            /// </summary>
            /// <param name="module">The module to validate.</param>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="module"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="module"/> has already been initialized.
            /// </exception>
            private void ValidateModule(Module module)
            {
                if (IsBuilt)
                {
                    throw new InvalidOperationException(
                        "Object is already built."
                    );
                }
                if (module == null)
                {
                    throw new ArgumentNullException(nameof(module));
                }
                if (module.IsInitialized)
                {
                    throw new ArgumentException(nameof(module));
                }
            }

            private RAPModule _RAP = new RAPStub();
            private CAPModule _CAP = new CAPStub();
            private SUModule _SU = new SUStub();
            private ASModule _AS = new ASStub();
            private ATModule _AT = new ATStub();
            private ARModule _AR = new ARStub();
        }

        /// <summary>
        /// Creates a new bundle from the specified modules.
        /// </summary>
        /// <param name="RAP">
        /// The recent activity perception module.
        /// </param>
        /// <param name="CAP">
        /// The current activity perception module.
        /// </param>
        /// <param name="SU">The information state upadte module.</param>
        /// <param name="AS">The action_selection module.</param>
        /// <param name="AT">The action timing module.</param>
        /// <param name="AR">The action realization module.</param>
        private ModuleBundle(
            RAPModule RAP, CAPModule CAP,
            SUModule  SU,  ASModule  AS,
            ATModule  AT,  ARModule  AR)
        {
            RecentActivityPerception = RAP;
            CurrentActivityPerception = CAP;
            StateUpdate = SU;
            ActionSelection = AS;
            ActionTiming = AT;
            ActionRealization = AR;
        }

        /// <summary>
        /// Gets the recent activity perception module.
        /// </summary>
        public RAPModule RecentActivityPerception { get; private set; }
        /// <summary>
        /// Gets the current activity perception module.
        /// </summary>
        public CAPModule CurrentActivityPerception { get; private set; }
        /// <summary>
        /// Gets the state update module.
        /// </summary>
        public SUModule StateUpdate { get; private set; }
        /// <summary>
        /// Gets the action selection module.
        /// </summary>
        public ASModule ActionSelection { get; private set; }
        /// <summary>
        /// Gets the action timing module.
        /// </summary>
        public ATModule ActionTiming { get; private set; }
        /// <summary>
        /// Gets the action realization module.
        /// </summary>
        public ARModule ActionRealization { get; private set; }

        /// <summary>
        /// Enumerates the modules contained in this bundle.
        /// </summary>
        /// <returns>An enumerator of modules.</returns>
        public IEnumerator<Module> GetEnumerator()
        {
            yield return RecentActivityPerception;
            yield return CurrentActivityPerception;
            yield return StateUpdate;
            yield return ActionSelection;
            yield return ActionTiming;
            yield return ActionRealization;
        }
        /// <summary>
        /// Enumerates the modules contained in this bundle.
        /// </summary>
        /// <returns>An enumerator of modules.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ModuleBundle)}{{ " +

                   $"{nameof(RecentActivityPerception)} = " +
                   $"{RecentActivityPerception}, " +

                   $"{nameof(CurrentActivityPerception)} = " +
                   $"{CurrentActivityPerception}, " +

                   $"{nameof(StateUpdate)} = {StateUpdate}, " +
                   $"{nameof(ActionSelection)} = {ActionSelection}, " +
                   $"{nameof(ActionTiming)} = {ActionTiming}, " +
                   $"{nameof(ActionRealization)} = {ActionRealization} }}";
        }
    }
}
