-- Full names and salaries of employees earning more than 5000, highest first.
SELECT FirstName || ' ' || LastName AS FullName, Salary
FROM Employees
WHERE Salary > 5000
ORDER BY Salary DESC;
