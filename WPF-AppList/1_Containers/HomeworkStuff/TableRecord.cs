using System;
using System.Collections.Generic;
using WPF_AppList._1._Containers.HomeworkStuff;

namespace WPF_AppList._1._Containers
{
    public class TableRecord
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<SubjectRecord> SubjectRecords = new();

        public TableRecord(List<SubjectRecord> subjectRecords)
        {
            SubjectRecords = subjectRecords;
        }
    }
}
