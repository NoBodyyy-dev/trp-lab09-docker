namespace Lab07_RazorPages.Models;

public static class StudentInfo
{
    public const string Name = "Богданов Д. С.";
    public const string Group = "БИСТ-24-ПО-3";
    public const int Id = 2405759;
    public const int TotalVariants = 21;

    public static int Variant => CalculateVariant(Id, TotalVariants);

    public static int CalculateVariant(int id, int total) => id % total + 1;
}
