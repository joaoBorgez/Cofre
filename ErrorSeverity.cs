using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinFormsApp1.Entities
{
    [Serializable]
    public enum ErrorSeverity
    {

        WARNING,
        VALIDATION,
        EXTERNAL,
        ERROR
    }
}