-- Every employee with their manager's first name ("No manager" for top-level employees).
SELECT e.FirstName AS Employee, COALESCE(m.FirstName, 'No manager') AS Manager
FROM Employees AS e
LEFT JOIN Employees AS m ON m.EmployeeId = e.ManagerId
ORDER BY e.EmployeeId;
