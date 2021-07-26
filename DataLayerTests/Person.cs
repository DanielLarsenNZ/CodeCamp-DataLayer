﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    /// <summary>
    /// A Person Model
    /// </summary>
    /// <remarks>AKA DTO (data transfer object), POCO (plain old c# object)</remarks>
    public class Person : Model
    {
        //TODO: Properties
        public string FirstName { get; set; }

        public string  LastName { get; set; }

        public double HoursWorked { get; set; }

        public string Phone { get; set; } 
    }
}