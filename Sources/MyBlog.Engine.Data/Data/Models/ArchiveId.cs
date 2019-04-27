using System;

namespace MyBlog.Engine.Data.Models
{
    public sealed class ArchiveId
    {
        #region Declarations

        private readonly Int32 _year;
        private readonly Nullable<Int32> _month;
        private readonly String _id;

        #endregion

        #region Constructors

        public ArchiveId(Int32 year)
        {
            _year = year;
            _month = null;
            _id = _year.ToString();
        }

        public ArchiveId(Int32 year, Int32 month)
        {
            _year = year;
            _month = month;
            _id = _year.ToString() + "-" + _month.ToString();
        }

        public ArchiveId(String id)
        {
            // Test the Id
            if (String.IsNullOrEmpty(id)) return;
            _id = id;
            String[] values = id.Split('-');
            if (values == null) return;

            // Try to get the year
            if (!Int32.TryParse(values[0], out _year))
            {
                _year = DateTime.MinValue.Year;
            }

            // Test if month is defined
            if (values.Length == 2)
            {
                // Try to get the month
                if (Int32.TryParse(values[1], out Int32 month))
                {
                    _month = month;
                }
                else
                {
                    // The month is undefined
                    _month = null;
                }
            }
            else
            {
                // The month is not defined
                _month = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Year
        /// </summary>
        public Int32 Year => _year;

        /// <summary>
        /// Month
        /// </summary>
        public Nullable<Int32> Month => _month;

        /// <summary>
        /// Id
        /// </summary>
        public String Id => _id;

        #endregion

        #region Methodes

        public override string ToString()
        {
            return _month.HasValue
                ? new DateTime(_year, _month.Value, 1).ToString("MMMM yyyy")
                : _year.ToString();
        }

        #endregion
    }
}
