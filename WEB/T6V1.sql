CREATE SCHEMA IF NOT EXISTS library;

SET search_path TO library;

-- Таблица authors
CREATE TABLE authors (
    author_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    birth_year INTEGER DEFAULT NULL
);

-- Таблица books
CREATE TABLE books (
    book_id SERIAL PRIMARY KEY,
    title VARCHAR(255) UNIQUE NOT NULL,
    page_count INT CHECK(page_count > 0),
    price DECIMAL(8, 2) DEFAULT 0.00
);

ALTER TABLE books ALTER COLUMN price SET DEFAULT 0.00;

RESET search_path;