DROP TABLE IF EXISTS orders;
CREATE TABLE orders (
  order_id       SERIAL PRIMARY KEY,
  order_date     DATE        NOT NULL,
  region          TEXT        NOT NULL CHECK (region IN ('North', 'South', 'East', 'West')),
  channel         TEXT        NOT NULL CHECK (channel IN ('online', 'store', 'partner')),
  payment_method  TEXT        NOT NULL CHECK (payment_method IN ('card', 'cash', 'transfer')),
  status          TEXT        NOT NULL CHECK (status IN ('new', 'paid', 'shipped', 'cancelled', 'returned')),
  amount          NUMERIC(10,2) NOT NULL CHECK (amount >= 0),
  shipping_fee    NUMERIC(10,2) NOT NULL DEFAULT 0 CHECK (shipping_fee >= 0),
  discount        NUMERIC(10,2) NOT NULL DEFAULT 0 CHECK (discount >= 0)
);

INSERT INTO orders (order_date,region,channel,payment_method,status,amount,shipping_fee,discount) VALUES
('2025-01-05','North','online','card','paid',        180, 10,  20),
('2025-01-06','North','store','cash','paid',         220,  0,  15),
('2025-01-07','North','partner','transfer','shipped',260, 15,  30),
('2025-01-10','North','online','card','cancelled',   140,  0,   0),
('2025-01-12','North','store','cash','returned',     190,  0,  10),
('2025-02-03','South','online','card','paid',        120,  8,   0),
('2025-02-04','South','store','cash','paid',         210,  0,  20),
('2025-02-05','South','partner','transfer','new',    130,  5,   0),
('2025-02-08','South','online','transfer','shipped', 175, 12,  15),
('2025-02-11','South','store','card','returned',     160,  0,   5),
('2025-03-02','East','online','card','paid',         95,   9,   0),
('2025-03-03','East','store','cash','paid',          205,  0,  25),
('2025-03-04','East','partner','transfer','shipped', 240, 15,  20),
('2025-03-06','East','online','card','new',          110,  7,   0),
('2025-03-09','East','store','cash','cancelled',     180,  0,   0),
('2025-04-01','West','online','transfer','paid',     155, 10,   0),
('2025-04-02','West','store','cash','paid',          260,  0,  30),
('2025-04-03','West','partner','card','paid',        300, 20,  40),
('2025-04-05','West','online','card','shipped',      220, 12,  10),
('2025-04-07','West','store','cash','returned',      210,  0,  15),
('2025-04-10','South','partner','transfer','paid',   280, 18,  35),
('2025-04-12','North','online','card','paid',        130,  9,   0),
('2025-04-14','East','partner','card','returned',    200, 10,  20),
('2025-04-15','South','online','card','cancelled',   160,  0,   0);

-- Задача 1
SELECT
    order_id,
    status,
    region,
    CASE
        WHEN status IN ('paid', 'shipped')
        THEN amount - discount + shipping_fee
        ELSE 0
    END AS net_revenue,
    CASE
        WHEN amount <= 150 THEN 'low'
        WHEN amount > 150 AND amount <= 220 THEN 'mid'
        ELSE 'high'
    END AS ticket_band
FROM orders
ORDER BY order_date, order_id;

-- Задача 2
SELECT
    order_id,
    channel,
    amount
FROM orders
WHERE amount > CASE
                   WHEN channel = 'online' THEN 150
                   WHEN channel = 'store' THEN 200
                   WHEN channel = 'partner' THEN 180
              END;

-- Задача 3
SELECT
    region,
    COUNT(order_id) AS num_orders,
    SUM(amount) AS total_amount,
    AVG(discount) AS average_discount,
    MAX(shipping_fee) AS max_shipping_fee
FROM orders
GROUP BY region
ORDER BY total_amount DESC;

-- Задача 4
SELECT
    region,
    SUM((amount - discount + shipping_fee)) FILTER (WHERE status IN ('paid', 'shipped')) AS delivered_revenue,
    COUNT(*) FILTER (WHERE status = 'returned') AS returns_cnt,
    ROUND(CAST(COUNT(*) FILTER (WHERE status = 'returned') AS numeric) / COUNT(*) * 100, 2) AS return_rate_percent
FROM orders
GROUP BY region
ORDER BY return_rate_percent DESC;

-- Задача 5
SELECT
    payment_method,
    COUNT(*) AS total_orders,
    COUNT(*) FILTER (WHERE status = 'returned') AS returned_orders,
    SUM(CASE
            WHEN status IN ('paid', 'shipped')
            THEN amount - discount + shipping_fee
            ELSE 0
        END) AS delivered_revenue
FROM orders
GROUP BY payment_method
HAVING
    COUNT(*) FILTER (WHERE status = 'returned')::numeric / COUNT(*) > 0.12 OR
    SUM(CASE
            WHEN status IN ('paid', 'shipped')
            THEN amount - discount + shipping_fee
            ELSE 0
        END) > 800;