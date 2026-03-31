using System;
using System.Collections.Generic;
using System.Text;

namespace cs330_proj1
{
    public class CoreGoal: IComparable<CoreGoal>
    {
        public string Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public List<Course> Courses {get;set;}


        public override String ToString() {
            StringBuilder courseList = new StringBuilder();
            for(int i=0; i<Courses.Count; i++) {
                courseList.Append(Courses[i].ToString().TrimEnd());
                if(i<Courses.Count-1) {
                    courseList.Append('\n');
                }
            }
            return $"{Id}-{Name}: {Description}\n{courseList}";

        }
        public int CompareTo(CoreGoal other) {
            return this.Name.CompareTo(other.Name);
        }

        
    }
}
