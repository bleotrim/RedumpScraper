public class DiscRing
{
    public string Number { get; set; }
    public string MasteringCode { get; set; }
    public string MasteringSidCode { get; set; }
    public string Toolstamp { get; set; }
    public string MouldSidCode { get; set; }
    public string Status { get; set; }

    public DiscRing(string Number, string MasteringCode, string MasteringSidCode, string Toolstamp, string MouldSidCode, string Status = "")
    {
        this.Number = Number;
        this.MasteringCode = MasteringCode;
        this.MasteringSidCode = MasteringSidCode;
        this.Toolstamp = Toolstamp;
        this.MouldSidCode = MouldSidCode;
        this.Status = Status;
    }
}
