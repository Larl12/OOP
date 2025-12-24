-- Создаем схему library
CREATE SCHEMA IF NOT EXISTS library;

-- Переключаемся на схему library
USE library;

-- Таблица authors (авторы)
CREATE TABLE authors (
    author_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    birth_year YEAR NULL -- Год рождения может быть пустым (NULL)
);

-- Таблица books (книги)
CREATE TABLE books (
    book_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) UNIQUE NOT NULL, -- Название должно быть уникальным
    page_count INT DEFAULT 0 CHECK(page_count >= 0),
    price DECIMAL(10, 2) DEFAULT 0.00, -- Цена по умолчанию равна 0
    author_id INT,
    FOREIGN KEY (author_id) REFERENCES authors(author_id) ON DELETE SET NULL
);