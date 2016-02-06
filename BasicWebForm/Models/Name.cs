using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicWebForm.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Person(int id, string firstName)
        {
            Id = id;
            Name = firstName;
        }
    }
}