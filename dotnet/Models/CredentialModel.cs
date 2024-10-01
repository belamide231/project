public class Credential {
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Credential(string email, string password) {
        Email = email;
        Username = email;
        Password = password;
    }
}