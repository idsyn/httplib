using System.IO;

namespace HttpLib
{
    /// <summary>
    /// a simple data structre that holds a file name, and stream
    /// </summary>
    public sealed class NamedFileStream
    {
        /// <summary>
        /// form name for file
        /// </summary>
        public string Name;

        /// <summary>
        /// Name of file
        /// </summary>
        public string Filename;

        /// <summary>
        /// content type of file
        /// </summary>
        public string ContentType;

        /// <summary>
        /// file stream
        /// </summary>
        public Stream Stream;

        public NamedFileStream() { }

        /// <summary>
        /// create a new namedfileStream
        /// </summary>
        /// <param name="name">form name for file</param>
        /// <param name="filename">name of file</param>
        /// <param name="contentType">content type of file</param>
        /// <param name="stream">file stream</param>
        public NamedFileStream(string name, string filename, string contentType, Stream stream)
        {
            Name = name;
            Filename = filename;
            ContentType = contentType;
            Stream = stream;
        }
    }
}
