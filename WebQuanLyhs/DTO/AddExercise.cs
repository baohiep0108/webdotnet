    namespace WebQuanLyhs.DTO
{
    public class AddExercise
    {
        public string Exercise_name { get; set; }
        public DateTime Creat_time { get; set; }

        public IFormFile File { get; set; }
        public string Link_submit_assignments { get; set; }
        public int Course_id { get; set; }
    }
}





