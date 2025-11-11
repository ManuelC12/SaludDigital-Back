namespace SaludDigital.Models
{
    public class Response
    {
        public string Status { get; set; }
        public string Info { get; set; }
        public Object Content { get; set; }

        public Response()
        {

        }

        public Response(string status, string info)
        {
            this.Status = status;
            this.Info = info;
            this.Content = null;
        }
    }
}
