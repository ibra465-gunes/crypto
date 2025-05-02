using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class AesEncryption
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Anahtarı girin:");
        string key = Console.ReadLine();

        Console.WriteLine("Güvenlik seviyesini girin (128, 192, 256):");
        int keySize = int.Parse(Console.ReadLine());

        Console.WriteLine("Şifreleme mi, deşifreleme mi yapmak istiyorsunuz? (e/d):");
        string choice = Console.ReadLine();

        if (choice.ToLower() == "e")
        {
            EncryptFile("C:\\Users\\elma\\Desktop\\Aes\\Aes\\1.png", "encrypted_image.dat", key, keySize); // Görsel dosyası için
            EncryptFile("C:\\Users\\elma\\Desktop\\Aes\\Aes\\1.mp3", "encrypted_audio.dat", key, keySize); // Ses dosyası için
        }
        else if (choice.ToLower() == "d")
        {
            DecryptFile("encrypted_image.dat", "decrypted.jpg", key, keySize); // Görsel dosyası için
            DecryptFile("encrypted_audio.dat", "decrypted.mp3", key, keySize); // Ses dosyası için
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
        }
    }

    public static void EncryptFile(string inputFile, string outputFile, string key, int keySize)
    {
        byte[] keyBytes = new byte[keySize / 8];
        Array.Copy(Encoding.UTF8.GetBytes(key), keyBytes, Math.Min(keyBytes.Length, key.Length));
        FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        Aes aesAlg = Aes.Create();
        try
        {
            aesAlg.KeySize = keySize;
            aesAlg.Key = keyBytes;
            aesAlg.GenerateIV();
            fsEncrypted.Write(aesAlg.IV, 0, aesAlg.IV.Length);
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            CryptoStream csEncrypt = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);
            fsInput.CopyTo(csEncrypt);
            csEncrypt.FlushFinalBlock();
        }
        finally
        {
            fsInput.Close();
            fsEncrypted.Close();
            aesAlg.Dispose();
        }
    }

    public static void DecryptFile(string inputFile, string outputFile, string key, int keySize)
    {
        byte[] keyBytes = new byte[keySize / 8];
        Array.Copy(Encoding.UTF8.GetBytes(key), keyBytes, Math.Min(keyBytes.Length, key.Length));
        FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        Aes aesAlg = Aes.Create();
        CryptoStream csDecrypt = null;
        try
        {
            aesAlg.KeySize = keySize;
            aesAlg.Key = keyBytes;
            byte[] ivBytes = new byte[aesAlg.BlockSize / 8];
            fsInput.Read(ivBytes, 0, ivBytes.Length);
            aesAlg.IV = ivBytes;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            csDecrypt = new CryptoStream(fsDecrypted, decryptor, CryptoStreamMode.Write);
            fsInput.CopyTo(csDecrypt);
        }
        finally
        {
            if (csDecrypt != null)
                csDecrypt.Close();
            fsInput.Close();
            fsDecrypted.Close();
            aesAlg.Dispose();
        }
    }
}
