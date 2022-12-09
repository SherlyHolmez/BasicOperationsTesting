using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleData
{
    [Serializable]
    public class Product
    {
        public int Id { get; set; }
        public Decimal UnitPrice { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Category { get; set; }
        public string QuantityPerUnit { get; set; }
        public int UnitsAvailable { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");

            builder.Append("Name ");
            builder.Append(Name);

            builder.Append(", Quantity/Unit ");
            builder.Append(QuantityPerUnit);

            builder.Append(", UnitPrice ");
            builder.Append(UnitPrice);

            builder.Append(", UnitsAvailable ");
            builder.Append(UnitsAvailable);

            builder.Append("]");

            return builder.ToString();
        }

    }
}
