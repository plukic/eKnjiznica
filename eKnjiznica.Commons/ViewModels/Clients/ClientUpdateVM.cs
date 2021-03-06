﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eKnjiznica.Commons.ViewModels.Clients
{
   
    [DataContract]
    public class ClientUpdateVM
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string OldPassword { get; set; }

        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
