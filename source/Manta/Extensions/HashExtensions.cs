using System.Diagnostics.CodeAnalysis;
using System.IO.Hashing;
using System.Text;

namespace Manta.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class HashExtensions
{
    public static string GetHashHex(this byte[] data)
    {
        var hashBytes = XxHash3.Hash(data);
        return Convert.ToHexStringLower(hashBytes);
    }

    public static string GetHashHex(this string input, Encoding? encoding = null)
    {
        var data = (encoding ?? Encoding.UTF8).GetBytes(input);
        return data.GetHashHex();
    }
}
