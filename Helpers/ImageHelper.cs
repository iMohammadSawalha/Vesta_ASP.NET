namespace Vesta.Helpers
{
    public static class ImageUrlValidator
    {
        public static async Task IsValidImageUrl(string imageUrl, int maxImageSize_MB =  2)
        {
            int MaxImageSizeInBytes = maxImageSize_MB *  1024 *  1024;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Head, imageUrl);
                    using (var response = await client.SendAsync(request))
                    {
                        long contentLength = response.Content.Headers.ContentLength ?? -1;
                        var responseMediaType = response.Content.Headers.ContentType?.MediaType;
                        if (responseMediaType != null && responseMediaType.StartsWith("image/"))
                        {
                            if (contentLength <= MaxImageSizeInBytes)
                                return;
                            throw new Exception($"The provided image is larger than the maximum allowed size!: {maxImageSize_MB}MB.");
                        }
                        else
                        {
                            throw new Exception($"The provided image Url does not correspond to an image!.");
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
