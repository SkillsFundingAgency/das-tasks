﻿using MediatR;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequest : IAsyncRequest<GetTaskResponse>
    {
        public string OwnerId { get; set; }
        public TaskType Type { get; set; }
    }
}
