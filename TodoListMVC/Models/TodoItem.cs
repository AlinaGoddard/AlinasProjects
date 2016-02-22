using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TodoListMVC.Models
{
    public enum RecordType
    {
        Add,
        Edit
    }

    [Table(Name = "TodoList")]
    public class TodoItem
    {
        private int _TodoID;
        [Column(IsPrimaryKey = true, Storage = "_TodoID")]
        public int TodoID
        {
            get
            {
                return this._TodoID;
            }
            set
            {
                this._TodoID = value;
            }
        }

        private string _TaskName;
        [Column(Storage = "_TaskName")]
        public string TaskName 
        {
            get
            {
                return this._TaskName;
            }
            set
            {
                this._TaskName = value;
            }
        }

        private int _Priority;
        [Column(Storage = "_Priority")]
        public int Priority
        {
            get
            {
                return this._Priority;
            }
            set
            {
                this._Priority = value;
            }
        }

        private DateTime _DueDate;
        [Column(Storage = "_DueDate")]
        public DateTime DueDate 
        {
            get
            {
                return this._DueDate;
            }
            set
            {
                this._DueDate = value;
            }
        }

        public RecordType Type { get; set; }
    }
}