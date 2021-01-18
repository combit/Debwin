using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Debwin.Core
{

    [Serializable]
    public class FilterDefinition : IComparable<FilterDefinition>
    {
        public string Name { get; set; }

        public LogLevel MinLevel { get; set; }

        public string LoggerNames { get; set; }

        public string MessageTextFilter { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateUntil { get; set; }

        public string Thread { get; set; }

        public List<FilterCriterion> Criteria { get; set; }

        public void AddCriterion(int propertyId, FilterOperator @operator, string expectedValues)
        {
            Criteria = Criteria ?? new List<FilterCriterion>(1);
            Criteria.Add(new FilterCriterion(propertyId, @operator, expectedValues));
        }

        public FilterDefinition()
        {
            MinLevel = LogLevel.Debug;
        }


        public Func<LogMessage, bool> BuildPredicate()
        {

            Func<LogMessage, bool> currentPredicate;

            // When building the expression, copy the old value to a local variable before referencing it in a new expression!
            // Otherwise the expression would reference itself and cause a stack overflow on execution!
            currentPredicate = (msg => msg.Level >= MinLevel);

            // Text Search
            if (!string.IsNullOrEmpty(MessageTextFilter))
            {
                var oldPredicate = currentPredicate;

                if (MessageTextFilter.StartsWith("/") && MessageTextFilter.EndsWith("/") && MessageTextFilter.Length > 2)   // Regex search when in slashes:   /regex/
                {
                    Regex regex = new Regex(MessageTextFilter.Substring(1, MessageTextFilter.Length - 2), RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    currentPredicate = (msg => oldPredicate(msg) && regex.IsMatch(msg.Message));
                }
                else
                {
                    currentPredicate = (msg => oldPredicate(msg) && msg.Message.IndexOf(MessageTextFilter, StringComparison.OrdinalIgnoreCase) != -1);
                }
            }

            // Logger Search
            if (!string.IsNullOrEmpty(LoggerNames))
            {
                // List of AND/OR conditions for the logger name, becomes one condition in the outer filter
                Func<LogMessage, bool> loggerFilterPredicate = BuildIncludeOrExcludeFilter(LoggerNames, (LogMessage msg) => msg.LoggerName);

                var oldPredicate = currentPredicate;
                currentPredicate = (msg => oldPredicate(msg) && loggerFilterPredicate(msg));
            }

            // Date From
            if (DateFrom != null)
            {
                DateTime dateFrom = DateFrom.Value;

                var oldPredicate = currentPredicate;
                currentPredicate = (msg => oldPredicate(msg) && msg.Timestamp >= dateFrom);

            }

            // Date To
            if (DateUntil != null)
            {
                DateTime dateTo = DateUntil.Value;

                var oldPredicate = currentPredicate;
                currentPredicate = (msg => oldPredicate(msg) && msg.Timestamp <= dateTo);
            }

            // Thread
            if (!string.IsNullOrEmpty(Thread))
            {
                var oldPredicate = currentPredicate;

                currentPredicate = (msg => oldPredicate(msg) && msg.Thread == Thread);

            }

            // Other criteria:
            if (Criteria != null)
            {
                foreach (FilterCriterion criterion in Criteria)
                {
                    var oldPredicate = currentPredicate;

                    switch (criterion.Operator)
                    {
                        case FilterOperator.StringEquals:
                            {
                                currentPredicate = (msg => oldPredicate(msg) && criterion.ExpectedValues.Equals((string)msg.GetProperty(criterion.PropertyId), StringComparison.InvariantCultureIgnoreCase));
                                break;
                            }
                        case FilterOperator.StringIncludesOrExcludes:
                            {
                                Func<LogMessage, bool> moduleFilterPredicate = BuildIncludeOrExcludeFilter(criterion.ExpectedValues, (LogMessage msg) => msg.GetProperty(criterion.PropertyId) as string);
                                currentPredicate = (msg => oldPredicate(msg) && moduleFilterPredicate(msg));
                                break;
                            }
                    }
                }
            }

            return currentPredicate;
        }

        /// <summary>Builds a filter condition on a property that includes only some values or excludes specific values.</summary>
        /// <param name="filterDef">
        /// Semicolon-separated list of values that should be compared with the property.
        /// Default: Message is only included, when the property matches ONE OF the listed values.
        /// If the filterDef string starts with an exclamation mark, a message is only included if the property value matches NONE OF the listed values.
        /// </param>
        /// <param name="propertyValueProvider">Function that returns the property to compare for a given log message.</param>
        private Func<LogMessage, bool> BuildIncludeOrExcludeFilter(string filterDef, Func<LogMessage, string> propertyValueProvider)
        {
            // List of AND/OR conditions for the value of a message property
            Func<LogMessage, bool> filterPredicate;

            bool excludeMode;
            if (filterDef.StartsWith("!"))  // include all, but exclude some of the values
            {
                excludeMode = true;
                filterDef = filterDef.Substring(1);
                filterPredicate = ((msg) => true);   // default: message is INCLUDED, until the property has one of the UNWANTED loggers
            }
            else    // include only the specified values
            {
                excludeMode = false;
                filterPredicate = ((msg) => false);  // default: message is NOT INCLUDED, until the property is one of the WANTED values
            }

            string[] possibleValues = filterDef.Split(';');

            for (int i = 0; i < possibleValues.Length; i++)
            {
                string value2Compare = possibleValues[i].Trim();

                var innerFilterPredicate = filterPredicate;
                if (excludeMode)  // (property != value1) AND (property != value2) AND ...
                {
                    filterPredicate = (msg => innerFilterPredicate(msg) && !String.Equals(propertyValueProvider(msg), value2Compare, StringComparison.Ordinal));
                }
                else     // (property = value1) OR (property = value2) OR ... 
                {
                    filterPredicate = (msg => innerFilterPredicate(msg) || String.Equals(propertyValueProvider(msg), value2Compare, StringComparison.Ordinal));
                }
            }

            return filterPredicate;
        }

        public int CompareTo(FilterDefinition other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }

    public enum FilterOperator
    {
        StringEquals,
        StringIncludesOrExcludes
    }


    [Serializable]
    public class FilterCriterion
    {
        public int PropertyId { get; set; }

        public FilterOperator Operator { get; set; }

        public string ExpectedValues { get; set; }

        [Obsolete("only for serializer")]
        public FilterCriterion() { /* just for serializer */ }

        public FilterCriterion(int propertyId, FilterOperator @operator, string expectedValues)
        {
            PropertyId = propertyId;
            Operator = @operator;
            ExpectedValues = expectedValues;
        }
    }
}
