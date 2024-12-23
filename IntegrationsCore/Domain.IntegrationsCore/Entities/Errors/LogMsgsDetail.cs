﻿using Domain.IntegrationsCore.Interfaces;

namespace Domain.IntegrationsCore.Entities.Errors
{
    public class LogMsgsDetail : ILogMsgsDetail
    {
        public int? IdLogMsgDetail { get; set; }
        public int? IdLogMsg { get; set; }
        public string FieldKeyValue { get; set; } = "";
        public string? RegText { get; set; }
        public DateTime LastUpdateOn { get; set; }
    }
}

