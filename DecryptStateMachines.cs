using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class Program {
	private static void Main(string[] args) {
		byte[] decrypted;
		if (args.Length < 1) {
			Console.Error.WriteLine("Program expects exactly one argument (file path)!");
			return;
		}

		string filePath = args[0];

		if (!File.Exists(filePath)) {
			Console.Error.WriteLine("File " + filePath + " was not found!");
			return;
		}

		byte[] encrypted = File.ReadAllBytes(filePath);
		byte[] bytes = Encoding.UTF8.GetBytes("1234567812345678");
		byte[] bytes2 = new PasswordDeriveBytes("statemachines", null).GetBytes(256 / 8);
		RijndaelManaged val = new RijndaelManaged();
		val.Mode = (CipherMode) 4;
		ICryptoTransform val2 = val.CreateDecryptor(bytes2, bytes);

		using(MemoryStream memoryStream = new MemoryStream(encrypted)) {
			CryptoStream val3 = new CryptoStream(memoryStream, val2, 0);
			try {
				byte[] array = new byte[encrypted.Length];
				val3.Read(array, 0, array.Length);
				decrypted = array;
			}
			finally {
				val3 ? .Dispose();
			}
		}
		string xml = Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);

		File.WriteAllText(Path.GetFileNameWithoutExtension(filePath) + ".xml", xml);
	}
}
