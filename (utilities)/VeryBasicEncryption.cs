/*
	C# "VeryBasicEncryption.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Text;
using System.Security.Cryptography;

namespace ReSearcher {

	internal static class VeryBasicEncryption {

		// inspired by implementation on MSDN:
		// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata.protect
		// allow for CryptographicException to bubble-up

		private static Byte[] additionalEntropy = { 1, 9, 8, 7 };

		public static String encrypt(this String thisString) {
			return(
				Convert.ToBase64String(
					ProtectedData.Protect(
						Encoding.Unicode.GetBytes(thisString),
						additionalEntropy,
						DataProtectionScope.CurrentUser
					)
				)
			);
		}

		public static String decrypt(this String thisString) {
			return(
				Encoding.Unicode.GetString(
					ProtectedData.Unprotect(
						Convert.FromBase64String(thisString),
						additionalEntropy,
						DataProtectionScope.CurrentUser
					)
				)
			);
		}

	}

}