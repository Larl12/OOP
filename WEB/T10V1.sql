CREATE TABLE ARTIST (
    id SERIAL PRIMARY KEY,
    name TEXT UNIQUE NOT NULL,
    country TEXT
);

CREATE TABLE ALBUM (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    release_year SMALLINT NOT NULL CHECK(release_year >= 1900 AND release_year <= EXTRACT(YEAR FROM CURRENT_DATE)), -- Ограничение на диапазон годов
    primary_artist_id INTEGER REFERENCES ARTIST(id) ON DELETE RESTRICT NOT NULL
);

CREATE TABLE ALBUM_INFO (
    album_id INTEGER PRIMARY KEY REFERENCES ALBUM(id) ON DELETE CASCADE,
    upc TEXT UNIQUE NOT NULL,
    label TEXT,
    duration_minutes SMALLINT NOT NULL CHECK(duration_minutes > 0)
);

CREATE TABLE ALBUM_CONTRIBUTOR (
    album_id INTEGER REFERENCES ALBUM(id) ON DELETE CASCADE,
    artist_id INTEGER REFERENCES ARTIST(id) ON DELETE CASCADE,
    role TEXT NOT NULL,
    PRIMARY KEY(album_id, artist_id, role)
);


INSERT INTO ARTIST (name, country) VALUES
('Kurt Cobain', 'USA'),
('David Bowie', 'UK'),
('Madonna', 'USA'),
('Elvis Presley', 'USA');

INSERT INTO ALBUM (title, release_year, primary_artist_id) VALUES
('Nevermind', 1991, 1),
('Heroes', 1977, 2),
('Like a Virgin', 1984, 3),
('Jailhouse Rock', 1957, 4);

INSERT INTO ALBUM_INFO (album_id, upc, label, duration_minutes) VALUES
(1, 'ABC123', 'Geffen Records', 50),
(2, 'DEF456', 'RCA Records', 45),
(3, 'GHI789', 'Warner Bros.', 55),
(4, 'JKL012', 'RCA Records', 40);

INSERT INTO ALBUM_CONTRIBUTOR (album_id, artist_id, role) VALUES
(1, 1, 'vocalist'),
(2, 2, 'composer'),
(3, 3, 'performer'),
(4, 4, 'lead singer');

SELECT * FROM ARTIST;
SELECT * FROM ALBUM;
SELECT * FROM ALBUM_INFO;
SELECT * FROM ALBUM_CONTRIBUTOR;