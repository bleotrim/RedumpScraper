public class PvdRecord
{
    public string Entry { get; set; }
    public string Contents { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string Gmt { get; set; }

    public PvdRecord(string Entry, string Contents, string Date, string Time, string Gmt)
    {
        this.Entry = Entry;
        this.Contents = Contents;
        this.Date = Date;
        this.Time = Time;
        this.Gmt = Gmt;
    }
}
