using EmployeePerformanceApp.Models;
using EmployeePerformanceApp.Services;

namespace EmployeePerformanceApp;

public sealed class MainForm : Form
{
    private readonly TextBox _fullNameTextBox = new();
    private readonly TextBox _departmentTextBox = new();
    private readonly NumericUpDown _kpiInput = new();
    private readonly NumericUpDown _qualityInput = new();
    private readonly NumericUpDown _disciplineInput = new();
    private readonly NumericUpDown _initiativeInput = new();
    private readonly Label _statsLabel = new();
    private readonly DataGridView _grid = new();

    private readonly BindingSource _bindingSource = new();
    private readonly List<EmployeeEvaluation> _evaluations = [];

    public MainForm()
    {
        Text = "Вариант 17 — Оценка и анализ производительности сотрудников";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 1100;
        Height = 700;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(BuildInputPanel(), 0, 0);
        root.Controls.Add(BuildGrid(), 0, 1);
        root.Controls.Add(BuildStatsPanel(), 0, 2);

        Controls.Add(root);
        RefreshStats();
    }

    private Control BuildInputPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 8,
            RowCount = 3,
            Padding = new Padding(0, 0, 0, 10)
        };

        panel.Controls.Add(new Label { Text = "ФИО", AutoSize = true }, 0, 0);
        panel.Controls.Add(_fullNameTextBox, 1, 0);
        panel.SetColumnSpan(_fullNameTextBox, 2);

        panel.Controls.Add(new Label { Text = "Отдел", AutoSize = true }, 3, 0);
        panel.Controls.Add(_departmentTextBox, 4, 0);
        panel.SetColumnSpan(_departmentTextBox, 2);

        ConfigureScoreInput(_kpiInput, "KPI", panel, 0, 1);
        ConfigureScoreInput(_qualityInput, "Качество", panel, 2, 1);
        ConfigureScoreInput(_disciplineInput, "Дисциплина", panel, 4, 1);
        ConfigureScoreInput(_initiativeInput, "Инициативность", panel, 6, 1);

        var addButton = new Button
        {
            Text = "Добавить оценку",
            AutoSize = true,
            Padding = new Padding(8)
        };
        addButton.Click += (_, _) => AddEvaluation();

        var resetButton = new Button
        {
            Text = "Очистить ввод",
            AutoSize = true,
            Padding = new Padding(8)
        };
        resetButton.Click += (_, _) => ClearInputs();

        panel.Controls.Add(addButton, 0, 2);
        panel.SetColumnSpan(addButton, 2);
        panel.Controls.Add(resetButton, 2, 2);
        panel.SetColumnSpan(resetButton, 2);

        var variantLabel = new Label
        {
            AutoSize = true,
            Text = "Формула (вариант 17): 0.4×KPI + 0.3×Качество + 0.2×Дисциплина + 0.1×Инициативность",
            ForeColor = Color.DimGray,
            Padding = new Padding(10, 10, 0, 0)
        };
        panel.Controls.Add(variantLabel, 4, 2);
        panel.SetColumnSpan(variantLabel, 4);

        return panel;
    }

    private void ConfigureScoreInput(NumericUpDown input, string caption, TableLayoutPanel panel, int column, int row)
    {
        input.Minimum = 0;
        input.Maximum = 100;
        input.DecimalPlaces = 0;
        input.Width = 90;

        panel.Controls.Add(new Label { Text = caption, AutoSize = true }, column, row);
        panel.Controls.Add(input, column + 1, row);
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AutoGenerateColumns = true;
        _grid.DataSource = _bindingSource;

        _bindingSource.DataSource = _evaluations;
        return _grid;
    }

    private Control BuildStatsPanel()
    {
        _statsLabel.AutoSize = true;
        _statsLabel.Font = new Font(Font, FontStyle.Bold);
        _statsLabel.Padding = new Padding(0, 10, 0, 0);
        return _statsLabel;
    }

    private void AddEvaluation()
    {
        if (string.IsNullOrWhiteSpace(_fullNameTextBox.Text) || string.IsNullOrWhiteSpace(_departmentTextBox.Text))
        {
            MessageBox.Show("Введите ФИО и отдел сотрудника.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var total = PerformanceCalculator.CalculateTotalScore(_kpiInput.Value, _qualityInput.Value, _disciplineInput.Value, _initiativeInput.Value);
        var level = PerformanceCalculator.GetPerformanceLevel(total);

        _evaluations.Add(new EmployeeEvaluation
        {
            FullName = _fullNameTextBox.Text.Trim(),
            Department = _departmentTextBox.Text.Trim(),
            Kpi = _kpiInput.Value,
            Quality = _qualityInput.Value,
            Discipline = _disciplineInput.Value,
            Initiative = _initiativeInput.Value,
            TotalScore = total,
            PerformanceLevel = level
        });

        _bindingSource.ResetBindings(false);
        RefreshStats();
        ClearInputs();
    }

    private void ClearInputs()
    {
        _fullNameTextBox.Clear();
        _departmentTextBox.Clear();
        _kpiInput.Value = 0;
        _qualityInput.Value = 0;
        _disciplineInput.Value = 0;
        _initiativeInput.Value = 0;
    }

    private void RefreshStats()
    {
        if (_evaluations.Count == 0)
        {
            _statsLabel.Text = "Статистика: данных пока нет.";
            return;
        }

        var avg = _evaluations.Average(e => e.TotalScore);
        var top = _evaluations.MaxBy(e => e.TotalScore);
        var byDepartment = _evaluations
            .GroupBy(e => e.Department)
            .Select(g => $"{g.Key}: {g.Average(x => x.TotalScore):0.00}")
            .ToList();

        _statsLabel.Text =
            $"Статистика: средний балл = {avg:0.00}. Лучший сотрудник: {top?.FullName} ({top?.TotalScore:0.00}). " +
            $"Средний балл по отделам: {string.Join("; ", byDepartment)}";
    }
}
