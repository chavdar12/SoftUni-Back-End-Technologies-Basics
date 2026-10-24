-- Simplified "SoftUni" company database. Written in portable SQL (runs on SQLite, SQL Server and PostgreSQL).
CREATE TABLE Departments (
    DepartmentId INTEGER PRIMARY KEY,
    Name         VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Employees (
    EmployeeId   INTEGER PRIMARY KEY,
    FirstName    VARCHAR(50) NOT NULL,
    LastName     VARCHAR(50) NOT NULL,
    JobTitle     VARCHAR(50) NOT NULL,
    DepartmentId INTEGER NOT NULL REFERENCES Departments (DepartmentId),
    ManagerId    INTEGER NULL REFERENCES Employees (EmployeeId),
    HireDate     DATE NOT NULL,
    Salary       DECIMAL(10, 2) NOT NULL CHECK (Salary > 0)
);

CREATE TABLE Projects (
    ProjectId INTEGER PRIMARY KEY,
    Name      VARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate   DATE NULL
);

CREATE TABLE EmployeesProjects (
    EmployeeId INTEGER NOT NULL REFERENCES Employees (EmployeeId),
    ProjectId  INTEGER NOT NULL REFERENCES Projects (ProjectId),
    PRIMARY KEY (EmployeeId, ProjectId)
);
