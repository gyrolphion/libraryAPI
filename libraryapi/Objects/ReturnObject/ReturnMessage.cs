namespace libraryapi.Objects.ReturnObject
{
    public class ReturnMessage
    {

        public bool success { get; set; }
        public string message { get; set; }

        public ReturnMessage()
        {
            this.success = false;
            this.message = null;
        }

        public ReturnMessage(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

    }
}
