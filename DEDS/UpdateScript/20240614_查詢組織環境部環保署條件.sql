--1.�s�W���(ConUnitCode)�@�@�j����´1�@
alter TABLE ConUnitCode Add BigOrg1 int Null
Go

--2.�ɤW���(ConUnitCode)
Update ConUnitCode Set BigOrg1 = 1 Where Code Like 'a%'
Update ConUnitCode Set BigOrg1 = 2 Where Code Like 'b%'
Go

