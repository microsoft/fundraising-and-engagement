using Microsoft.Extensions.Options;
//using FundraisingandEngagement.Config;
using FundraisingandEngagement.Utils.ConfigModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FundraisingandEngagement.Services
{
    public class SaltString
    {
        public SaltString(IOptions<SaltStringConfig> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public SaltString() { }

        public SaltStringConfig Options { get; } //set only via Secret Manager

        public bool AdminP2PKeyMatched(string input)
        {
            var saltedString = SaltInputString(input);
			return String.Compare(saltedString, Options.SaltedP2PAdminKey, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0;
        }

        public bool ApiKeyMatched(string input)
        {
            var saltedinput = SaltInputString(input);
			return String.Compare(saltedinput, Options.SaltedGatewayAPIKey, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0;
		}

		public string SaltInputString(string input)
        {


            byte[] pwd = Encoding.Unicode.GetBytes(input);

            byte[] salt = CreateRandomSalt(7);

            // Create a TripleDESCryptoServiceProvider object.
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            try
            {
                Console.WriteLine("Creating a key with PasswordDeriveBytes...");

                // Create a PasswordDeriveBytes object and then create
                // a TripleDES key from the password and salt.
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(pwd, salt);


                // Create the key and set it to the Key property
                // of the TripleDESCryptoServiceProvider object.
                tdes.Key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, tdes.IV);

                // On .NET Core, the Default property always returns the UTF8Encoding.
                //return System.Text.Encoding.Unicode.GetString(tdes.Key);

                return System.Text.Encoding.Default.GetString(tdes.Key);

            }
            catch
            {}
            finally
            {
                // Clear the buffers
                ClearBytes(pwd);
                ClearBytes(salt);

                // Clear the key.
                tdes.Clear();
            }
            return string.Empty;
        }

        //////////////////////////////////////////////////////////
        // Helper methods:
        // CreateRandomSalt: Generates a random salt value of the
        //                   specified length.
        //
        // ClearBytes: Clear the bytes in a buffer so they can't
        //             later be read from memory.
        //////////////////////////////////////////////////////////

        public byte[] CreateRandomSalt(int length)
        {
            // Create a buffer
            byte[] randBytes;

            if (length >= 1)
            {
                randBytes = new byte[length];
            }
            else
            {
                randBytes = new byte[1];
            }

            // Create a new RNGCryptoServiceProvider.
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            // Fill the buffer with random bytes.
            rand.GetBytes(randBytes);

            // return the bytes.
            return randBytes;
        }

        public void ClearBytes(byte[] buffer)
        {
            // Check arguments.
            if (buffer == null)
            {
                throw new ArgumentException("buffer");
            }

            // Set each byte in the buffer to 0.
            for (int x = 0; x < buffer.Length; x++)
            {
                buffer[x] = 0;
            }
        }
    }
}
