--1.新增欄位(ConUnitCode)　　大單位組織1　
alter TABLE ConUnitCode Add BigOrg1 int Null
Go

--2.補上資料(ConUnitCode)
Update ConUnitCode Set BigOrg1 = 1 Where Code Like 'a%'
Update ConUnitCode Set BigOrg1 = 2 Where Code Like 'b%'
Go

