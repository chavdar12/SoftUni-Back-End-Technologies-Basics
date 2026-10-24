INSERT INTO Departments (DepartmentId, Name) VALUES
    (1, 'Engineering'),
    (2, 'Quality Assurance'),
    (3, 'Sales'),
    (4, 'Marketing');

INSERT INTO Employees (EmployeeId, FirstName, LastName, JobTitle, DepartmentId, ManagerId, HireDate, Salary) VALUES
    (1, 'Ivan',    'Petrov',   'CTO',                 1, NULL, '2015-02-01', 9500.00),
    (2, 'Maria',   'Georgieva','Senior Developer',    1, 1,    '2017-06-15', 7200.00),
    (3, 'Georgi',  'Ivanov',   'Developer',           1, 2,    '2021-09-01', 4800.00),
    (4, 'Elena',   'Dimitrova','QA Lead',             2, 1,    '2018-03-12', 6100.00),
    (5, 'Petar',   'Stoyanov', 'QA Engineer',         2, 4,    '2022-01-10', 3900.00),
    (6, 'Anna',    'Koleva',   'QA Engineer',         2, 4,    '2023-05-22', 3600.00),
    (7, 'Nikolay', 'Todorov',  'Sales Representative',3, NULL, '2019-11-04', 3200.00);

INSERT INTO Projects (ProjectId, Name, StartDate, EndDate) VALUES
    (1, 'Online Store',     '2024-01-15', '2024-12-20'),
    (2, 'Mobile App',       '2025-03-01', NULL),
    (3, 'Test Automation',  '2025-06-01', NULL);

INSERT INTO EmployeesProjects (EmployeeId, ProjectId) VALUES
    (2, 1), (3, 1), (5, 1),
    (2, 2), (3, 2),
    (4, 3), (5, 3), (6, 3);
