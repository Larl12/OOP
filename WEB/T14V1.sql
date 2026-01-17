CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    full_name TEXT NOT NULL,
    email TEXT NOT NULL,
    bio TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

DO $$
DECLARE
    i INT := 0;
BEGIN
    WHILE i < 10000 LOOP
        INSERT INTO users(full_name, email, bio)
        VALUES (
            format('User %s', i),
            format('user%s@example.com', i),
            substring(md5(random()::text), 1, 100)
        );
        i := i + 1;
    END LOOP;
END $$;

EXPLAIN ANALYZE
SELECT *
FROM users
WHERE email = 'user1234@example.com';

CREATE INDEX idx_email ON users(email);

EXPLAIN ANALYZE
SELECT *
FROM users
WHERE email = 'user1234@example.com';

EXPLAIN ANALYZE
SELECT *
FROM users
WHERE bio ILIKE '%abcdef%';

CREATE INDEX idx_bio_ilike ON users ((lower(bio)));

EXPLAIN ANALYZE
SELECT *
FROM users
WHERE lower(bio) LIKE '%abcdef%';