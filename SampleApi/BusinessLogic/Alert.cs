using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.BusinessLogic
{
    public class Alert
    {
        public int ID { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Severity { get; set; }

        public IEnumerable<Storage.IPAdress> Adresses { get; set; }

        public Storage.Alert ToStorageItem()
        {
            return new Storage.Alert()
            {
                ID = this.ID,
                Title = this.Title,
                Description = this.Description,
                Severity = this.Severity,
                Adresses = this.Adresses
            };
        }

        public static Alert FromStorageItem(Storage.Alert item)
        {
            return new Alert() { ID = item.ID, Title = item.Title, Severity = item.Severity, Adresses = item.Adresses };
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Alert))
                return false;

            return ((Alert)obj).ID.Equals(this.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}
