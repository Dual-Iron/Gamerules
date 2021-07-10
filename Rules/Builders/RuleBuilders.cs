﻿using System;

namespace Gamerules.Rules.Builders
{
    /// <summary>
    /// Builds rules with boolean values.
    /// </summary>
    public class BoolRuleBuilder : RuleBuilder<BoolRuleBuilder, bool>
    {
        /// <inheritdoc/>
        protected override void DoRegister(string id, bool defaultValue)
        {
            var ret = new BoolRule(defaultValue) { Description = description };
            ret.OnUpdateValue += onUpdate;
            ret.Register(id);
        }
    }

    /// <summary>
    /// Builds rules with enum values.
    /// </summary>
    public class EnumRuleBuilder<T> : RuleBuilder<EnumRuleBuilder<T>, T> where T : Enum
    {
        /// <inheritdoc/>
        protected override void DoRegister(string id, T defaultValue)
        {
            var ret = new EnumRule<T>(defaultValue) { Description = description };
            ret.OnUpdateValue += onUpdate;
            ret.Register(id);
        }
    }

    /// <summary>
    /// Builds rules with integer values.
    /// </summary>
    public class IntRuleBuilder : RuleBuilder<IntRuleBuilder, int>
    {
        private int max = int.MaxValue;
        private int min = int.MinValue;

        /// <summary>
        /// Sets the rule's maximum value.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public IntRuleBuilder Max(int max)
        {
            this.max = max;
            return this;
        }

        /// <summary>
        /// Sets the rule's minimum value.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public IntRuleBuilder Min(int min)
        {
            this.min = min;
            return this;
        }

        /// <inheritdoc/>
        protected override void DoRegister(string id, int defaultValue)
        {
            var ret = new IntRule(defaultValue, min, max) { Description = description };
            ret.OnUpdateValue += onUpdate;
            ret.Register(id);
        }
    }

    /// <summary>
    /// Builds rules with decimal values.
    /// </summary>
    public class FloatRuleBuilder : RuleBuilder<FloatRuleBuilder, float>
    {
        private float max = float.PositiveInfinity;
        private float min = float.NegativeInfinity;

        /// <summary>
        /// Sets the rule's maximum value.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public FloatRuleBuilder Max(int max)
        {
            this.max = max;
            return this;
        }

        /// <summary>
        /// Sets the rule's minimum value.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public FloatRuleBuilder Min(int min)
        {
            this.min = min;
            return this;
        }

        /// <inheritdoc/>
        protected override void DoRegister(string id, float defaultValue)
        {
            var ret = new FloatRule(defaultValue, min, max) { Description = description };
            ret.OnUpdateValue += onUpdate;
            ret.Register(id);
        }
    }
}
