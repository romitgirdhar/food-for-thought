using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fft_mobileapp.Classes
{
    class Item
    {
        public string barcode { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public string issuingCountry { get; set; }

        public void setDesc(string desc)
        {
            description = desc;
        }

        public void setBarcode(string bc)
        {
            barcode = bc;
        }

        public void setSize(string sz)
        {
            size = sz;
        }

        public void setIssuingCountry(string ic)
        {
            issuingCountry = ic;
        }

        override
        public string ToString()
        {
            return barcode + "\t" + description + "\t" + size + "\t" + issuingCountry;
        }
    }
}