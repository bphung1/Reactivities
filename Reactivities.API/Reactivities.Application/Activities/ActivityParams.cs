using Reactivities.Application.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reactivities.Application.Activities
{
    public class ActivityParams : PagingParams
    {
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
