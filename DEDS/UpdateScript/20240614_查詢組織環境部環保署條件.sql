--1.�s�W���(ConUnitCode)�@�@�Ȼs�Ʋ�´1�@
alter TABLE ConUnitCode Add CusOrg1 int Null
Go

--2.�ɤW���(ConUnitCode)
Update ConUnitCode Set CusOrg1 = 1 Where Code Like 'a%'
Update ConUnitCode Set CusOrg1 = 2 Where Code Like 'b%'
Go

