using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicWebForm.Models
{
    public enum RecordType
    {
        Add,
        Edit
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RecordType Type { get; set; }

        public Person(int id, string firstName, RecordType type)
        {
            Id = id;
            Name = firstName;
            Type = type;
        }
    }
}