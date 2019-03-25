using System;
using System.IO;

namespace Yetkilim.Web.Helpers
{
	public class FileHelper
	{
		public FileHelper()
		{
		}

		public static string GetUniqueFileName(string fileName)
		{
			fileName = Path.GetFileName(fileName);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			Guid guid = Guid.NewGuid();
			return string.Concat(fileNameWithoutExtension, "_", guid.ToString().Substring(0, 4), Path.GetExtension(fileName));
		}
	}
}