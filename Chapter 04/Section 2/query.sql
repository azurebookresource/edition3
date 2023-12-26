-- Create a database
CREATE DATABASE petstore;
USE petstore;

-- Create a table 
CREATE TABLE pet (id INTEGER, type VARCHAR(50), breed VARCHAR(50), age INTEGER);

-- insert rows
INSERT INTO pet VALUES (1, 'dog', 'poodle', 2);
INSERT INTO pet VALUES (2, 'dog', 'Maltese', 4);
INSERT INTO pet VALUES (3, 'cat', 'Persian', 6);

-- Read
SELECT * FROM pet;

-- Update
UPDATE pet SET age = 3 WHERE id = 1;
SELECT * FROM pet;

-- Delete
DELETE FROM pet WHERE id = 2;
SELECT * FROM pet;