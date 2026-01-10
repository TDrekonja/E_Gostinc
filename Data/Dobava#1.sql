use E_Gostinc;
select * from AspNetUsers;
/*SELECT ID, Naziv FROM DobavniArtikel ORDER BY ID;
TRUNCATE table DobavaVSkladisce;
TRUNCATE table DobavniArtikel;
TRUNCATE table Skladisce;
TRUNCATE table Dobava;



SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMNPROPERTY(object_id('DobavniArtikel'), COLUMN_NAME, 'IsIdentity') AS IsIdentity
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'DobavniArtikel';
*/
SET IDENTITY_INSERT DobavniArtikel ON;
INSERT INTO DobavniArtikel (ID, Naziv, Enota, VrstaID) VALUES
(32961, 'Whisky Tullamore Dew (40%) stn 1L', '1L', 1),
(39450, 'Vodka Gorbatschov (37,5%) stn 1L', '1L', 1),
(90710, 'Jagermeister pomaranča (33%) stn 0,7L', '0,7L', 1),
(48505, 'Gin Finsbury (37,5%) stn 1L', '1L', 1),
(38078, 'Pivo Union Radler isotonic 0,5L', '0,5L', 6),
(259,   'Pivo Laško točeno Zlatorog svetlo sod 30L', '30L', 6),
(202,   'Coca Cola ploč 0,33L', '0,33L', 5),
(178,   'Ledeni čaj breskev Sola ploč 0,33L', '0,33L', 5),
(185,   'Fanta orange ploč 0,33L', '0,33L', 5),
(145,   'Red Bull ploč 0,25L', '0,25L', 5),
(9936,  'Voda Dana negazirana pet 0,5L', '0,5L', 5),
(1967,  'Jagermeister (35%) stn 1L', '1L', 1),
(692,   'Whisky Jack Daniels old No.7 stn 1L', '1L', 1),
(8294,  'Tequila Sierra gold Reposado (38%) stn 1L', '1L', 1),
(618,   'Pelinkovec grenki Fructal (28%) stn 1L', '1L', 1),
(8543,  'Viljamovka Roner Williams (40%) stn 0,7L', '0,7L', 1),
(28767, 'Borovničevec Fructal (20%) stn 1L', '1L', 1),
(89371, 'Liker limonce Stock (25%) stn 1L', '1L', 1),
(47111, 'Nektar pomaranča Fructal Gemina 1L', '1L', 5),
(47112, 'Nektar jabolko Fructal Gemina 1L', '1L', 5),
(51534, 'Pivo Union Radler grenivka 0,5L', '0,5L', 6),
(62858, 'Brisače papir 2-slojne', 'kos', 9),
(8,     'Voda Radenska gazirana pet 1,5L', '1,5L', 5),
(56831, 'Vrečke za smeti 150L 10kom', 'pak', 9),
(361,   'Pivo Union 6*ploč 0,5L', 'pak', 6),
(54733, 'Kozarci PVC Laško 0,3L (50/1)', 'pak', 8),
(54734, 'Kozarci PVC Laško 0,5L (50/1)', 'pak', 8),
(88208, 'Tequila Sierra Tropical Chilli liker 0,7L', '0,7L', 1);


SET IDENTITY_INSERT DobavniArtikel OFF;

INSERT INTO Skladisce (Naziv)
VALUES ('Mladinski klub Kanal');

INSERT INTO Dobava (Datum, Prevzel_uporabnik_id)
VALUES ('2025-09-10', '5484f146-1f7a-401b-bf90-e8f706015db6');

INSERT INTO DobavaVSkladisce (Kolicina, Skladisce_id, Dobava_id, DobavniArtikel_Id)
VALUES
(6,   1, 1, 32961),   -- Whisky Tullamore Dew (40%) 1L
(12,  1, 1, 39450),   -- Vodka Gorbatschov (37,5%) 1L
(6,   1, 1, 90710),   -- Jagermeister pomaranča (33%) 0,7L
(6,   1, 1, 48505),   -- Gin Finsbury (37,5%) 1L
(120, 1, 1, 38078),   -- Pivo Union Radler isotonic 0,0% ploč 0,5L
(3,   1, 1, 00259),   -- Pivo Laško točeno Zlatorog 30L sod
(144, 1, 1, 00202),   -- Coca Cola ploč 0,33L
(120, 1, 1, 00178),   -- Ledeni čaj breskev Sola Union ploč 0,33L
(72,  1, 1, 00185),   -- Fanta orange ploč 0,33L
(144, 1, 1, 00145),   -- Red Bull ploč 0,25L
(48,  1, 1, 09936),   -- Voda Dana negazirana 0,5L
(12,  1, 1, 01967),   -- Jagermeister (35%) 1L
(8,   1, 1, 00692),   -- Whisky Jack Daniel's Old No.7 (40%) 1L
(7,   1, 1, 08294),   -- Tequila Sierra Gold Reposado (38%) 1L
(8,   1, 1, 00618),   -- Pelinkovec grenki Fructal (28%) 1L
(8,   1, 1, 08543),   -- Viljamovka Roner Williams (40%) 0,7L
(8,   1, 1, 28767),   -- Borovničevec Fructal (20%) 1L
(8,   1, 1, 89371),   -- Liker limonce Stock (25%) 1L
(24,  1, 1, 47111),   -- Nektar pomaranča Fructal gemina 1L
(12,  1, 1, 47112),   -- Nektar jabolko Fructal gemina 1L
(96,  1, 1, 51534),    -- Pivo Union Radler z grenivko (2%) ploč 0,5L
(3,   1, 1, 62858),   -- Brisače papirnate 2-slojne 215m
(36,  1, 1, 00008),   -- Voda Radenska gazirana classic 1,5L
(5,   1, 1, 56831),   -- Vrečke za smeti 150L
(48,  1, 1, 00361),   -- Pivo Union 6x ploč 0,5L zavit
(100, 1, 1, 54733),   -- Kozarci PVC Laško 0,3L
(100, 1, 1, 54734),   -- Kozarci PVC Laško 0,5L
(6,   1, 1, 88208);   -- Tequila Sierra Tropical Chilli liker (18%) 0,7L
