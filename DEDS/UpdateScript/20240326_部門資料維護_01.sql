--部門資料維護
--1.新增資料表(ConUnitCode)

--1.2 匯入資料(ConUnitCode)

--2.新增資料表(ConUnitPerson)

--2.2 匯入資料(ConUnitPerson)

--3.User 新增欄位([ConUnit] [nvarchar](10) NULL,
ALTER TABLE [User] Add [ConUnit] [nvarchar](10) NULL
Go

Update [User] Set ConUnit = 'b4' Where Id = 'A110235'  
Update [User] Set ConUnit = 'b3' Where Id = 'AO7384'  
Update [User] Set ConUnit = 'b13' Where Id = 'ase3479306'  
Update [User] Set ConUnit = 'a2' Where Id = 'CHEN,YAN-NAN'  
Update [User] Set ConUnit = 'b11' Where Id = 'chihya'  
Update [User] Set ConUnit = 'a6' Where Id = 'chotseng'  
Update [User] Set ConUnit = 'b15' Where Id = 'engallenchen8'  
Update [User] Set ConUnit = 'b8' Where Id = 'epa0808'  
Update [User] Set ConUnit = 'b7' Where Id = 'EPB10005'  
Update [User] Set ConUnit = 'b14' Where Id = 'EPB5566'  
Update [User] Set ConUnit = 'b20' Where Id = 'fr58310'  
Update [User] Set ConUnit = 'b20' Where Id = 'g2010'  
Update [User] Set ConUnit = 'b12' Where Id = 'H10126'  
Update [User] Set ConUnit = 'b6' Where Id = 'HE,LI-XIN'  
Update [User] Set ConUnit = 'a8' Where Id = 'hncheng'  
Update [User] Set ConUnit = 'b1' Where Id = 'K072012'  
Update [User] Set ConUnit = 'b2' Where Id = 'kn5070'  
Update [User] Set ConUnit = 'a4' Where Id = 'kting7'  
Update [User] Set ConUnit = 'a9' Where Id = 'kuochiang.lin'  
Update [User] Set ConUnit = 'b19' Where Id = 'lin'  
Update [User] Set ConUnit = 'b22' Where Id = 'love13530'  
Update [User] Set ConUnit = 'a10' Where Id = 'NERA999'  
Update [User] Set ConUnit = 'a12' Where Id = 'ningjen.liao'  
Update [User] Set ConUnit = 'b16' Where Id = 'ptepb0208'  
Update [User] Set ConUnit = 'b16' Where Id = 'ptepb760'  
Update [User] Set ConUnit = 'a11' Where Id = 'pychuang'  
Update [User] Set ConUnit = 'b9' Where Id = 'q10031003'  
Update [User] Set ConUnit = 'b21' Where Id = 'sutun'  
Update [User] Set ConUnit = 'b17' Where Id = 't8411'  
Update [User] Set ConUnit = 'a2' Where Id = 'TsangshuoChang'  
Update [User] Set ConUnit = 'a5' Where Id = 'w2832'  
Update [User] Set ConUnit = 'a3' Where Id = 'yenchun.liu'  
Update [User] Set ConUnit = 'a7' Where Id = 'yuju.li'  
Update [User] Set ConUnit = 'b10' Where Id = 'ZHANG,XUAN-WEI'  
Update [User] Set ConUnit = 'a1' Where Id = '劉哲仲'  

----Select Id, ConUnit,
----       'Update [User] Set ConUnit = ''' + ConUnit + ''' Where Id = ''' + Id + '''  '
----From [User] 
----Where ConUnit Is Not Null

------通聯手冊Email轉入到應變人員清冊
----Update ConUnitPerson
----Set Email = b.Email
----From ConUnitPerson　a
----Left Join UserBasic b On a.Name = b.Name
----Where a.Email Is Null And b.Name Is Not Null
