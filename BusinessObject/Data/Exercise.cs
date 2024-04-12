using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Data
{
	[Table("tb_Exercise")]
	public class Exercise
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Exercise_id { get; set; }
		[DisplayName("Exercise")]
		public string Exercise_name { get; set; }
		[StringLength(250)]
		[DisplayName("Creat time")]
		public DateTime Creat_time { get; set; }
		[DisplayName("File name ")]
		public string? File_name {  get; set; }
		public string? Link_submit_assignments { get; set; }

		[ForeignKey(nameof(Course))]
		public int Course_id { get; set; }
		public virtual Course Course { get; set; }
	}
}
