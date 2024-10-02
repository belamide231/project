public class RegisterResults {

    
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
        public RegisterSuccess(string email, string password) : base(StatusCodes.Status200OK) {
            Registration = true;
            Credential = new CredentialModel(email, password);
        }
    }
}