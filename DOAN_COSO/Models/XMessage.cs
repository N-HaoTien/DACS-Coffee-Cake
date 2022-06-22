using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN_COSO
{
    public class XMessage
    {
        public string Type { get; set; }

        public string Messeage { get; set; }

        public XMessage() { }

        public XMessage(string type , string me) 
        {
            Type = type;
            Messeage = me;
        }


    }
}