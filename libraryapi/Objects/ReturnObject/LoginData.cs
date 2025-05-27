namespace libraryapi.Objects.ReturnObject
{
    public class LoginData
    {

        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        
        public LoginData()
        {
            this.success = false;
            this.token = null;
            this.message = null;
        }

        public LoginData(bool success, string token, string message)
        {
            this.success = success;
            this.token = token;
            this.message = message;
        }

    }
}
