--1.�s�W���(ConUnitCode)
alter TABLE ConUnitCode Add UserOrg1 int Null
Go

--2.�ɤW���(ConUnitCode)
Update ConUnitCode Set UserOrg1 = 1 Where Code Like 'a%'
Update ConUnitCode Set UserOrg1 = 2 Where Code Like 'b%'
Go

