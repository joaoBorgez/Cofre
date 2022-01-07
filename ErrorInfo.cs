using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WinFormsApp1.Entities
{
    [Serializable]
    public class ErrorInfo
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public ErrorSeverity Severity { get; set; }
        public string Source { get; set; }
    }
}