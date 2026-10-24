using Microsoft.Data.Sqlite;

namespace Databases.Tests;

// Each test builds a fresh in-memory database from the schema and seed scripts, then runs one query script.
public class SqlQueriesTests
{
    private SqliteConnection _connection = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        Execute(Script("00-schema.sql"));
        Execute(Script("01-seed.sql"));
    }

    [TearDown]
    public void TearDown() => _connection.Dispose();

    [Test]
    public void AllDepartments_AreSortedByName()
    {
        var rows = Query(Script("02-all-departments.sql"));

        Assert.That(rows.Select(r => r[0]), Is.EqualTo(new object[] { "Engineering", "Marketing", "Quality Assurance", "Sales" }));
    }

    [Test]
    public void EmployeesWithSalaryOver5000_AreSortedBySalaryDescending()
    {
        var rows = Query(Script("03-employees-with-salary-over-5000.sql"));

        Assert.That(rows.Select(r => r[0]), Is.EqualTo(new object[] { "Ivan Petrov", "Maria Georgieva", "Elena Dimitrova" }));
        Assert.That(rows.Select(r => Convert.ToDecimal(r[1])), Is.Ordered.Descending);
    }

    [Test]
    public void EmployeesPerDepartment_IncludesEmptyDepartments()
    {
        var rows = Query(Script("04-employees-per-department.sql"));

        Assert.That(rows, Has.Count.EqualTo(4));
        Assert.That(rows[0], Is.EqualTo(new object[] { "Engineering", 3L, 7166.67 }));
        Assert.That(rows[1], Is.EqualTo(new object[] { "Quality Assurance", 3L, 4533.33 }));
        Assert.That(rows[3], Is.EqualTo(new object[] { "Marketing", 0L, 0.0 }));
    }

    [Test]
    public void EmployeesWithManagers_ShowsNoManagerForTopLevel()
    {
        var rows = Query(Script("05-employees-with-managers.sql"));

        Assert.That(rows[0], Is.EqualTo(new object[] { "Ivan", "No manager" }));
        Assert.That(rows[2], Is.EqualTo(new object[] { "Georgi", "Maria" }));
        Assert.That(rows[6], Is.EqualTo(new object[] { "Nikolay", "No manager" }));
    }

    [Test]
    public void ActiveProjects_ExcludeFinishedOnes()
    {
        var rows = Query(Script("06-active-projects.sql"));

        Assert.That(rows, Is.EqualTo(new[]
        {
            new object[] { "Test Automation", 3L },
            new object[] { "Mobile App", 2L },
        }));
    }

    [Test]
    public void RaiseQaSalaries_OnlyChangesQaDepartment()
    {
        var affected = Execute(Script("07-raise-qa-salaries.sql"));

        Assert.That(affected, Is.EqualTo(3));
        Assert.That(Scalar("SELECT Salary FROM Employees WHERE EmployeeId = 4"), Is.EqualTo(6710.0).Within(0.001));
        Assert.That(Scalar("SELECT Salary FROM Employees WHERE EmployeeId = 1"), Is.EqualTo(9500.0));
    }

    [Test]
    public void Schema_RejectsNonPositiveSalary()
    {
        Assert.Throws<SqliteException>(() => Execute(
            "INSERT INTO Employees (FirstName, LastName, JobTitle, DepartmentId, HireDate, Salary) " +
            "VALUES ('Bad', 'Data', 'Intern', 1, '2026-01-01', 0)"));
    }

    private static string Script(string name) =>
        File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Sql", name));

    private int Execute(string sql)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteNonQuery();
    }

    private double Scalar(string sql)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        return Convert.ToDouble(command.ExecuteScalar());
    }

    private List<object[]> Query(string sql)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        using var reader = command.ExecuteReader();
        var rows = new List<object[]>();
        while (reader.Read())
        {
            var row = new object[reader.FieldCount];
            reader.GetValues(row);
            rows.Add(row);
        }
        return rows;
    }
}
