public class VerifyResult {

    public static string Taken = "Email is already taken.";
    public static string Error = "Error verifying email.";
    public static string Exist = "We already sent a verification code to your email, please check it before it expires.";
    public static string Mailed = "We sent an mail to your email.";

    public class Conflict : StatusModel {

        public string conflict { get; set; }
        public bool verify { get; set; }

        public Conflict(string Conflict) : base(409) {
            conflict = Conflict;
            verify = false;
        }
    }


    public class Success : StatusModel {

        public string message { get; set; }
        public bool verify { get; set; }
        public Success(string Message) : base(200) {
            message = Message;
            verify = true; 
        }
    }
}