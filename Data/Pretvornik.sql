-- Najprej dodaj Bar skladišče, če ga še nimaš
IF NOT EXISTS (SELECT * FROM Skladisce WHERE Naziv = 'Bar')
BEGIN
    INSERT INTO Skladisce (Naziv) VALUES ('Bar');
END
GO

-- TOČENO PIVO iz sodov
-- Laško točeno 0,5L iz soda 30L (ID soda = 259, 30L = 60 x 0.5L)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT ID, 259, 60 
FROM Artikel 
WHERE Naziv = 'Laško točeno 0,5 L';

-- Laško točeno 0,3L iz soda 30L (30L = 100 x 0.3L)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT ID, 259, 100 
FROM Artikel 
WHERE Naziv = 'Laško točen 0,3 L';

-- ŽGANJE 0.03L iz steklenic 1L
-- Tequila Sierra Silver (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 8294, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Tequila Sierra Silver 0,03';

-- Tequila Sierra Tropical (steklenica 0.7L = 23.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 88208, 23.33 
FROM Artikel a
WHERE a.Naziv = 'Tequila Sierra Tropical 0,03';

-- Jägermeister (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 1967, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Jägermeister 0,03';

-- Jack Daniel's (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 692, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Jack Daniel''s 0,03';

-- Vodka (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 39450, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Vodka 0,03';

-- Havana rum (steklenica 1L = 33.33 shotov) - ni v DobavniArtikel, treba bi bilo dodati
-- INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
-- SELECT a.ID, ???, 33.33 
-- FROM Artikel a
-- WHERE a.Naziv = 'Havana rum 0,03';

-- Gin (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 48505, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Gin 0,03';

-- Pelinkovec (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 618, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Pelinkovec 0,03';

-- Viljamovka (steklenica 0.7L = 23.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 8543, 23.33 
FROM Artikel a
WHERE a.Naziv = 'Viljamovka 0,03';

-- Borovničke (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 28767, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Borovničke 0,03';

-- Limonce (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 89371, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Limonce 0,03';

-- Tullamore Dew (steklenica 1L = 33.33 shotov)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 32961, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Tullamore Dew 0,03';

-- MIKSANI NAPITKI (potrebujejo več sestavin, težje je avtomatizirati)
-- Za Jäger Cola, Jack Cola, itd. bi moral imeti ločene povezave za vsako sestavino
-- Primer: Jäger Cola = 0.03L Jägermeister + 0.2L Cole

-- Jäger Cola (uporablja Jägermeister)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 1967, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Jäger Cola';

-- Za Colo (ločena)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 202, 1.65  -- 0.33L pločevinka = cca 1.65 x 0.2L porcij
FROM Artikel a
WHERE a.Naziv = 'Jäger Cola';

-- Jack Cola
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 692, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Jack Cola';

INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 202, 1.65 
FROM Artikel a
WHERE a.Naziv = 'Jack Cola';

-- Tully Cola
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 32961, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Tully Cola';

INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 202, 1.65 
FROM Artikel a
WHERE a.Naziv = 'Tully Cola';

-- Rum Cola
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 202, 1.65 
FROM Artikel a
WHERE a.Naziv = 'Rum Cola';

-- Juice vodka (Vodka + sok)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 39450, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Juice vodka';

-- Za sok
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 47111, 5  -- 1L nektarja = 5 x 0.2L porcij
FROM Artikel a
WHERE a.Naziv = 'Juice vodka';

-- Gin tonic
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 48505, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Gin tonic';

-- Red bull vodka
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 39450, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Red bull vodka';

INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 145, 1  -- 1 Red Bull = 1 miks
FROM Artikel a
WHERE a.Naziv = 'Red bull vodka';

-- BREZALKOHOLNO PIVO iz pločevink
-- Union Radler isotonic (direkt pločevinka)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 38078, 1 
FROM Artikel a
WHERE a.Naziv = 'Radler Isotonic';

-- Radler 0,5L iz pločevinke
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 51534, 1 
FROM Artikel a
WHERE a.Naziv = 'Radler 0,5 L';

-- Union pločevinka
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 361, 6  -- 6-pack = 6 pločevink
FROM Artikel a
WHERE a.Naziv = 'Union pločevinka 0,5 L';

-- BREZALKOHOLNO
-- Coca Cola (iz pločevink)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 202, 1 
FROM Artikel a
WHERE a.Naziv = 'Coca Cola 0,33';

-- Fanta
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 185, 1 
FROM Artikel a
WHERE a.Naziv = 'Fanta 0,33';

-- Ledeni čaj
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 178, 1 
FROM Artikel a
WHERE a.Naziv = 'Ledeni čaj 0,33';

-- Red Bull
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 145, 1 
FROM Artikel a
WHERE a.Naziv = 'Red Bull 0,25';

-- Voda 0,5L
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 9936, 1 
FROM Artikel a
WHERE a.Naziv = 'Voda 0,5';

-- Radenska 2dl iz večjih steklenic
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 8, 7.5  -- 1.5L = 7.5 x 0.2L porcij
FROM Artikel a
WHERE a.Naziv = 'Radenska 2dl';

-- Sokovi iz 1L plastenk
-- Juice 0,2L (pomaranča)
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 47111, 5  -- 1L = 5 x 0.2L
FROM Artikel a
WHERE a.Naziv = 'Juice 0,2';

-- Jabolčnik 0,2L
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 47112, 5 
FROM Artikel a
WHERE a.Naziv = 'Jabolčnik 0,2';

-- OPOMBA: Cocktails (Sex on the beach, Mojito, Cuba Libre, Long Island, Fifi)
-- uporabljajo več sestavin in bi potrebovali bolj kompleksen sistem
-- Lahko jih dodaš ročno ali pa ustvariš ločeno tabelo za recepte

GO
-- Dodaj manjkajoče dobavne artikle, če jih nimaš
INSERT INTO DobavniArtikel (ID, Naziv, Enota, VrstaID) VALUES
(99901, 'Rum Havana 1L', '1L', 1),  -- Za Havana rum shots
(99902, 'Sprite pločevinka 0,33L', '0,33L', 5);  -- Za Sprite

-- Potem dodaj povezave
INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 99901, 33.33 
FROM Artikel a
WHERE a.Naziv = 'Havana rum 0,03';

INSERT INTO ArtikelDobavniArtikelPovezava (Artikel_id, DobavniArtikel_Id, FaktorPretvorbe)
SELECT a.ID, 99902, 1 
FROM Artikel a
WHERE a.Naziv = 'Sprite 0,33';