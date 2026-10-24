-- Projects that are still running, with the number of people on each.
SELECT p.Name AS Project, COUNT(ep.EmployeeId) AS TeamSize
FROM Projects AS p
LEFT JOIN EmployeesProjects AS ep ON ep.ProjectId = p.ProjectId
WHERE p.EndDate IS NULL
GROUP BY p.ProjectId, p.Name
ORDER BY TeamSize DESC, Project;
