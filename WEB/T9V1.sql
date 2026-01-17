DROP TABLE IF EXISTS item CASCADE;

CREATE TABLE item (
    id           serial PRIMARY KEY,
    name         text,
    cost_cents   integer,
    created_at   date DEFAULT CURRENT_DATE,
    deprecated   boolean DEFAULT false
);

INSERT INTO item (name, cost_cents, deprecated) VALUES
('Notebook',     125000, false),
('Smartphone',    89999, false),
('Headphones',     1599, false),
('TV',           459999, false),
('Tablet',       219900, false),
('Cable',            99, true),
('Keyboard',       4599, false),
('Mouse',          2999, false),
('Monitor',      119999, false),
('Webcam',        25999, false),
('Microphone',    34999, false),
('Speaker',       49999, false),
('Charger',        1299, false),
('Flash Drive',    1899, false),
('Power Bank',    45999, false),
('SSD 1TB',      219999, false),
('Router',        89999, false),
('Printer',      169999, false),
('Scanner',      139999, true),
('Projector',    299999, false);

ALTER TABLE item RENAME TO products;

ALTER TABLE products ALTER COLUMN cost_cents TYPE numeric(10,2) USING cost_cents / 100.0;

ALTER TABLE products RENAME COLUMN cost_cents TO price;

ALTER TABLE products ADD COLUMN sku text;

UPDATE products SET sku = 'SKU-' || lpad(id::text, 5, '0');

ALTER TABLE products DROP COLUMN deprecated;