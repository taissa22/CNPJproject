namespace Shared.Application.Utilidades.Compression
{
    /// <summary>
    /// Type for determine where the Zip component will look for the content to compress.
    /// </summary>
    public enum ZipContentType
    {
        /// <summary>
        /// Zip will compress the content from Files property.
        /// </summary>
        Files,
        /// <summary>
        /// Zip will compress the content from Streams property.
        /// </summary>
        Stream
    }
}
