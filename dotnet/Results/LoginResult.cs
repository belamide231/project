using System.Text.Json.Serialization;

public class LoginResult {

    public static readonly string InvalidUsername = "Invalid username.";
    public static readonly string IncorrectPassword = "Password is incorrect.";
    public static readonly string AccountIsLocked = "Account is locked due to consecutive fail attempt.";
    public static readonly string InternalServerIsError = "Internal server is error.";

    public class LoginFail : StatusModel {
        public bool Login { get; set; } 
        public string Conflict { get; set; }
        public LoginFail(string Cause) : base(StatusCodes.Status409Conflict) {
            Login = false;
            Conflict = Cause;
        } 
    }

    public class AccountLocked : StatusModel {
        public bool Login { get; set; }
        public string Conflict { get; set; }
        public string LockEnd { get; set; }
        public AccountLocked(string lockEnd) : base(StatusCodes.Status403Forbidden) {
            Login = false;
            Conflict = AccountIsLocked;
            LockEnd = lockEnd;
        }
    }

    public class InternalServerError : StatusModel {
        public bool Login { get; set; }
        public string Error { get; set; }
        public InternalServerError(string error) : base(StatusCodes.Status500InternalServerError) {
            Login = false;
            Error = error;
        }
    }

    public class LoginSuccess : StatusModel {
        public bool Login { get; set; }
        public string AccessToken { get; set; }
        public LoginSuccess(string token) : base(StatusCodes.Status200OK) {
            Login = true;
            AccessToken = token;
        } 
    }
}