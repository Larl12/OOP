CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    full_name TEXT NOT NULL,
    department TEXT NOT NULL,
    position TEXT NOT NULL,
    salary NUMERIC(10, 2) NOT NULL CHECK(salary >= 0),
    hire_date DATE NOT NULL CHECK(hire_date <= CURRENT_DATE)
);

-- Тестовые данные
INSERT INTO employees (full_name, department, position, salary, hire_date) VALUES
('Игорь Петров', 'Sales', 'Senior Sales Manager', 60000, '2020-05-15'),
('Анна Иванова', 'Marketing', 'Content Specialist', 45000, '2017-01-01'),
('Максим Сидоров', 'Support', 'IT Trainee', 30000, '2014-12-31'),
('Елена Смирнова', 'HR', 'Recruitment Intern', 25000, '2023-06-15'),
('Дмитрий Кузнецов', 'Engineering', 'Software Engineer', 80000, '2019-03-10'),
('Светлана Сергеева', 'Sales', 'Junior Sales Representative', 40000, '2021-08-20'),
('Андрей Федоров', 'Marketing', 'Digital Marketer', 65000, '2022-09-01'),
('Сергей Павлов', 'Marketing', 'Social Media Intern', 35000, '2020-02-15'),
('Ольга Васильева', 'Sales', 'Senior Account Executive', 75000, '2018-01-01'),
('Николай Волков', 'Support', 'Technical Support Analyst', 45000, '2015-01-01'),
('Марина Петрова', 'Human Resources', 'HR Coordinator', 55000, '2021-07-15'),
('Павел Иванов', 'Sales', 'Senior Business Development Manager', 85000, '2022-12-31'),
('Алексей Смирнов', 'Engineering', 'Team Lead', 90000, '2016-04-20'),
('Юлия Кузнецова', 'Marketing', 'Brand Manager', 60000, '2019-11-15'),
('Антон Васильев', 'Support', 'Customer Service Trainee', 32000, '2013-06-15'),
('Наталья Федорова', 'Accounting', 'Financial Analyst', 50000, '2023-03-10'),
('Александр Александров', 'Sales', 'Regional Sales Director', 100000, '2020-06-25'),
('Виктор Александрович', 'IT', 'DevOps Engineer', 70000, '2017-08-10'),
('Алина Алексеевна', 'Marketing', 'SEO Specialist', 55000, '2021-02-15'),
('Артём Сергеевич', 'Design', 'UI Designer', 65000, '2022-04-20'),
('Михаил Дмитриевич', 'Legal', 'Lawyer', 80000, '2019-09-15'),
('Галина Викторовна', 'Administration', 'Office Manager', 45000, '2023-01-15'),
('Денис Алексеевич', 'Sales', 'Senior Product Manager', 90000, '2021-03-10'),
('Роман Андреевич', 'Development', 'Frontend Developer', 75000, '2020-11-15'),
('Дарья Николаевна', 'Management', 'Project Manager', 85000, '2022-05-20'),
('Борис Фёдоров', 'Sales', 'Senior Sales Consultant', 65000, '2020-04-15'),
('Иван Сергеевич', 'Marketing', 'Market Researcher', 55000, '2019-06-15'),
('Сергей Петрович', 'Support', 'Helpdesk Technician', 40000, '2015-01-01'),
('Анна Михайловна', 'Sales', 'Account Manager', 50000, '2022-08-15'),
('Кирилл Иванович', 'HR', 'Talent Acquisition Specialist', 55000, '2021-09-15'),
('Алёна Александровна', 'Engineering', 'Backend Developer', 70000, '2020-02-15'),
('Владимир Евгеньевич', 'Finance', 'Chief Financial Officer', 120000, '2018-03-15'),
('Татьяна Олеговна', 'IT', 'Data Scientist', 80000, '2021-07-15'),
('Евгений Владимирович', 'Research & Development', 'R&D Engineer', 75000, '2022-04-15'),
('Людмила Ивановна', 'Supply Chain Management', 'Logistics Manager', 60000, '2020-05-15');

-- a) Выборка сотрудников (SELECT)
SELECT *
FROM employees
WHERE LOWER(department) LIKE '%sales%' AND
      position ILIKE 'Senior%' AND
      hire_date BETWEEN '2018-01-01' AND '2022-12-31'
ORDER BY full_name ASC;

-- b) Повышение зарплат (UPDATE)
UPDATE employees
SET salary = salary * 1.10
WHERE department = 'Marketing' AND
      salary BETWEEN 50000 AND 70000 AND
      POSITION('Intern' IN position) = 0;

-- c) Удаление сотрудников (DELETE)
DELETE FROM employees
WHERE department = 'Support' AND
      hire_date < '2015-01-01' AND
      POSITION('Trainee' IN position) > 0;