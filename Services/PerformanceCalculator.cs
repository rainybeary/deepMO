namespace EmployeePerformanceApp.Services;

public static class PerformanceCalculator
{
    // Вариант 17: интегральная оценка = 40% KPI + 30% Качество + 20% Дисциплина + 10% Инициативность
    public static decimal CalculateTotalScore(decimal kpi, decimal quality, decimal discipline, decimal initiative)
        => Math.Round((kpi * 0.40m) + (quality * 0.30m) + (discipline * 0.20m) + (initiative * 0.10m), 2);

    public static string GetPerformanceLevel(decimal totalScore) => totalScore switch
    {
        >= 90m => "Отличная",
        >= 75m => "Хорошая",
        >= 60m => "Удовлетворительная",
        _ => "Требует улучшений"
    };
}
