using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole
{
    public class CityEntity
    {
        //City|State short|State full|County|City alias,
        public string City { get; set; }
        public string StateShort { get; set; }
        public string StateFull { get; set; }
        public string County { get; set; }
        public string CityAlias { get; set; }



        public string CityCN { get; set; }
        public string StateShortCN { get; set; }
        public string StateFullCN { get; set; }
        public string CountyCN { get; set; }
        public string CityAliasCN { get; set; }


    }
}
