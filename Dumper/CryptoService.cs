using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///CryptoService 的摘要说明
/// </summary>
public class CryptoService
{
    public CryptoService()
    {

    }

    #region 加密

    public static string Encrypt(string data)
    {
        return Encrypt(System.Text.Encoding.UTF8.GetBytes(data));
    }

    public static string Encrypt(byte[] data)
    {
        byte[] V_0, V_1;

        byte[] V_2 = InternalEncrypt(data, out V_0, out V_1);
        byte[] V_3 = System.BitConverter.GetBytes(V_0.Length);
        byte[] V_4 = System.BitConverter.GetBytes(V_1.Length);

        int V_5 = V_3.Length + V_4.Length + V_0.Length + V_1.Length + V_2.Length;

        byte[] V_6 = new byte[V_5];

        int V_7 = 0;

        V_3.CopyTo(V_6, V_7);

        V_4.CopyTo(V_6, V_7 += V_3.Length);

        V_0.CopyTo(V_6, V_7 += V_4.Length);

        V_1.CopyTo(V_6, V_7 += V_0.Length);

        V_2.CopyTo(V_6, V_7 += V_1.Length);

        Array.Reverse(V_6);

        V_6 = System.Text.Encoding.UTF8.GetBytes(Convert.ToBase64String(V_6));

        Array.Reverse(V_6);

        return Convert.ToBase64String(V_6);
    }

    public static byte[] InternalEncrypt(byte[] data, out byte[] key, out byte[] IV)
    {
        byte[] bytes = null;
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        System.Security.Cryptography.ICryptoTransform trans = GetEncServiceProvider(out key, out IV);
        System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, trans, System.Security.Cryptography.CryptoStreamMode.Write);

        try
        {
            cs.Write(data, 0, data.Length);

            if (cs != null)
            {
                cs.FlushFinalBlock();
                cs.Close();
            }
            bytes = ms.ToArray();
        }
        finally
        {
            if (cs != null)
            {
                cs.Close();
            }
            if (ms != null)
            {
                ms.Close();
            }
        }

        return bytes;
    }

    public static System.Security.Cryptography.ICryptoTransform GetEncServiceProvider(out byte[] key, out byte[] IV)
    {
        System.Security.Cryptography.TripleDESCryptoServiceProvider obj = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
        obj.Mode = System.Security.Cryptography.CipherMode.CBC;
        key = obj.Key;
        IV = obj.IV;
        return obj.CreateEncryptor();
    }

    #endregion

    #region 解密

    public static string Decrypt(string data)
    {
        return Decrypt(Convert.FromBase64String(data));
    }

    public static string Decrypt(byte[] data)
    {
        Array.Reverse(data);
        data = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(data));
        Array.Reverse(data);
        int loc1 = BitConverter.ToInt32(data, 0);
        int loc2 = BitConverter.ToInt32(data, 4);
        byte[] loc3 = CopyData(data, 8, loc1);
        byte[] V_4 = CopyData(data, 8 + loc1, loc2);
        int V_5 = (8 + loc1) + loc2;
        byte[] V_6 = CopyData(data, V_5, data.Length - V_5);
        return System.Text.Encoding.UTF8.GetString(InternalDecrypt(V_6, loc3, V_4));
    }

    public static byte[] CopyData(byte[] src, int index, int length)
    {
        byte[] buffer = new byte[length];
        for (int i = index; i < (index + length); i++)
        {
            buffer[i - index] = src[i];
        }
        return buffer;
    }

    public static byte[] InternalDecrypt(byte[] data, byte[] key, byte[] IV)
    {
        System.IO.MemoryStream loc0 = new System.IO.MemoryStream();
        System.Security.Cryptography.ICryptoTransform loc1 = GetDecServiceProvier(key, IV);
        System.Security.Cryptography.CryptoStream loc2 = new System.Security.Cryptography.CryptoStream(loc0, loc1, System.Security.Cryptography.CryptoStreamMode.Write);
        loc2.Write(data, 0, data.Length);

        if (loc2 != null)
        {
            loc2.FlushFinalBlock();
            loc2.Close();
        }

        byte[] loc3 = loc0.ToArray();
        if (loc0 != null)
        {
            loc0.Close();
        }

        return loc3;
    }

    public static System.Security.Cryptography.ICryptoTransform GetDecServiceProvier(byte[] key, byte[] IV)
    {
        System.Security.Cryptography.TripleDESCryptoServiceProvider loc0 = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
        loc0.Mode = System.Security.Cryptography.CipherMode.CBC;
        return loc0.CreateDecryptor(key, IV);
    }

    #endregion
}
