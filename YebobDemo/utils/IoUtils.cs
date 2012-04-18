using System.IO;

namespace YebobDemo.Utils
{
	internal sealed class IoUtils
	{
        public static void CopyStream(Stream source, Stream destination)
        {
            int bytesCount;
            byte[] buffer = new byte[0x1000];
            while ((bytesCount = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, bytesCount);
            }
        }
	}
}