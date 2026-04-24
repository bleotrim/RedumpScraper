public class LibCryptSector
{
    public string Sector { get; set; }
    public string Msf { get; set; }
    public string Contents { get; set; }
    public string Xor { get; set; }
    public string Comments { get; set; }

    public LibCryptSector(string Sector, string Msf, string Contents, string Xor, string Comments)
    {
        this.Sector = Sector;
        this.Msf = Msf;
        this.Contents = Contents;
        this.Xor = Xor;
        this.Comments = Comments;
    }
}
