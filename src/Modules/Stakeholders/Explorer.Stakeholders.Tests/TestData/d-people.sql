INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-11, -11, 'Ana', 'Anić', 'autor1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com');

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-21, -21, 'Pera', 'Perić', 'turista1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-23, -23, 'Steva', 'Stević', 'turista3@gmail.com');

-- New test data ; Nenad ; Profile data
INSERT INTO stakeholders."People" ("Id", "UserId", "Name", "Surname", "Email", "Biography", "Moto", "ImageId")
VALUES (-31, -31, 'Nenad', 'Nenadovic', 'nenad@gmail.com', 'A very nice person!', 'Moto.', -11),
	   (-32, -32, 'Nikola', 'Nikolic', 'nikola@gmail.com', 'A very nice person!', 'Moto.', -12);

