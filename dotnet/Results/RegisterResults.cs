using System.Text.Json.Serialization;

public class RegisterResults {

    public static string IncorrectVerificationCode = "Verification code is incorrect";
    public static string InvalidVerificationCode = "Verification code is invalid.";
    public static string RegisteringThisEmailIsLock = "This email is locked, wait till the code expires.";

    
    public class RegisterFail : StatusModel {
        public bool Registration { get; set; }
        public string Conflict { get; set; }
        public RegisterFail(string Cause) : base(StatusCodes.Status409Conflict) {
            Conflict = Cause;
            Registration = false;
        } 
    }


    public class RegisterSuccess : StatusModel {
        public bool Registration { get; set; }
        public CredentialModel Credential { get; set; }
        public string DeviceId { get; set; }

        public RegisterSuccess(string email, string password, string deviceId) : base(StatusCodes.Status200OK) {
            Registration = true;
            Credential = new CredentialModel(email, password);
            DeviceId = deviceId;
        }
    }


    public class VerificationCodeInvalid : StatusModel {
        public string Conflict { get; set; }
        public bool Registration { get; set; }

        public VerificationCodeInvalid(string cause) : base(StatusCodes.Status401Unauthorized) {
            Conflict = cause;
            Registration = false;
        }
    }


    public class VerificationCodeConflict : StatusModel {
        public string Conflict { get; set; }
        public bool Registration { get; set; }
        public VerificationCodeConflict(string conflict, int failedAttempts) : base(StatusCodes.Status409Conflict) {
            Conflict = 
                failedAttempts == 3 ? InvalidVerificationCode : 
                failedAttempts == 1 ? conflict + $", you still have {3 - failedAttempts} Attempts." :
                conflict + $", you still have {3 - failedAttempts} Attempt.";
            Registration = false;
        }
    }
}