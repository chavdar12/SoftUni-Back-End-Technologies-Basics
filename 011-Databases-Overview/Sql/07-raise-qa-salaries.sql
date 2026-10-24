-- Give the Quality Assurance department a 10% raise.
UPDATE Employees
SET Salary = Salary * 1.10
WHERE DepartmentId = (SELECT DepartmentId FROM Departments WHERE Name = 'Quality Assurance');
