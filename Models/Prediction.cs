namespace Lab07_RazorPages.Models;

public record Prediction(
    int Id,
    string Sign,
    string Category,
    string Text,
    int LuckyNumber,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
