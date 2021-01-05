using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendServices.Models
{
    public class TaskEntity
    {
        public int idTask { get; set; }
        public string description { get; set; }
        public int idPeople { get; set; }
        public string stateTask { get; set; }
        public string priority { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string notes { get; set; }
    }
}
