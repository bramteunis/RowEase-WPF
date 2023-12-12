﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Certificate
    {
        public int id { get; set; }
        public string name { get; set; }

        public Certificate(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
