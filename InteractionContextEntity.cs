using System;
using System.Collections.Generic;
using System.Linq;



namespace WinFormsApp1.Entities
{
    public class InteractionContextEntity
    {
        public Guid InteractionId { get; set; }
        public string UserId { get; set; }
        public string MediadorId { get; set; }
        public string TipoMediador { get; set; }
        public string UserAplicacional
        {
            get
            {
                int userAplicacional = int.TryParse(this.MediadorId, out userAplicacional) ? userAplicacional : -1;

                return userAplicacional > 0 ? userAplicacional.ToString() : this.UserId;
            }
        }
        public ErrorInfo[] Warings { get; private set; }

        public bool HasWarings { get { return this.Warings != null && this.Warings.Length > 0; } }

        public void AddWarnig(ErrorInfo waring)
        {
            if (waring == null)
            {
                return;
            }

            waring.Severity = ErrorSeverity.WARNING;

            if (Warings == null)
            {
                Warings = new ErrorInfo[] { waring };
            }
            else
            {
                List<ErrorInfo> temp = new List<ErrorInfo>(this.Warings);

                temp.Add(waring);

                this.Warings = temp.ToArray<ErrorInfo>();
            }
        }

        public void AddWarnig(int code, string description, string Source)
        {
            ErrorInfo waring = new ErrorInfo() { Code = code, Severity = ErrorSeverity.WARNING, Description = description, Source = Source };

            if (Warings == null)
            {
                Warings = new ErrorInfo[] { waring };
            }
            else
            {
                List<ErrorInfo> temp = new List<ErrorInfo>(this.Warings);

                temp.Add(waring);

                this.Warings = temp.ToArray<ErrorInfo>();
            }
        }

        public ErrorInfo RemoveWarnig(int index)
        {

            if (Warings == null || Warings.Length -1 < index)
            {
                return null;
            }
            else
            {
                List<ErrorInfo> temp = new List<ErrorInfo>(this.Warings);

                ErrorInfo error = temp[index];

                temp.RemoveAt(index);

                this.Warings = temp.Count == 0 ? null : temp.ToArray<ErrorInfo>();

                return error;
            }
        }

        public ErrorInfo RemoveLastWarnig()
        {

            if (Warings == null || Warings.Length == 0)
            {
                return null;
            }
            else
            {
                List<ErrorInfo> temp = new List<ErrorInfo>(this.Warings);

                ErrorInfo error = temp[this.Warings.Length -1];

                temp.RemoveAt(this.Warings.Length - 1);

                this.Warings = temp.Count == 0 ? null : temp.ToArray<ErrorInfo>();

                return error;
            }
        }
        
    }
}