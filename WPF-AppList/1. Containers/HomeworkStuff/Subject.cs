namespace WPF_AppList._1._Containers.HomeworkStuff
{
    public class Subject
    {
        public string Name { get; }
        public string Description { get; }
        public string Teacher { get; set; }

        public Subject(string name, string description, string teacher)
        {
            Name = name;
            Description = description;
            Teacher = teacher;
        }
        public Subject()
        {
            Name = "";
            Description = "";
            Teacher = "";
        }

        public override string ToString()
        {
            return ($"Subject: { Name }\n{ Description }\n\nTeacher: { Teacher }");
        }
    }
}
