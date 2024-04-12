using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Data
{
    [Table("tb_Teacher_Class")]
    public class Teacher_Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Study_Class_id { get; set; }

        [ForeignKey(nameof(Class_Role))]
        [DisplayName("Class")]
        public int Class_Role_id { get; set; }
        public virtual Class_Role Class_Role { get; set; }

        [ForeignKey(nameof(Teacher_Course))]
        [DisplayName("")]
        public int Teacher_Course_id { get; set; }
        public virtual Teacher_Course Teacher_Course { get; set; }

    }
}
