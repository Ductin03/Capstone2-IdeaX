namespace IdeaX.Model.RequestModels
{
    public class OtpVerificationRequestModel
    {
        public string Email { get; set; }
        public string OTP { get; set; } = string.Empty;
    }
}
