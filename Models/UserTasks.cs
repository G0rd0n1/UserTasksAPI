namespace UserTasksAPI.Models
{
    public class UserTasks
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public DateTime? DueDate { get; set; }
        

    }
}
