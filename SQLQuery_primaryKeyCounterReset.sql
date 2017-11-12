-- Reset the primary key counter
DBCC CHECKIDENT ('Categories', RESEED, 8)