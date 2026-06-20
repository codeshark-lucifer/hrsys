-- ========================================================
-- HRSYSTEM - FIXED SCHEMA (PHPMYADMIN COMPATIBLE)
-- CODESHARK PRODUCTION DATABASE SETUP WITH ATTENDANCE
-- ========================================================

CREATE DATABASE IF NOT EXISTS hrsys_db;
USE hrsys_db;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS attendance;
DROP TABLE IF EXISTS payroll;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS employees;
DROP TABLE IF EXISTS departments;
SET FOREIGN_KEY_CHECKS = 1;

-- --------------------------------------------------------
-- 1. DEPARTMENTS TABLE
-- --------------------------------------------------------
CREATE TABLE departments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    budget DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------
-- 2. EMPLOYEES TABLE
-- --------------------------------------------------------
CREATE TABLE employees (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_code VARCHAR(20) NOT NULL UNIQUE, -- Filled programmatically via C#
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20) DEFAULT NULL,
    hire_date DATE NOT NULL,
    department_id INT NULL,
    status ENUM('Active', 'Terminated', 'On Leave') NOT NULL DEFAULT 'Active',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (department_id) REFERENCES departments(id) ON DELETE SET NULL,
    INDEX idx_employee_status (status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------
-- 3. ATTENDANCE TABLE
-- --------------------------------------------------------
CREATE TABLE attendance (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT NOT NULL,
    work_date DATE NOT NULL,
    check_in TIME NOT NULL,
    check_out TIME DEFAULT NULL,
    is_late BOOLEAN GENERATED ALWAYS AS (check_in > '08:00:00') STORED,
    punctuality_score INT GENERATED ALWAYS AS (IF(check_in > '08:00:00', 85, 100)) STORED,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    UNIQUE KEY uq_emp_daily_attendance (employee_id, work_date),
    INDEX idx_attendance_date (work_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------
-- 4. PAYROLL TABLE
-- --------------------------------------------------------
CREATE TABLE payroll (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT NOT NULL,
    salary_month DATE NOT NULL,
    base_salary DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    bonuses DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    deductions DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    net_pay DECIMAL(10, 2) GENERATED ALWAYS AS (base_salary + bonuses - deductions) STORED,
    payment_status ENUM('Pending', 'Processed', 'Failed') NOT NULL DEFAULT 'Pending',
    payment_date DATETIME DEFAULT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    UNIQUE KEY uq_employee_month (employee_id, salary_month)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------
-- 5. USERS TABLE
-- --------------------------------------------------------
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT UNIQUE NULL,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role ENUM('Admin', 'HR', 'Finance', 'Employee') NOT NULL DEFAULT 'Employee',
    status ENUM('Active', 'Suspended') NOT NULL DEFAULT 'Active',
    last_login DATETIME DEFAULT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ========================================================
-- DATA SEEDING SECTION
-- ========================================================

-- 1. Seed Departments
INSERT INTO departments (id, name, budget) VALUES 
(1, 'Executive Management', 150000.00),
(2, 'Human Resources', 45000.00),
(3, 'Finance & Accounting', 60000.00),
(4, 'Software Engineering', 250000.00),
(5, 'Quality Assurance', 80000.00);

-- 2. Seed Employees
INSERT INTO employees (id, employee_code, first_name, last_name, email, phone, hire_date, department_id, status) VALUES
(1, 'EMP_001', 'Morm', 'LeapSovann', 'mormleapsovann@gmail.com', '+85512345678', '2025-01-15', 4, 'Active'),
(2, 'EMP_002', 'Alice', 'Smith', 'alice.smith@codeshark.io', '+15550192834', '2025-03-10', 2, 'Active'),
(3, 'EMP_003', 'Bob', 'Johnson', 'bob.johnson@codeshark.io', '+15550123456', '2025-06-01', 3, 'Active'),
(4, 'EMP_004', 'Charlie', 'Dev', 'charlie.dev@codeshark.io', '+15550177665', '2025-09-22', 4, 'Active'),
(5, 'EMP_005', 'David', 'Tester', 'david.qa@codeshark.io', '+15550188992', '2026-02-01', 5, 'Active'),
(6, 'EMP_006', 'Eva', 'Left', 'eva.left@codeshark.io', '+15550144332', '2025-02-14', 1, 'Terminated');

-- 3. Seed Attendance Logs
INSERT INTO attendance (employee_id, work_date, check_in, check_out) VALUES
(1, '2026-06-15', '07:52:00', '17:00:00'),
(2, '2026-06-15', '08:15:00', '17:05:00'),
(3, '2026-06-15', '07:58:00', '17:00:00'),
(4, '2026-06-15', '08:45:00', '17:15:00'),
(1, '2026-06-16', '07:45:00', '17:00:00');

-- 4. Seed Users (FIXED: Added the missing INSERT INTO statement)
INSERT INTO users (id, employee_id, username, email, password_hash, role, status) VALUES
(1, 1, 'codeshark', 'mormleapsovann@gmail.com', '$2a$11$dEGEPwnXOsFTwe8ZjwyeeusEOfPfO0op0LxY7iemiPbLGt2ovTEnW', 'Admin', 'Active'), -- "sovann@1029"
(2, 2, 'alices', 'alice.smith@codeshark.io', '$2a$11$yaliX4pjr0r4i9RoJW7KQeVG8KSmrtf0SMXu/UsyrDdC4290Ma/ti', 'HR', 'Active'),       -- "password123"
(3, 3, 'bobj', 'bob.johnson@codeshark.io', '$2a$11$yaliX4pjr0r4i9RoJW7KQeVG8KSmrtf0SMXu/UsyrDdC4290Ma/ti', 'Finance', 'Active'),   -- "password123"
(4, 4, 'charlied', 'charlie.dev@codeshark.io', '$2a$11$yaliX4pjr0r4i9RoJW7KQeVG8KSmrtf0SMXu/UsyrDdC4290Ma/ti', 'Employee', 'Active'); -- "password123"