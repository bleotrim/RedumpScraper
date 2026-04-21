namespace RedumpLib;

public class SecuritySectorRange
{
    public int Number { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public string? Note { get; set; } // For possible notes like 'XGD2 (Wave 2)'

    public SecuritySectorRange(int number, int start, int end, string? note = null)
    {
        Number = number;
        Start = start;
        End = end;
        Note = note;
    }
}
