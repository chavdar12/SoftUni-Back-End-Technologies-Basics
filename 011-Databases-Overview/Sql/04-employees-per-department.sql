-- Number of employees and average salary per department, including empty departments.
SELECT d.Name AS Department, COUNT(e.EmployeeId) AS EmployeesCount, ROUND(COALESCE(AVG(e.Salary), 0), 2) AS AverageSalary
FROM Departments AS d
LEFT JOIN Employees AS e ON e.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentId, d.Name
ORDER BY EmployeesCount DESC, Department;
