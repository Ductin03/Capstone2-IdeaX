namespace IdeaX.Model.ResponseModels
{
    public class AuthResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public UserResponseModel User { get; set; }
    }
}
