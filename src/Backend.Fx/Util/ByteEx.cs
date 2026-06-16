using System;
using JetBrains.Annotations;

namespace Backend.Fx.Util;

[PublicAPI]
public static class BytesEx
{
    private const int OneKiloByte = 1000;
    private const int OneMegaByte = 1000 * 1000;
    private const int OneGigaByte = 1000 * 1000 * 1000;
    private const long OneTeraByte = 1000L * 1000 * 1000 * 1000;

    private const int OneKibiByte = 1024;
    private const int OneMebiByte = 1024 * 1024;
    private const int OneGibiByte = 1024 * 1024 * 1024;
    private const long OneTebiByte = 1024L * 1024 * 1024 * 1024;

    // ReSharper disable InconsistentNaming
    public static long TB(this int tb) => tb.EnsurePositive() * OneTeraByte;
    
    public static int GB(this int gb) => gb.EnsurePositive() * OneGigaByte;

    public static int MB(this int mb) => mb.EnsurePositive() * OneMegaByte;

    public static int KB(this int kb) => kb.EnsurePositive() * OneKiloByte;

    public static long TiB(this int tb) => tb.EnsurePositive() * OneTebiByte;

    public static int GiB(this int gb) => gb.EnsurePositive() * OneGibiByte;

    public static int MiB(this int mb) => mb.EnsurePositive() * OneMebiByte;

    public static int KiB(this int kb) => kb.EnsurePositive() * OneKibiByte;
    // ReSharper restore InconsistentNaming

    public static string ToHumanReadable(this short size) => ((long)size.EnsurePositive()).ToHumanReadable();

    public static string ToHumanReadable(this ushort size) => ((long)size.EnsurePositive()).ToHumanReadable();

    public static string ToHumanReadable(this int size) => ((long)size.EnsurePositive()).ToHumanReadable();

    public static string ToHumanReadable(this uint size) => ((long)size.EnsurePositive()).ToHumanReadable();

    public static string ToHumanReadable(this long size) => ((ulong)size.EnsurePositive()).ToHumanReadable();

    public static string ToHumanReadable(this ulong size)
    {
        switch (size)
        {
            case > OneTeraByte:
                return $"{Math.Round((float)size / OneTebiByte, 1)} TB";
            case > OneGigaByte:
                return $"{Math.Round((float)size / OneGibiByte, 1)} GB";
            case > OneMegaByte:
                return $"{Math.Round((float)size / OneMebiByte, 1)} MB";
            case > OneKiloByte:
                return $"{Math.Round((float)size / OneKibiByte, 1)} KB";
            default:
                return $"{size} B";
        }
    }
    
    private static T EnsurePositive<T>(this T size) where T : struct, IComparable<T>
    {
        if (size.CompareTo(default) < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Size cannot be negative");
        }

        return size;
    }
}