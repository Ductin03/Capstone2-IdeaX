using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace IdeaX.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName),
                PublicId = Guid.NewGuid().ToString()  // Tạo tên file ngẫu nhiên
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Kiểm tra xem quá trình upload có thành công không
            if (uploadResult?.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null!;
        }
    }
}
