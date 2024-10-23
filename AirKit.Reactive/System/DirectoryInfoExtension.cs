namespace AirKit.Reactive.System;

public static class DirectoryInfoExtension
{
    public static FileInfo CreateFile(this DirectoryInfo directoryInfo, string fileName)
    {
        var filePath = Path.Combine(directoryInfo.FullName, fileName);
        return new(filePath);
    }
}
