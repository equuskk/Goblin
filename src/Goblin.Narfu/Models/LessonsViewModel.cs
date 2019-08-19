using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goblin.Narfu.Models
{
    public class LessonsViewModel
    {
        private readonly DateTime _date;
        public IEnumerable<Lesson> Lessons { get; }

        public LessonsViewModel(IEnumerable<Lesson> lessons, DateTime date)
        {
            _date = date;
            Lessons = lessons;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            
            foreach(var lesson in Lessons.Where(x => x.StartTime.DayOfYear == _date.DayOfYear))
            {
                strBuilder.AppendFormat("{0} - {1} [{2}] ({3})",
                                        lesson.StartEndTime, lesson.Name, lesson.Type, lesson.Teacher)
                          .AppendLine()
                          .AppendFormat("У группы {0}", lesson.Groups).AppendLine()
                          .AppendFormat("В аудитории {0} ({1})", lesson.Auditory, lesson.Address).AppendLine()
                          .AppendLine()
                          .AppendLine();
            }

            return strBuilder.ToString();
        }
    }
}