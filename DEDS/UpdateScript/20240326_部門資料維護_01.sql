--������ƺ��@
--1.�s�W��ƪ�(ConUnitCode)

--1.2 �פJ���(ConUnitCode)

--2.�s�W��ƪ�(ConUnitPerson)

--2.2 �פJ���(ConUnitPerson)

/****** Object:  Table [dbo].[ConUnitCode]    Script Date: 2024/3/28 �U�� 01:25:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConUnitCode](
	[Code] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_UnitCode] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConUnitPerson]    Script Date: 2024/3/28 �U�� 01:25:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConUnitPerson](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConType] [int] NOT NULL,
	[ConUnit] [nvarchar](10) NULL,
	[Name] [nvarchar](50) NULL,
	[Position] [nvarchar](50) NULL,
	[Tel] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[HTel] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Remark] [nvarchar](200) NULL,
	[BDate] [datetime] NULL,
	[BId] [nvarchar](24) NULL,
	[BName] [nvarchar](50) NULL,
	[UDate] [datetime] NULL,
	[UId] [nvarchar](24) NULL,
	[UName] [nvarchar](50) NULL,
	[ConfirmDate] [datetime] NULL,
	[PSort] [int] NULL,
 CONSTRAINT [PK_UnitPerson] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a1', N'�s�D������', 1)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a10', N'��a���Ҭ�s�|', 10)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a11', N'���Һ޲z�p(��X�W����)', 11)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a12', N'���Һ޲z�p(���ҽåͲ�)', 12)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a2', N'��X�W���q', 2)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a3', N'���ҫO�@�q', 3)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a4', N'�j�����ҥq', 4)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a5', N'����O�@�q', 5)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a6', N'�ʴ���T�q', 6)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a7', N'����ܾE�p', 7)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a8', N'�귽�`���p', 8)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'a9', N'�ƾǪ���޲z�p', 9)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b1', N'�򶩥�', 31)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b10', N'�n�뿤', 40)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b11', N'���L��', 41)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b12', N'�Ÿq��', 42)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b13', N'�Ÿq��', 43)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b14', N'�O�n��', 44)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b15', N'������', 45)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b16', N'�̪F��', 46)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b17', N'�O�F��', 47)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b18', N'�Ὤ��', 48)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b19', N'�y����', 49)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b2', N'�O�_��', 32)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b20', N'���', 50)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b21', N'������', 51)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b22', N'�s����', 52)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b3', N'�s�_��', 33)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b4', N'��饫', 34)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b5', N'�s�˿�', 35)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b6', N'�s�˥�', 36)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b7', N'�]�߿�', 37)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b8', N'�O����', 38)
GO
INSERT [dbo].[ConUnitCode] ([Code], [Name], [Sort]) VALUES (N'b9', N'���ƿ�', 39)
GO
SET IDENTITY_INSERT [dbo].[ConUnitPerson] ON 
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (1, 1, N'a1', N'�B����', N'�u�����ҧ޳N�v', N'(02)2311-7722 #2362', N'0928-600-010', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-03-27T11:23:37.997' AS DateTime), 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (2, 1, N'a1', N'�}����', N'�޺ʭݷs�D�����ղժ�', N'(02)2311-7722 #2042', N'0928-216-741', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-03-26T16:31:57.783' AS DateTime), N'�B����', N'�B����', CAST(N'2024-03-27T11:23:37.997' AS DateTime), 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (3, 1, N'a2', N'���ۨk', N'²���ޥ�', N'(02)2311-7722 #2905', N'0912-126-130', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (5, 1, N'a2', N'�L�z��', N'���', N'(02)2311-7722 #2930', N'0905-116-357', NULL, NULL, NULL, CAST(N'2024-03-26T15:59:17.557' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (6, 1, N'a2', N'�i�ܺ�', N'�˥��ޤh', N'(02)2311-7722 #2943', N'0933-528-226', NULL, NULL, N'��113�~�a�`����²�T�q���H�ӭ����D', CAST(N'2024-03-26T15:59:57.453' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (7, 1, N'a3', N'�f����', N'²���ޥ�', N'(02)2311-7722 #2703', N'0921-993-376', N'02-2651-5832', NULL, NULL, CAST(N'2024-03-26T16:01:15.990' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (8, 1, N'a3', N'������', N'���', N'(02)2311-7722 #2730', N'0928-403-230', N'02-2609-9089', NULL, NULL, CAST(N'2024-03-26T16:02:35.313' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (9, 1, N'a3', N'�B�ۧ�', N'���', N'(02)2311-7722 #2740', N'0911-121-957', NULL, NULL, NULL, CAST(N'2024-03-26T16:03:13.337' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (10, 1, N'a4', N'�]����', N'²���ޥ�', N'(02)2311-7722 #6006', N'0975-113-127', NULL, NULL, NULL, CAST(N'2024-03-26T16:04:56.650' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (11, 1, N'a4', N'��ڢ��', N'���', N'(02)2311-7722 #6800', N'0920-755-570', NULL, NULL, NULL, CAST(N'2024-03-26T16:05:32.087' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (12, 1, N'a4', N'���ʹ@', N'�U�z���ҧ޳N�v', N'(02)2311-7722 #6805', N'0988-500-566', NULL, NULL, NULL, CAST(N'2024-03-26T16:06:09.520' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (13, 1, N'a5', N'�\����', N'²���ޥ�', N'(02)2311-7722 #2803', N'0958-909-541', NULL, NULL, NULL, CAST(N'2024-03-26T16:06:55.597' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (14, 1, N'a5', N'�B����', N'���', N'(02)2311-7722 #2830', N'0933-015-742', N'02-2339-7503', NULL, NULL, CAST(N'2024-03-26T16:07:47.347' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (15, 1, N'a5', N'���تF', N'�ޥ�', N'(02)2311-7722 #2832', N'0933-158-122', N'02-2308-4177', NULL, NULL, CAST(N'2024-03-26T16:08:39.740' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (16, 1, N'a6', N'���H��', N'�M���e��', N'(02)2311-7722 #2302', N'0935-804-051', N'02-8285-5900', NULL, NULL, CAST(N'2024-03-26T16:09:57.873' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (17, 1, N'a6', N'�����D', N'���', N'(02)2311-7722 #2330', N'0908-733-200', NULL, NULL, NULL, CAST(N'2024-03-26T16:11:20.283' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (18, 1, N'a6', N'���`�`', N'���R�v', N'(02)2311-7722 #2343', N'0958-532-612', N'02-2532-6152', NULL, NULL, CAST(N'2024-03-26T16:12:33.650' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (19, 1, N'a7', N'���H�g', N'�Ʋժ�', N'(02)2322-2050 #66401', N'0933-713-105', N'02-8667-6667', NULL, NULL, CAST(N'2024-03-26T16:13:33.890' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (20, 1, N'a7', N'���W', N'���', N'(02)2322-2050 #66410', N'0988-920-317', NULL, NULL, NULL, CAST(N'2024-03-26T16:14:10.080' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (21, 1, N'a7', N'���|��', N'���ҧ޳N�v', N'(02)2322-2050 #66435', N'0912-596-500', NULL, NULL, NULL, CAST(N'2024-03-26T16:14:47.603' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (22, 1, N'a8', N'���T��', N'�M���e��', N'(02)2370-5888 #3008', N'0963-580-512', NULL, NULL, NULL, CAST(N'2024-03-26T16:15:49.697' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (23, 1, N'a8', N'�����', N'���', N'(02)2370-5888 #3201', N'0933-156-944', NULL, NULL, NULL, CAST(N'2024-03-26T16:16:29.523' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (24, 1, N'a8', N'�G�N�n', N'�U�z���ҧ޳N�v', N'(02)2370-5888 #3206', N'0933-909-171', NULL, NULL, NULL, CAST(N'2024-03-26T16:17:10.677' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (25, 1, N'a9', N'�c�a�f', N'�Ʋժ�', N'(02)2325-7399 #55401', N'0932-944-427', NULL, NULL, NULL, CAST(N'2024-03-26T16:18:16.237' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (26, 1, N'a9', N'�i�a��', N'���', N'(02)2325-7399 #55830', N'0933-060-487', NULL, NULL, NULL, CAST(N'2024-03-26T16:18:54.203' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-27T15:04:07.073' AS DateTime), N'scullyepa', N'���Q�W', NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (27, 1, N'a9', N'�L��j', N'�ޤh', N'(02)2325-7399 #55232', N'0928-106-830', NULL, NULL, NULL, CAST(N'2024-03-26T16:19:52.050' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (28, 1, N'a10', N'���ߨk', N'�D��', N'(03)491-5818 #4400', N'0928-918-966', NULL, NULL, NULL, CAST(N'2024-03-26T16:20:40.890' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (29, 1, N'a10', N'���E�y', N'���', N'(03)491-5818 #2510', N'0910-288-161', NULL, NULL, NULL, CAST(N'2024-03-26T16:21:13.223' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (30, 1, N'a10', N'���ӽ�', N'�M��', N'(03)491-5818 #2516', N'0983-410-837', NULL, NULL, NULL, CAST(N'2024-03-26T16:21:58.983' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (31, 1, N'a11', N'�i��ұ', N'�ժ�', N'(02)2383-2389 #59005', N'0916-307-024', NULL, NULL, NULL, CAST(N'2024-03-26T16:23:00.117' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (32, 1, N'a11', N'�B�s��', N'���', N'(02)2383-2389 #59901', N'0922-762-981', NULL, NULL, NULL, CAST(N'2024-03-26T16:23:44.737' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (33, 1, N'a11', N'�����q', N'���ҧ޳N�v', N'(02)2383-2389 #59903', N'0966-621-905', NULL, NULL, NULL, CAST(N'2024-03-26T16:24:17.500' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (34, 1, N'a12', N'�Q��y', N'�ժ�', N'(04)2252-1718 #53800', N'0937-408-181', NULL, NULL, NULL, CAST(N'2024-03-26T16:24:51.963' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (35, 1, N'a12', N'������', N'���', N'(04)2252-1718 #53901', N'0978-726-976', NULL, NULL, NULL, CAST(N'2024-03-26T16:25:31.787' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (36, 1, N'a12', N'���ʤ�', N'�ޤh', N'(04)2252-1718 #53902', N'0983-297-181', NULL, NULL, NULL, CAST(N'2024-03-26T16:26:10.380' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (37, 2, N'a2', N'�c��y', N'�M��', N'(02)2311-7722 #2912', N'0939-903-725', NULL, NULL, NULL, CAST(N'2024-03-26T16:43:59.743' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (38, 2, N'a2', N'���[��', N'�ޥ�', N'(02)2311-7722 #2985', N'�L����A�q�����f�i�ܺ�', N'02-2249-5980', NULL, NULL, CAST(N'2024-03-26T16:44:36.123' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (39, 2, N'a3', N'�Ӻ��x', N'����', N'(02)2311-7722 #2744', N'0932-232-158', NULL, NULL, NULL, CAST(N'2024-03-26T16:45:14.683' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (40, 2, N'a3', N'���{��', N'����', N'(02)2311-7722 #2736', N'0928-994-485', NULL, NULL, NULL, CAST(N'2024-03-26T16:45:52.397' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (41, 2, N'a4', N'�P�o��', N'�ޥ�', N'(02)2311-7722 #6508', N'0963-252-869', NULL, NULL, NULL, CAST(N'2024-03-26T16:46:28.840' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (42, 2, N'a4', N'�B�W�g', N'�ޥ�', N'(02)2311-7722 #6323', N'0937-205-019', NULL, NULL, NULL, CAST(N'2024-03-26T16:47:09.380' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (43, 2, N'a5', N'������', N'�M��', N'(02)2311-7722 #2881', N'0956-124-167', NULL, NULL, NULL, CAST(N'2024-03-26T16:47:55.227' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (44, 2, N'a5', N'���تF', N'�ޥ�', N'(02)2311-7722 #2832', N'0933-158-122', N'02-2308-4177', NULL, NULL, CAST(N'2024-03-26T16:48:27.460' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (45, 2, N'a6', N'��β�', N'�ޥ�', N'(02)2311-7722 #2366', N'0989-757-525', NULL, NULL, NULL, CAST(N'2024-03-26T16:49:07.883' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (46, 2, N'a6', N'���h�a', N'���R�v', N'(02)2311-7722 #2334', N'0963-293-519', NULL, NULL, NULL, CAST(N'2024-03-26T16:49:44.703' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (47, 2, N'a7', N'���F�w', N'�ޥ�', N'(02)2322-2050 #66113', N'0928-276-982', NULL, NULL, NULL, CAST(N'2024-03-26T16:50:33.983' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (48, 2, N'a7', N'�\�m��', N'�ޥ�', N'(02)2322-2050 #66317', N'0933-956-317', NULL, NULL, NULL, CAST(N'2024-03-26T16:51:14.490' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (49, 2, N'a8', N'�\�ʵ�', N'�ޥ�', N'(02)2370-5888 #3723', N'0958-638-987', NULL, NULL, NULL, CAST(N'2024-03-26T17:24:25.357' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (50, 2, N'a8', N'�����a', N'�ޥ�', N'(02)2370-5888 #3712', N'0953-881-980', NULL, NULL, NULL, CAST(N'2024-03-26T17:25:08.357' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (51, 2, N'a9', N'�x�R�y', N'�ޥ�', N'(02)2325-7399 #55210', N'0910-980-605', NULL, NULL, NULL, CAST(N'2024-03-26T17:45:20.910' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 4)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (52, 2, N'a9', N'�B�ب}', N'�ޥ�', N'(02)2325-7399 #55427', N'0953-294-101', NULL, NULL, NULL, CAST(N'2024-03-26T17:45:55.257' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 5)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (53, 1, N'b1', N'�L�ۦw', NULL, N'02-24651115 #259', N'0911951993', NULL, N'ten7495@mail.klcg.gov.tw', NULL, CAST(N'2024-03-26T17:58:06.557' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-27T11:50:38.343' AS DateTime), N'K072012', N'�L�ۦw', CAST(N'2024-03-27T11:50:43.750' AS DateTime), 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (54, 1, N'b2', N'�\����', NULL, N'02-27208889 #7273', N'0919982470', NULL, N'kn5070@gov.taipei', NULL, CAST(N'2024-03-26T17:58:46.447' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (55, 1, N'b3', N'�d�F��', NULL, N'02-29532111#4044', N'0966113089', NULL, N'AO7384@ntpc.gov.tw', NULL, CAST(N'2024-03-26T17:59:26.793' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (56, 1, N'b4', N'���ӭ�', NULL, N'03-3381900#204', N'0953760943', NULL, N'110235@tyemid.gov.tw', NULL, CAST(N'2024-03-26T18:00:00.370' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (57, 1, N'b5', N'ù�E�s', NULL, N'03-5519345 #5507', N'0988511739', NULL, N'10014912@hchg.gov.tw', NULL, CAST(N'2024-03-26T18:00:46.107' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (58, 1, N'b6', N'�P�ѷ�', NULL, N'03-5368920#1012', N'0937990615', NULL, N'63055@ems.hccepb.gov.tw', NULL, CAST(N'2024-03-26T18:01:26.403' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-27T15:13:14.363' AS DateTime), N'scullyepa', N'���Q�W', NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (59, 1, N'b7', N'���v��', NULL, N'037-558558 #511', N'0911600833', NULL, N'cwhuang@mail.mlepb.gov.tw', NULL, CAST(N'2024-03-26T18:02:02.620' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (60, 1, N'b8', N'�P����', NULL, N'04-22289111 #66518', N'0988562272', NULL, N'cjchou@taichung.gov.tw', NULL, CAST(N'2024-03-26T18:02:38.647' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (61, 1, N'b9', N'ù�ߴP', NULL, N'04-7115655 #411', N'0905027312', NULL, N'q10031003@chepb.gov.tw', NULL, CAST(N'2024-03-26T18:03:22.450' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (62, 1, N'b10', N'�i�a޳', NULL, N'049-2237530 #1302', N'0917819988', NULL, N'jk85102@mail.ntepb.gov.tw', NULL, CAST(N'2024-03-26T18:04:01.317' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (63, 1, N'b11', N'�H�οP', NULL, N'05-5526302', N'0937782020', NULL, N'ylepb1257@ylepb.gov.tw', NULL, CAST(N'2024-03-26T18:04:44.940' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-27T15:29:46.717' AS DateTime), N'scullyepa', N'���Q�W', NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (64, 1, N'b12', N'�B�¦p', NULL, N'05-2251775 #507', N'0919295978', NULL, N'cycepb10126@ems.chiayi.gov.tw', NULL, CAST(N'2024-03-26T18:05:25.987' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (65, 1, N'b13', N'�i�a��', NULL, N'05-3620800 #317', N'0987686030', NULL, N'ase3479306@cyepb.gov.tw', NULL, CAST(N'2024-03-26T18:06:02.567' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (66, 1, N'b14', N'�����', NULL, N'06-2686660 #3307', N'0928573612', NULL, N'srk1130@mail.tnepb.gov.tw', NULL, CAST(N'2024-03-26T18:06:52.917' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (67, 1, N'b15', N'������', NULL, N'07-7351500 #2337', N'0972591504', NULL, N'engallenchen8@kcg.gov.tw', NULL, CAST(N'2024-03-26T18:07:39.093' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (68, 1, N'b16', N'�U���y', NULL, N'08-7351911 #701', N'0912888365', NULL, N'dengue@mail.ptepb.gov.tw', NULL, CAST(N'2024-03-26T18:08:29.583' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-26T18:09:27.393' AS DateTime), N'admin', N'�t�κ޲z��', NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (69, 1, N'b16', N'���ɦ�', NULL, N'08-7351911 #704', N'0920487360', NULL, N'karaoke.ma@gmail.com', NULL, CAST(N'2024-03-26T18:09:13.237' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (70, 1, N'b17', N'���_��', NULL, N'089-221999#305', N'0956160777', NULL, N't415@epb.taitung.gov.tw', NULL, CAST(N'2024-03-26T18:16:53.103' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-27T15:30:51.087' AS DateTime), N'scullyepa', N'���Q�W', NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (71, 1, N'b18', N'²�έ�', NULL, N'03-8237575 #2620', N'0988489838', NULL, N'hlepb2577@gmail.com', NULL, CAST(N'2024-03-26T18:17:27.757' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (72, 1, N'b19', N'�L���', NULL, N'03-9907755 #312', N'0933986963', NULL, N'log@ilepb.gov.tw', NULL, CAST(N'2024-03-26T18:17:56.833' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (73, 1, N'b20', N'�\�R��', NULL, N'06-9221778 #212', N'0972131018', NULL, N'fr84720@phepb.penghu.gov.tw', NULL, CAST(N'2024-03-26T18:18:37.543' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (74, 1, N'b20', N'�ڪF�@', NULL, N'06-9221778 #205', N'0953296636', NULL, N'fr58310@phepb.penghu.gov.tw', NULL, CAST(N'2024-03-26T18:19:06.427' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (75, 1, N'b21', N'���Ӳ�', NULL, N'082-336823 #811', N'0913960656', NULL, N'sutun@mail.kinmen.gov.tw', NULL, CAST(N'2024-03-26T18:19:44.920' AS DateTime), N'admin', N'�t�κ޲z��', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[ConUnitPerson] ([Id], [ConType], [ConUnit], [Name], [Position], [Tel], [Mobile], [HTel], [Email], [Remark], [BDate], [BId], [BName], [UDate], [UId], [UName], [ConfirmDate], [PSort]) VALUES (76, 1, N'b22', N'���¤�', NULL, N'0836-26520 #266', N'0911078531', NULL, N'love13530@yahoo.com.tw', NULL, CAST(N'2024-03-26T18:20:14.333' AS DateTime), N'admin', N'�t�κ޲z��', CAST(N'2024-03-28T09:51:46.360' AS DateTime), N'admin', N'�t�κ޲z��', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[ConUnitPerson] OFF
GO


--3.User �s�W���([ConUnit] [nvarchar](10) NULL,
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
Update [User] Set ConUnit = 'a1' Where Id = '�B����'  

----Select Id, ConUnit,
----       'Update [User] Set ConUnit = ''' + ConUnit + ''' Where Id = ''' + Id + '''  '
----From [User] 
----Where ConUnit Is Not Null