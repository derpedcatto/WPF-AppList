using System;

namespace WPF_AppList._1._Containers.HomeworkStuff
{
    public class SubjectRecord
    {
        public TimeOnly Time { get; set; }
        public Subject Subject { get; }

        public SubjectRecord(TimeOnly time, Subject subject)
        {
            Time = time;
            Subject = subject;
        }
    }
}
