﻿using System;
using System.Text;
using Kingo.BuildingBlocks.Resources;

namespace Kingo.BuildingBlocks.Constraints
{
    /// <summary>
    /// Represents a range or domain of values.
    /// </summary>
    /// <typeparam name="TValue">Type of values in this range.</typeparam>
    public struct Range<TValue> : IRange<TValue>, IEquatable<Range<TValue>> where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly TValue _left;
        private readonly TValue _right;
        private readonly RangeOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{TValue}" /> class.
        /// </summary>
        /// <param name="left">The lower boundary of this range.</param>
        /// <param name="right">The upper boundary of this range.</param>
        /// <param name="options">
        /// The options indicating whether or <paramref name="left"/> and/or <paramref name="right"/> are part of this range themselves.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="left"/> and <paramref name="right"/> do not represent a valid range, or both are
        /// equal and <paramref name="options"/> specifies at least one exclusive boundary.
        /// </exception>
        public Range(TValue left, TValue right, RangeOptions options = RangeOptions.None)           
        {
            if (IsValidRange(left, right, options))
            {
                _left = left;
                _right = right;
                _options = options & RangeOptions.AllExclusive;                
            }
            else
            {
                throw NewInvalidRangeException(left, right, options);
            }            
        }

        private static bool IsValidRange(TValue left, TValue right, RangeOptions options)
        {
            if (Comparer.IsSmallerThan(left, right))
            {
                return true;
            }
            if (Comparer.IsEqualTo(left, right as IEquatable<TValue>))
            {
                return !IsSet(options, RangeOptions.LeftExclusive) && !IsSet(options, RangeOptions.RightExclusive);
            }
            return false;
        }

        /// <inheritdoc />
        public TValue Left
        {
            get { return _left; }
        }

        /// <inheritdoc />
        public TValue Right
        {
            get { return _right; }
        }        

        /// <summary>
        /// Indicates whether or not the lower boundary is included within the range.
        /// </summary>
        public bool IsLeftInclusive
        {
            get { return !IsLeftExclusive; }
        }

        /// <summary>
        /// Indicates whether or not the lower boundary is excluded from the range.
        /// </summary>
        public bool IsLeftExclusive
        {
            get { return IsSet(_options, RangeOptions.LeftExclusive); }
        }

        /// <summary>
        /// Indicates whether or not the upper boundary is included within the range.
        /// </summary>
        public bool IsRightInclusive
        {
            get { return !IsRightExclusive; }
        }

        /// <summary>
        /// Indicates whether or not the upper boundary is excluded from the range.
        /// </summary>
        public bool IsRightExclusive
        {
            get { return IsSet(_options, RangeOptions.RightExclusive); }
        }

        private static bool IsSet(RangeOptions options, RangeOptions option)
        {
            return (options & option) == option;
        }

        #region [====== Equals ======]

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Range<TValue>)
            {
                return Equals((Range<TValue>) obj);
            }
            return false;
        }

        /// <inheritdoc />
        public bool Equals(Range<TValue> other)
        {
            return _options == other._options && Equals(_left, other._left) && Equals(_right, other._right);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Of(_options, _left, _right);
        }

        #endregion

        /// <inheritdoc />
        public bool Contains(TValue value)
        {
            return IsWithinLeftBoundary(value) && IsWithinRightBoundary(value);
        }

        private bool IsWithinLeftBoundary(TValue value)
        {
            if (IsLeftInclusive)
            {
                return Comparer.IsGreaterThanOrEqualTo(value, _left);
            }
            return Comparer.IsGreaterThan(value, _left);
        }

        private bool IsWithinRightBoundary(TValue value)
        {
            if (IsRightInclusive)
            {
                return Comparer.IsSmallerThanOrEqualTo(value, _right);
            }
            return Comparer.IsSmallerThan(value, _right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(_left, _right, _options);
        }

        private static Exception NewInvalidRangeException(TValue left, TValue right, RangeOptions options)
        {
            var messageFormat = ExceptionMessages.Range_InvalidRange;
            var message = string.Format(messageFormat, ToString(left, right, options));
            return new ArgumentOutOfRangeException(message);
        } 

        private static string ToString(TValue left, TValue right, RangeOptions options)
        {
            var range = new StringBuilder();
            range.Append(IsSet(options, RangeOptions.LeftExclusive) ? '<' : '[');
            range.AppendFormat("{0}, {1}", left, right);
            range.Append(IsSet(options, RangeOptions.RightExclusive) ? '>' : ']');
            return range.ToString();
        }        
    }
}