/*
 * Salesforce API for .NET
 * 
 * Gaiaware also develops Gaia Ajax - the open source Ajax framework for ASP.NET
 * http://gaiaware.net
 * 
 * Copyright (c) 2009 Gaiaware AS.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gaiaware.Salesforce
{
    public abstract class SforceControllerBase
    {
        protected IEnumerable<T> ConvertQueryResult<T>(QueryResult result) where T : sObject
        {
            var data = new T[result.size];
            for (int i = 0; i < data.Length; i++)
            {
                yield return (T) result.records[i];
            }
        }

        protected IEnumerable<string> ParseSaveResult(SaveResult[] saveResults, out List<string> errors)
        {
            errors = null;
            if (saveResults == null)
                return null;

            var retVal = new List<string>();
            if (saveResults.Length > 0)
            {
                for (int i = 0; i < saveResults.Length; i++)
                    //add to return value if success
                    if (saveResults[i].success)
                        retVal.Add(saveResults[i].id);
                    else
                    {
                        if (errors == null) errors = new List<string>();
                        errors.Add(GetErrorMessage(saveResults[i].errors));
                    }
            }

            return retVal;
        }

        private static string GetErrorMessage(IEnumerable<Error> errors)
        {
            string errorMsg = "";
            foreach (Error err in errors)
                errorMsg += string.Format("Code:{0} - Message:{1}", err.statusCode, err.message);

            return errorMsg;
        }

        protected string ParseFirstSaveResult(params SaveResult[] saveResults)
        {
            if (saveResults == null)
                return string.Empty;

            if ((saveResults.Length == 1) && (saveResults[0].success))
            {
                return saveResults[0].id;
            }

            // fetch all errors and rethrow as exception
            string errors = String.Empty;
            foreach (SaveResult result in saveResults)
                foreach (Error err in result.errors)
                    errors += string.Format("Code:{0} - Message:{1}", err.statusCode, err.message);

            // TODO: find a way to persist the errors to easen support

            if (!String.IsNullOrEmpty(errors))
                throw new Exception(errors);

            // otherwise return empty string
            return string.Empty;
        }

        #region Nested type: CryptoHelper

        protected sealed class CryptoHelper
        {
            private const string _privatekey = @"<RSAKeyValue>
  <Modulus>oYBAgHS60X/3QoBnnK+YdcoPbyDA/rRpPwlNt5cnmI89BT6YQ8C3jNe6OD0IY7lYQBC1BCsjuj+wholXcH7zpzRBgAQ0pet3mEWJTgtI/kPJdDBTbxlF721AMhi5PLLCj5sDIJ0ItXu5nYe/OYlzqQQsZ6Y7TJIrnZIeDK70aeE=</Modulus>
  <Exponent>AQAB</Exponent>
  <P>ziy2+dGDjkW6B2kTVq4xsRP72HIPjuJ3PPaLgjWR2bovhSmZDVYA+zExiJNx2/PpmwhKS+Ofikh/ASDzyl6evw==</P>
  <Q>yIe8sr8FmMAK9cLNfe0WzPeK7SG0VGXwO4OGEuF88TnhgCB47WkMc+dHup0EzwTpUGVN9VSDEoy7Tohdc1m/Xw==</Q>
  <DP>RP8ZzMicqgQTmV9EpYFuB8CUS38ATeTj2nb3gP/Ea4SvWnTtT1U0xttTpE0TkXQy/TrtAjCOt4xDVHFepJ69qQ==</DP>
  <DQ>XSV/51+Hz/5UmPfV0AqTLr5FkAS56QI45swfOSH4kWybbJKo2U6UdDoYPXy2QRs87RVBcxXAlJs+XipFjlE/7Q==</DQ>
  <InverseQ>dpY7Al3HFq7HgcLnvZLWYlU0qYSUSWWkLMZphkA0E+7cB5r1pMfIKx6XjyEuVbib9wWedOHzgHSnCXj9401dmw==</InverseQ>
  <D>WzUnOGS0LSTr627BFUhF/h/DX6tL04zn82W5snl3Lz2V5KRZMcpg3vXmgWRWnJtML8I/uL70Snc1poViEHJh/6Jeyh2k3WpELmKsAMXkjMfEX5Tt8i9no6SlUC+HJA3KHMtJ2+3jUzjuuKDPT1Z+YPNQr6gIhWW9CAm8CzAEDVE=</D>
</RSAKeyValue>";

            private const string _publicKey = @"<RSAKeyValue>
  <Modulus>oYBAgHS60X/3QoBnnK+YdcoPbyDA/rRpPwlNt5cnmI89BT6YQ8C3jNe6OD0IY7lYQBC1BCsjuj+wholXcH7zpzRBgAQ0pet3mEWJTgtI/kPJdDBTbxlF721AMhi5PLLCj5sDIJ0ItXu5nYe/OYlzqQQsZ6Y7TJIrnZIeDK70aeE=</Modulus>
  <Exponent>AQAB</Exponent>
</RSAKeyValue>";

            private byte[] CreateSignature(byte[] plainTextBytes)
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(_privatekey);

                byte[] hash = HashAlgorithm.Create("SHA1").ComputeHash(plainTextBytes);
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA1");
                return formatter.CreateSignature(hash);
            }

            public string GetSignature(string data)
            {
                // retrieve the first 20 characters to be used as the SHA1
                int length = Math.Min(data.Length, 20);
                var plainTextBytes = new byte[20];
                Encoding.UTF8.GetBytes(data, 0, length, plainTextBytes, 0);

                byte[] signature = CreateSignature(plainTextBytes);

                // return the string as a Base64 encoded string
                return string.Format("{0}", Convert.ToBase64String(signature));
            }

            public bool VerifySignature(byte[] data, byte[] signature)
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(_publicKey);

                byte[] hash = HashAlgorithm.Create("SHA1").ComputeHash(data);

                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA1");
                bool messageIsValid = deformatter.VerifySignature(hash, signature);

                return messageIsValid;
            }

            public bool VerifySignature(string data, string publicSignature)
            {
                byte[] signature = Convert.FromBase64String(publicSignature);

                // extract 20 bytes from the signature part to be used as SHA1
                var plainTextBytes = new byte[20];
                int length = Math.Min(data.Length, 20);
                Encoding.UTF8.GetBytes(data, 0, length, plainTextBytes, 0);

                return VerifySignature(plainTextBytes, signature);
            }
        }

        #endregion
    }
}