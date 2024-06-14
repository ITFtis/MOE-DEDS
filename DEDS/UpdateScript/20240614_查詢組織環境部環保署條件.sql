--1.新增欄位(ConUnitCode)　　客製化組織1　
alter TABLE ConUnitCode Add CusOrg1 int Null
Go

--2.補上資料(ConUnitCode)
Update ConUnitCode Set CusOrg1 = 1 Where Code Like 'a%'
Update ConUnitCode Set CusOrg1 = 2 Where Code Like 'b%'
Go

