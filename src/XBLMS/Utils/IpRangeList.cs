using System.Collections.Generic;
using System.Text;

namespace XBLMS.Utils
{
    public class IpRangeList
    {
        private bool _isSorted;
        private readonly List<uint> _ipNumList = new List<uint>();
        private readonly uint _ipMask;

        /// <summary>
        /// Constructor that sets the mask for the list
        /// </summary>
        public IpRangeList(uint mask)
        {
            _ipMask = mask;
        }

        /// <summary>
        /// Add a new IP numer (range) to the list
        /// </summary>
        public void Add(uint ipNum)
        {
            _isSorted = false;
            _ipNumList.Add(ipNum & _ipMask);
        }

        /// <summary>
        /// Checks if an IP number is within the ranges included by the list
        /// </summary>
        public bool Check(uint ipNum)
        {
            var found = false;
            if (_ipNumList.Count > 0)
            {
                if (!_isSorted)
                {
                    _ipNumList.Sort();
                    _isSorted = true;
                }

                ipNum = ipNum & _ipMask;
                if (_ipNumList.BinarySearch(ipNum) >= 0) found = true;
            }

            return found;
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            _ipNumList.Clear();
            _isSorted = false;
        }

        /// <summary>
        /// The ToString is overriden to generate a list of the IP numbers
        /// </summary>
        public override string ToString()
        {
            var buf = new StringBuilder();
            foreach (uint ipNum in _ipNumList)
            {
                if (buf.Length > 0) buf.Append("\r\n");
                buf.Append(((int)ipNum & 0xFF000000) >> 24).Append('.');
                buf.Append(((int)ipNum & 0x00FF0000) >> 16).Append('.');
                buf.Append(((int)ipNum & 0x0000FF00) >> 8).Append('.');
                buf.Append(((int)ipNum & 0x000000FF));
            }

            return buf.ToString();
        }
    }
}
