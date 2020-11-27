using System.Collections.Generic;
using System.Linq;

namespace PatentsHelperRn
{
    public class ReferenceNumerals
    {
        public int Increment { get; set; }
        public Dictionary<int, string> KeyValuePairs { get; set; } = new Dictionary<int, string>();

        public int HighestNumber
        {
            get => KeyValuePairs.Keys.DefaultIfEmpty().Max();
        }

        public int LastNumber
        {
            get => KeyValuePairs.Keys.DefaultIfEmpty().Last();
        }

        public int NewNumberByHighst
        {
            get => HighestNumber + Increment;
        }

        public int NewNumberByLast
        {
            get => LastNumber + Increment;
        }

        public bool ReferenceNumeralExists(int number)
        {
            return KeyValuePairs?.Keys?.Contains(number) == true;
        }
    }
}
