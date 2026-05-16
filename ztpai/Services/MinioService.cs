using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace ztpai.Services
{
    public class MinioService(IMinioClient minioClient) : IProductImageService
    {
        private const string BucketName = "productphotos";

        public async Task<string> UploadProductImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            var extension = Path.GetExtension(file.FileName);
            var objectName = $"{Guid.NewGuid():N}{extension}";

            await using var stream = file.OpenReadStream();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);

            return objectName;
        }

        public async Task DeleteProductImageAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is required.");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName);

            await minioClient.RemoveObjectAsync(removeObjectArgs);
        }

        public async Task<(Stream Stream, string ContentType)> GetProductImageAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is required.");
            }

            var statArgs = new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName);

            var stat = await minioClient.StatObjectAsync(statArgs);
            var memoryStream = new MemoryStream();

            var getArgs = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await minioClient.GetObjectAsync(getArgs);
            memoryStream.Position = 0;

            var contentType = string.IsNullOrWhiteSpace(stat.ContentType) ? "application/octet-stream" : stat.ContentType;
            return (memoryStream, contentType);
        }
    }
}
