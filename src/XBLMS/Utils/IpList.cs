using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XBLMS.Utils
{
    public class IpList
    {
        private readonly List<IpRangeList> _ipRangeList = new List<IpRangeList>();
        private readonly SortedList _maskList = new SortedList();
        private readonly List<int> _usedList = new List<int>();

        public IpList()
        {
            // Initialize IP mask list and create IPArrayList into the ipRangeList
            uint mask = 0x00000000;
            for (var level = 1; level < 33; level++)
            {
                mask = (mask >> 1) | 0x80000000;
                _maskList.Add(mask, level);
                _ipRangeList.Add(new IpRangeList(mask));
            }
        }

        // Parse a String IP address to a 32 bit unsigned integer
        // We can't use System.Net.IPAddress as it will not parse
        // our masks correctly eg. 255.255.0.0 is pased as 65535 !
        private uint parseIP(string ipNumber)
        {
            uint res = 0;
            var elements = ipNumber.Split('.');
            if (elements.Length == 4)
            {
                res = (uint)Convert.ToInt32(elements[0]) << 24;
                res += (uint)Convert.ToInt32(elements[1]) << 16;
                res += (uint)Convert.ToInt32(elements[2]) << 8;
                res += (uint)Convert.ToInt32(elements[3]);
            }

            return res;
        }

        /// <summary>
        /// Add a single IP number to the list as a string, ex. 10.1.1.1
        /// </summary>
        public void Add(string ipNumber)
        {
            Add(parseIP(ipNumber));
        }

        /// <summary>
        /// Add a single IP number to the list as a unsigned integer, ex. 0x0A010101
        /// </summary>
        private void Add(uint ip)
        {
            _ipRangeList[31].Add(ip);
            if (_usedList.Contains(31)) return;
            _usedList.Add(31);
            _usedList.Sort();
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        /// fixed bits, ex. 172.16.0.0 255.255.0.0 will add 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(string ipNumber, string mask)
        {
            Add(parseIP(ipNumber), parseIP(mask));
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        /// fixed bits, ex. 0xAC1000 0xFFFF0000 will add 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(uint ip, uint uMask)
        {
            var level = _maskList[uMask];
            if (level == null) return;
            ip = ip & uMask;
            _ipRangeList[(int)level - 1].Add(ip);
            if (_usedList.Contains((int)level - 1)) return;
            _usedList.Add((int)level - 1);
            _usedList.Sort();
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        /// fixed bits, ex. 192.168.1.0/24 which will add 192.168.1.0 - 192.168.1.255
        /// </summary>
        public void Add(string ipNumber, int maskLevel)
        {
            Add(parseIP(ipNumber), (uint)_maskList.GetKey(_maskList.IndexOfValue(maskLevel)));
        }

        /// <summary>
        /// Adds IP numbers using a from and to IP number. The method checks the range and
        /// splits it into normal ip/mask blocks.
        /// </summary>
        public void AddRange(string fromIp, string toIp)
        {
            AddRange(parseIP(fromIp), parseIP(toIp));
        }

        /// <summary>
        /// Adds IP numbers using a from and to IP number. The method checks the range and
        /// splits it into normal ip/mask blocks.
        /// </summary>
        private void AddRange(uint fromIp, uint toIp)
        {
            // If the order is not asending, switch the IP numbers.
            if (fromIp > toIp)
            {
                var tempIp = fromIp;
                fromIp = toIp;
                toIp = tempIp;
            }

            if (fromIp == toIp)
            {
                Add(fromIp);
            }
            else
            {
                var diff = toIp - fromIp;
                var diffLevel = 1;
                var range = 0x80000000;
                if (diff < 256)
                {
                    diffLevel = 24;
                    range = 0x00000100;
                }

                while (range > diff)
                {
                    range = range >> 1;
                    diffLevel++;
                }

                var mask = (uint)_maskList.GetKey(_maskList.IndexOfValue(diffLevel));
                var minIp = fromIp & mask;
                if (minIp < fromIp) minIp += range;
                if (minIp > fromIp)
                {
                    AddRange(fromIp, minIp - 1);
                    fromIp = minIp;
                }

                if (fromIp == toIp)
                {
                    Add(fromIp);
                }
                else
                {
                    if ((minIp + (range - 1)) <= toIp)
                    {
                        Add(minIp, mask);
                        fromIp = minIp + range;
                    }

                    if (fromIp == toIp)
                    {
                        Add(toIp);
                    }
                    else
                    {
                        if (fromIp < toIp) AddRange(fromIp, toIp);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an IP number is contained in the lists, ex. 10.0.0.1
        /// </summary>
        public bool CheckNumber(string ipNumber)
        {
            return CheckNumber(parseIP(ipNumber));
        }

        /// <summary>
        /// Checks if an IP number is contained in the lists, ex. 0x0A000001
        /// </summary>
        private bool CheckNumber(uint ip)
        {
            var found = false;
            var i = 0;
            while (!found && i < _usedList.Count)
            {
                found = _ipRangeList[_usedList[i]].Check(ip);
                i++;
            }

            return found;
        }

        /// <summary>
        /// Clears all lists of IP numbers
        /// </summary>
        public void Clear()
        {
            foreach (var i in _usedList)
            {
                _ipRangeList[i].Clear();
            }

            _usedList.Clear();
        }

        /// <summary>
        /// Generates a list of all IP ranges in printable format
        /// </summary>
        public override string ToString()
        {
            var buffer = new StringBuilder();
            foreach (var i in _usedList)
            {
                buffer.Append("\r\nRange with mask of ").Append(i + 1).Append("\r\n");
                buffer.Append(_ipRangeList[i]);
            }

            return buffer.ToString();
        }
    }
}
