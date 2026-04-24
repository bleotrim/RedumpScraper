public class DiscTrack
{
    public string Number { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public string Crc32 { get; set; }
    public string Md5 { get; set; }
    public string Sha1 { get; set; }

    public DiscTrack(string Number, string Type, string Size, string Crc32, string Md5, string Sha1)
    {
        this.Number = Number;
        this.Type = Type;
        this.Size = Size;
        this.Crc32 = Crc32;
        this.Md5 = Md5;
        this.Sha1 = Sha1;
    }
}
