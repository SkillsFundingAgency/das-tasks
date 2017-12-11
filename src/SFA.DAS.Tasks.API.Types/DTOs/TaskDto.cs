namespace SFA.DAS.Tasks.API.Types.DTOs
{
    public class TaskDto
    {
        public string Type { get; set; }
        public string EmployerAccountId { get; set; }
        public int ItemsDueCount { get; set; }

        public string OwnerId => EmployerAccountId; //This has been left to support legacy clients. Remove then clients have been updated
    }
}
