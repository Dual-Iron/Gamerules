using System;

namespace Gamerules.Rules.Builders
{
    /// <summary>
    /// Base class for rule builders.
    /// </summary>
    /// <typeparam name="T">Value type of the rule.</typeparam>
    /// <typeparam name="TRule">The type of rule this builder constructs.</typeparam>
    /// <typeparam name="TBuilder">The implementing class's type.</typeparam>
    public abstract class RuleBuilder<TBuilder, TRule, T> where TRule : notnull, IRule where T : notnull where TBuilder : RuleBuilder<TBuilder, TRule, T>
    {
        private bool registered;

        /// <summary>
        /// Rule's default value.
        /// </summary>
        protected T? defaultValue;

        /// <summary>
        /// Rule's description.
        /// </summary>
        protected string description = "";

        /// <summary>
        /// Callback for changing value.
        /// </summary>
        protected UpdateValueDelegate<T>? onUpdate;

        /// <inheritdoc/>
        protected RuleBuilder() { }

        /// <summary>
        /// Sets the rule's description.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public TBuilder Description(string description)
        {
            this.description = description ?? throw new ArgumentNullException(nameof(description));
            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the rule's default value.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public TBuilder Default(T value)
        {
            defaultValue = value;
            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the rule's callback for when its value is updated.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public TBuilder OnUpdate(UpdateValueDelegate<T> onUpdateValue)
        {
            onUpdate = onUpdateValue;
            return (TBuilder)this;
        }

        /// <summary>
        /// Registers the rule and disposes of this builder instance.
        /// </summary>
        /// <param name="id">The ID to register this rule under. Can only contain a-z, 0-9, forward slash, and underscore.</param>
        public TRule Register(string id)
        {
            if (registered) throw new InvalidOperationException("Builder instance already registered a rule.");
            registered = true;
            var rule = GetRule(id);
            RuleAPI.Register(id, rule);
            return rule;
        }

        /// <summary>
        /// Should construct and return a gamerule instance.
        /// </summary>
        protected abstract TRule GetRule(string id);
    }
}
