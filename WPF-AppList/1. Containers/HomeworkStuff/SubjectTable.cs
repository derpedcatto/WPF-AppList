using System;

namespace WPF_AppList._1._Containers.HomeworkStuff
{
    public class SubjectTable
    {
        public TableRecord[] TableRecords { get; }

        public SubjectTable()
        {
            TableRecords = new TableRecord[7];  // 7 days of week
        }
    }
}
