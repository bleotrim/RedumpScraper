public class DiscRing
{
    public string? Number { get; set; } = null;
    public string? MasteringCode { get; set; } = null;
    public string? MasteringSidCode { get; set; } = null;
    public string? Toolstamp { get; set; } = null;
    public string? MouldSidCode { get; set; } = null;
    public string? Status { get; set; } = null;
    public string? AdditionalMouldText { get; set; } = null;
    public string? WriteOffset { get; set; } = null;

    public DiscRing(string? Number = null, string? MasteringCode = null, string? MasteringSidCode = null, string? Toolstamp = null, string? MouldSidCode = null, string? Status = null, string? AdditionalMouldText = null, string? WriteOffset = null)
    {
        this.Number = Number;
        this.MasteringCode = MasteringCode;
        this.MasteringSidCode = MasteringSidCode;
        this.Toolstamp = Toolstamp;
        this.MouldSidCode = MouldSidCode;
        this.Status = Status;
        this.AdditionalMouldText = AdditionalMouldText;
        this.WriteOffset = WriteOffset;
    }
}

//TODO: add test for rings values using redump ids: 2 and 811