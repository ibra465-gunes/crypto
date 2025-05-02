using System.Numerics;

class RSA
{
    public bool isPrime(int number)
    {
        if (number <= 1) 
            return false;
        if (number == 2)
            return true;
        if (number % 2 == 0) 
            return false;
        for (int i = 3; i < Math.Sqrt(number); i++)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }
    public int producePrime(int min, int max)
    {
        Random r = new Random();
        int number;
        do
        {
            number = r.Next(min, max);
        } while (!isPrime(number));
        return number;
    }
    private int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    public int tersMod(int e, int phi)
    {
        int m0 = phi,x0 = 0,x1 = 1, t, q;
        if (phi == 1)
            return 0;
        while (e > 1)
        {
            q = e / phi;
            t = phi;
            phi = e % phi;
            e = t;
            t = x0;
            x0 = x1 - q * x0;
            x1 = t;
        }
        if (x1 < 0)
            x1 += m0;
        return x1;
    }
    public int[] produceKey()
    {
        Random random = new Random();
        int p = producePrime(2, 46000);
        int q = producePrime(2, 46000);
        int [] key = new int[4];
        int n = p * q;
        key[2] = n;
        int phi = (p - 1) * (q - 1);
        key[3] = phi;
        int e = random.Next(2, phi);
        while (GCD(e, phi) != 1)
        {
            e = random.Next(2, phi);
        }
        key[0] = e;
        int d = tersMod(e, phi);
        key[1] = d;
        return key;
    }
    public int[] enCrypt(string message, int e, int n)
    {
        int[] crypt = new int[message.Length];
        for (int i = 0; i < message.Length; i++)
        {
            int value = (int)message[i];
            BigInteger encryptedValue = BigInteger.ModPow(value, e, n);
            crypt[i] = (int)encryptedValue;
        }
        return crypt;
    }
    public string deCrypt(int[] message, int d, int n, string path)
    {
        string a = "";
        for (int i = 0; i < message.Length; i++)
        {
            int value = message[i];
            BigInteger decryptedValue = BigInteger.ModPow(value, d, n);
            char decryptedChar = (char)(int)decryptedValue;
            a += decryptedChar;
        }
        File.WriteAllText(path, a);
        return a;
    }
    public static void Main()
    {
        RSA rsa = new RSA();
        int [] b = rsa.produceKey();
        int e = b[0], n = b[2];
        string message = File.ReadAllText("C:\\Users\\elma\\Desktop\\RSA\\RSA\\DüzMetin.txt");
        Console.WriteLine(e + " " + n);
        int[] a = rsa.enCrypt(message, e, n);
        File.WriteAllText("C:\\Users\\elma\\Desktop\\RSA\\RSA\\ŞifreliMetin.txt", string.Join(" ", a));
        string path = "C:\\Users\\elma\\Desktop\\RSA\\RSA\\DüzMetin1.txt";
        string c = rsa.deCrypt(a, b[1], b[2], path);
        Console.WriteLine(c);
    }
}
