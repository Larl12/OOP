CREATE TABLE products (
    product_id SERIAL PRIMARY KEY,
    product_name VARCHAR(255) NOT NULL,
    price NUMERIC(10, 2) CHECK(price >= 0),
    quantity_in_stock INT CHECK(quantity_in_stock >= 0)
);

INSERT INTO products (product_name, price, quantity_in_stock) VALUES
('Смартфон', 29999.99, 10),
('Ноутбук', 59999.99, 5),
('Планшет', 19999.99, 20),
('Фитнес-браслет', 2999.99, 50);