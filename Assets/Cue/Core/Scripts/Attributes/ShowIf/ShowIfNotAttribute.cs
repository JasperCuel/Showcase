using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cue.Core
{
    /// <summary>
    /// Only show this field in the Unity Inspector when conditionField (boolean) is true
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfNotAttribute : PropertyAttribute
    {
        public string conditionField;
        public List<object> hasToEqualValues;
        public object defaultValue;
        public bool hide = false;

        /// <summary>
        /// Only show this field in the Unity Inspector when the value for <paramref name="conditionField"/> does not equal <paramref name="hasToEqualValue"/>
        /// </summary>
        /// <param name="conditionField">nameof([variable])</param>
        /// <param name="hasToEqualValue">The value the variable should not equal</param>
        /// <param name="defaultValue">Value this parameter will be set to when it hides</param>
        public ShowIfNotAttribute(string conditionField, object hasToEqualValue, object defaultValue, bool hide = false)
        {
            this.conditionField = conditionField;
            this.hasToEqualValues = new() { hasToEqualValue };
            this.defaultValue = defaultValue;
            this.hide = hide;
        }

        /// <summary>
        /// Only show this field in the Unity Inspector when the value for <paramref name="conditionField"/> does not equal one of the items listed in <paramref name="hasToEqualValues"/>
        /// </summary>
        /// <param name="conditionField">nameof([variable])</param>
        /// <param name="hasToEqualValues">A set of values the variable should not equal. If the variable equals one of them, it will not show.</param>
        /// <param name="defaultValue">Value this parameter will be set to when it hides</param>
        public ShowIfNotAttribute(string conditionField, object[] hasToEqualValues, object defaultValue, bool hide = false)
        {
            this.conditionField = conditionField;
            this.hasToEqualValues = new() { hasToEqualValues };
            this.defaultValue = defaultValue;
            this.hide = hide;
        }
    }
}