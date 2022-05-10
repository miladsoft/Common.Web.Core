using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Common.Web.Core
{
    /// <summary>
    /// Protection Provider Service
    /// </summary>
    public class ProtectionProviderService : IProtectionProviderService
    {
        private readonly ILogger<ProtectionProviderService> _logger;
        private readonly IDataProtector _dataProtector;
        private readonly ISerializationProvider _serializationProvider;

        /// <summary>
        /// The default protection provider
        /// </summary>
        public ProtectionProviderService(
            IDataProtectionProvider dataProtectionProvider,
            ILogger<ProtectionProviderService> logger,
            ISerializationProvider serializationProvider)
        {
            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataProtector = dataProtectionProvider.CreateProtector(typeof(ProtectionProviderService).FullName ?? nameof(ProtectionProviderService));
            _serializationProvider = serializationProvider ?? throw new ArgumentNullException(nameof(serializationProvider));
        }

        /// <summary>
        /// Decrypts the message
        /// </summary>
        public string? Decrypt(string inputText)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException(nameof(inputText));
            }

            try
            {
                var inputBytes = WebEncoders.Base64UrlDecode(inputText);
                var bytes = _dataProtector.Unprotect(inputBytes);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex.Demystify(), "Invalid base 64 string. Fall through.");
            }
            catch (CryptographicException ex)
            {
                _logger.LogError(ex.Demystify(), "Invalid protected payload. Fall through.");
            }

            return null;
        }

        /// <summary>
        /// Encrypts the message
        /// </summary>
        public string Encrypt(string inputText)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException(nameof(inputText));
            }

            var inputBytes = Encoding.UTF8.GetBytes(inputText);
            var bytes = _dataProtector.Protect(inputBytes);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        /// <summary>
        /// It will serialize an object as a JSON string and then encrypt it as a Base64UrlEncode string.
        /// </summary>
        public string EncryptObject(object data)
        {
            return Encrypt(_serializationProvider.Serialize(data));
        }

        /// <summary>
        /// It will decrypt a Base64UrlEncode encrypted JSON string and then deserialize it as an object.
        /// </summary>
        public T? DecryptObject<T>(string data)
        {
            var decryptData = Decrypt(data);
            return decryptData == null ? default : _serializationProvider.Deserialize<T>(decryptData);
        }
    }
}