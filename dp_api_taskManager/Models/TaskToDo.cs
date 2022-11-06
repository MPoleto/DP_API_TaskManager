namespace dp_api_taskManager.Models
{
    public class TaskToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public EnumStatusTask Status { get; set; }
    }
}