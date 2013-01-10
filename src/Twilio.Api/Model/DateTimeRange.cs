using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio
{
    /// <summary>
    /// Represents a range of time
    /// </summary>
    public class DateTimeRange : IEquatable<DateTimeRange>
    {
        DateTime? startDate, endDate;

        /// <summary>
        /// Creates a new default DateTimeRange
        /// </summary>
        public DateTimeRange() : this(new DateTime?(), new DateTime?()) { }
        
        /// <summary>
        /// Creates a new DateTimeRange with specific start and end datetime values
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public DateTimeRange(DateTime? startDate, DateTime? endDate)
        {
            AssertStartDateFollowsEndDate(startDate, endDate);
            this.startDate = startDate;
            this.endDate = endDate;
            this.StartComparison = ComparisonType.GreaterThanOrEqualTo;
            this.EndComparison = ComparisonType.LessThanOrEqualTo;
        }
        
        /// <summary>
        /// A TimeSpan representing the length of time between the end and start dates.
        /// </summary>
        public TimeSpan? TimeSpan
        {
            get { return endDate - startDate; }
        }
        
        /// <summary>
        /// The start of the DateTimeRange
        /// </summary>
        public DateTime? Start
        {
            get { return startDate; }
            set
            {
                AssertStartDateFollowsEndDate(value, this.endDate);
                startDate = value;
            }
        }

        /// <summary>
        /// Indicates what comparison type should be used for the StartTime
        /// </summary>
        public ComparisonType StartComparison { get; set; }

        /// <summary>
        /// The end of the DateTimeRange
        /// </summary>
        public DateTime? End
        {
            get { return endDate; }
            set
            {
                AssertStartDateFollowsEndDate(this.startDate, value);
                endDate = value;
            }
        }

        /// <summary>
        /// Indicates what comparison type should be used for the StartTime
        /// </summary>
        public ComparisonType EndComparison { get; set; }

        private void AssertStartDateFollowsEndDate(DateTime? startDate, DateTime? endDate)
        {
            if ((startDate.HasValue && endDate.HasValue) &&
                (endDate.Value < startDate.Value))
                throw new InvalidOperationException("Start Date must be less than or equal to End Date");
        }

        /// <summary>
        /// Determines if two DateTimeRange objects have the same value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DateTimeRange other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            return ((startDate == other.Start) && (endDate == other.End));
        }
    }
}
