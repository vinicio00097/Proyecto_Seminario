using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models
{
    public class JsonMessage
    {
        public String Status { get; set; }
        public String Code { get; set; }
        public Object Data { get; set; }
        public String Message { get; set; }

        public JsonMessage(string status, string code, object data, string message)
        {
            Status = status;
            Code = code;
            Data = data;
            Message = message;
        }

    }
}
