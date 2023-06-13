# Script

````SQL
CREATE TABLE authors(id serial PRIMARY KEY, first_name varchar(100), middle_name varchar(100), last_name varchar(100));
CREATE TABLE books(id serial PRIMARY KEY, title varchar(200), author_id int references authors(id));

INSERT INTO authors (first_name, middle_name, last_name) VALUES ('Edward', 'Morgan', 'Forster'), ('Lewis', null, 'Carrol'), ('Bram', null, 'Stocker'), ('Henry', null, 'Fielding'), ('George', null, 'Eliot'), ('Herman', null, 'Melville'), ('Arthur', 'Conan', 'Doyle'), ('William', null, 'Shakespeare'), ('Homer', null, 'Homer'), ('Oscar', null, 'Wilde'), ('William', 'Sydney', 'Porter'), ('Herbert', 'George', 'Wells');

INSERT INTO books (title, author_id) VALUES 
('A Room with a View', 1),
('Alice''s Adventures in Wonderland', 2),
('Dracula', 3),
('History of Tom Jones, a Foundling', 4),
('Middlemarch', 5),
('Moby Dick; Or, The Whale', 6),
('The Adventures of Sherlock Holmes', 7),
('The Complete Works of William Shakespeare', 8),
('The Iliad', 9),
('The Picture of Dorian Gray', 10),
('The Trimmed Lamp, and Other Stories of the Four Million', 11),
('The War of the Worlds', 12);

SELECT a.first_name, a.middle_name, a.last_name, b.title
FROM authors a
INNER JOIN books b ON a.id = b.author_id
WHERE soundex(a.first_name) = soundex('wilyam')
    OR soundex(a.last_name) = soundex('wilyam');

CREATE INDEX books_title_trigram ON books USING gist (title gist_trgm_ops);

SELECT title FROM books WHERE title % 'The Adventures of Sherlock Holmes';
SELECT title FROM books WHERE title % 'Though Advnturs of Serlok Holms';
````
