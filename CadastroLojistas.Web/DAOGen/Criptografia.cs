using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ControleEstoque.Web.DAOGen
{
    /// <summary>
    /// Enumerator com os tipos de classes para criptografia.
    /// </summary>
    public enum CryptProvider
    {
        // Representa a classe base para implementações criptografia dos algoritmos simétricos Rijndael.     
        Rijndael,

        // Representa a classe base para implementações do algoritmo RC2.        
        RC2,

        // Representa a classe base para criptografia de dados padrões (DES - Data Encryption Standard).
        DES,

        // Representa a classe base (TripleDES - Triple Data Encryption Standard).        
        TripleDES
    }
    public class Criptografia
    {
        #region Variáveis e Métodos Privados
        public string _key = string.Empty;                
		public CryptProvider _cryptProvider;
        public SymmetricAlgorithm _algorithm;
        #endregion

        // Construtor com o tipo de criptografia a ser usada Você pode escolher o tipo pelo Enum chamado CryptProvider.
        // Tipo de criptografia
        #region
        public Criptografia(CryptProvider cryptProvider)
        {
            // Seleciona algoritmo simétrico
            switch (cryptProvider)
            {
                case CryptProvider.Rijndael:
                    _algorithm = new RijndaelManaged();
                    _cryptProvider = CryptProvider.Rijndael;
                    break;
                case CryptProvider.RC2:
                    _algorithm = new RC2CryptoServiceProvider();
                    _cryptProvider = CryptProvider.RC2;
                    break;
                case CryptProvider.DES:
                    _algorithm = new DESCryptoServiceProvider();
                    _algorithm.Mode = CipherMode.ECB;
                    _algorithm.Padding = PaddingMode.None;
                    _cryptProvider = CryptProvider.DES;
                    break;
                case CryptProvider.TripleDES:
                    _algorithm = new TripleDESCryptoServiceProvider();
                    _cryptProvider = CryptProvider.TripleDES;
                    break;
            }
            _algorithm.Mode = CipherMode.ECB;
        }
        #endregion

        #region Public methods
        // Encripta o dado solicitado. Texto a ser criptografado        
        public virtual string Encriptar(string texto, string chave)
        {
            byte[] keyByte = ConvertHexToBytes(chave);
            byte[] plainByte = ConvertHexToBytes(texto);

            // Seta a chave privada
            _algorithm.Key = keyByte;

            // Interface de criptografia / Cria objeto de criptografia
            ICryptoTransform cryptoTransform = _algorithm.CreateEncryptor();
            byte[] Retorno = cryptoTransform.TransformFinalBlock(plainByte, 0, plainByte.Length);

            return BitConverter.ToString(Retorno).Replace("-", string.Empty);
        }

        // Texto a ser decriptografado        
        public virtual string Decriptar(string textoCriptografado, string Chave)
        {
            // Converte a base 64 string em num array de bytes                        
            byte[] cryptoByte = ConvertHexToBytes(textoCriptografado);
            byte[] keyByte = ConvertHexToBytes(Chave);

            // Seta a chave privada
            _algorithm.Key = keyByte;

            // Interface de criptografia / Cria objeto de descriptografia
            ICryptoTransform cryptoTransform = _algorithm.CreateDecryptor();			
            byte[] Retorno = cryptoTransform.TransformFinalBlock(cryptoByte, 0, cryptoByte.Length);
		
            // Converte matriz de bytes em Hexadecimal
            return BitConverter.ToString(Retorno).Replace("-", string.Empty);
        }

        public virtual byte[] ConvertHexToBytes(string input)
        {
            var result = new byte[(input.Length + 1) / 2];
            var offset = 0;
            if (input.Length % 2 == 1)
            {
                // If length of input is odd, the first character has an implicit 0 prepended.
                result[0] = (byte)Convert.ToUInt32(input[0] + "", 16);
                offset = 1;
            }
            for (int i = 0; i < input.Length / 2; i++)
            {
                result[i + offset] = (byte)Convert.ToUInt32(input.Substring(i * 2 + offset, 2), 16);
            }
            return result;
        }
        #endregion
    }
}