using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.AcceptanceTests.Commitments
{
    public class CommitmentsBadTestMessage
    {
        public string AccountId { get; set; }

        public CommitmentsBadTestMessage()
        {
            AccountId = "Accountid";
        }
    }
}
