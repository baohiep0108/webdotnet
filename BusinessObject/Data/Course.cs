using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Data
{
	[Table("tb_Coures")]
	public class Course
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Coures_id { get; set; }
		[DisplayName("Category")]
		public string? Coures_name { get; set; }
		[DisplayName("Category")]

		[ForeignKey(nameof(Category_Course))]
		public int Category_coures_id { get; set; }
        [DisplayName("Category")]

        public virtual Category_Course Category_Course { get; set; }
		public ICollection<Exercise> Exercises { get; set;} = new List<Exercise>();
		public ICollection<Teacher_Course> Teacher_Courses { get; set;} = new List<Teacher_Course>();
		

	}
}
