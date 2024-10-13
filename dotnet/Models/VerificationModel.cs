public class VerificationModel {

    public string VerificationCode { get; set; } 
    public string VerificationType { get; set; }
    public int FailedAttempts { get; set; } 
    
    public static string ChangePassword = "ChangePassword";
    public static string Registration = "Registration";
    public static string Forgot = "Forgot";


    public VerificationModel(string verificationCode, string verificationType, int failedAttempts) {
        VerificationCode = verificationCode;
        VerificationType = verificationType;
        FailedAttempts = failedAttempts;
    } 
}