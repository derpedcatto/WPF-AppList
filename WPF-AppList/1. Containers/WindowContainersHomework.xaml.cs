using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF_AppList._1._Containers.HomeworkStuff;

namespace WPF_AppList
{
    /// <summary>
    /// Interaction logic for WindowContainersHomework.xaml
    /// </summary>
    public partial class WindowContainersHomework : Window
    {
        private static string _teacher1 = "Самойленко Денис Николаевич";
        private static string _teacher2 = "Загоруйко Александр Дмитриевич";
        private Subject _subjectWPF = new Subject("WPF", "WPF Description", _teacher1);
        private Subject _subjectHTML = new Subject("HTML", "HTML Description", _teacher2);
        private Subject _subjectEmpty = new Subject("", "", "");

        public SubjectTable SubjectTable { get; }


        /// <summary>
        /// I'm to lazy to make proper logic, so constructor is BIG
        /// </summary>
        public WindowContainersHomework()
        {
            InitializeComponent();

            // Basic schedule
            TimeOnly time0 = new(00, 00);
            TimeOnly time1 = new(08, 00);
            TimeOnly time2 = new(09, 20);
            TimeOnly time3 = new(13, 20);

            // Basic subject lists
            List<SubjectRecord> list1 = new();
            list1.Add(new SubjectRecord(time1, _subjectWPF));
            list1.Add(new SubjectRecord(time2, _subjectWPF));
            list1.Add(new SubjectRecord(time3, _subjectHTML));
            List<SubjectRecord> list2 = new();
            list2.Add(new SubjectRecord(time1, _subjectHTML));
            list2.Add(new SubjectRecord(time2, _subjectHTML));
            list2.Add(new SubjectRecord(time3, _subjectWPF));
            List<SubjectRecord> list3 = new();
            list3.Add(new SubjectRecord(time0, _subjectEmpty));


            // Initializing SubjectTable
            SubjectTable = new();
            SubjectTable.TableRecords[0] = new(list1);
            SubjectTable.TableRecords[1] = new(list2);
            for (int i = 2; i < SubjectTable.TableRecords.Length; i++)
                SubjectTable.TableRecords[i] = new(list3);

            // Setting Day Of Week for every table
            for (int i = 0; i < SubjectTable.TableRecords.Length; i++)
                SubjectTable.TableRecords[i].DayOfWeek = (DayOfWeek)i;


            // Setting elements for SubjectTableGrid
            for (int i = 1; i < SubjectTable.TableRecords.Length + 1; i++) // 0 is reserved for textblocks
            {
                var record = SubjectTable.TableRecords[i - 1];

                // init new elements
                var day = new TextBlock();
                var lesson = new TextBlock();
                var time = new TextBlock();
                var subject = new TextBlock();

                // filling new elements with values
                day.Text = record.DayOfWeek.ToString();

                int lessonCount = 1;
                foreach (var item in record.SubjectRecords)
                {
                    lesson.Text += lessonCount++ + "\n";
                    time.Text += item.Time.ToString("HH:mm") + "\n";
                    subject.Text += item.Subject.Name + "\n";
                }
                lesson.Text = lesson.Text.TrimEnd();
                time.Text = time.Text.TrimEnd();
                subject.Text = subject.Text.TrimEnd();
                try { subject.ToolTip = record.SubjectRecords[i - 1].Subject.ToString(); } catch { }

                // adding new elements to grid
                SubjectTableGrid.Children.Add(day);
                Grid.SetRow(day, i);
                Grid.SetColumn(day, 0);
                SubjectTableGrid.Children.Add(lesson);
                Grid.SetRow(lesson, i);
                Grid.SetColumn(lesson, 1);
                SubjectTableGrid.Children.Add(time);
                Grid.SetRow(time, i);
                Grid.SetColumn(time, 2);
                SubjectTableGrid.Children.Add(subject);
                Grid.SetRow(subject, i);
                Grid.SetColumn(subject, 3);
            }
        }
    }
}
