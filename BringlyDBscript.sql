USE [master]
GO
/****** Object:  Database [DEV_Bringly]    Script Date: 3/12/2018 5:24:39 PM ******/
CREATE DATABASE [DEV_Bringly]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DEV_Bringly', FILENAME = N'E:\SQL-2014\DataBase\DEV_Bringly.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DEV_Bringly_log', FILENAME = N'E:\SQL-2014\DataBaseLog\DEV_Bringly_log.ldf' , SIZE = 22144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DEV_Bringly] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DEV_Bringly].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DEV_Bringly] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DEV_Bringly] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DEV_Bringly] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DEV_Bringly] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DEV_Bringly] SET ARITHABORT OFF 
GO
ALTER DATABASE [DEV_Bringly] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DEV_Bringly] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DEV_Bringly] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DEV_Bringly] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DEV_Bringly] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DEV_Bringly] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DEV_Bringly] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DEV_Bringly] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DEV_Bringly] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DEV_Bringly] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DEV_Bringly] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DEV_Bringly] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DEV_Bringly] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DEV_Bringly] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DEV_Bringly] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DEV_Bringly] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DEV_Bringly] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DEV_Bringly] SET RECOVERY FULL 
GO
ALTER DATABASE [DEV_Bringly] SET  MULTI_USER 
GO
ALTER DATABASE [DEV_Bringly] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DEV_Bringly] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DEV_Bringly] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DEV_Bringly] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DEV_Bringly] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DEV_Bringly', N'ON'
GO
USE [DEV_Bringly]
GO
/****** Object:  User [dev_Bringly]    Script Date: 3/12/2018 5:24:39 PM ******/
CREATE USER [dev_Bringly] FOR LOGIN [dev_Bringly] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [dev_Bringly]
GO
/****** Object:  Table [dbo].[tblCity]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCity](
	[CityGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblCity_CityGuid]  DEFAULT (newid()),
	[CityName] [nvarchar](50) NOT NULL,
	[CityUrlName] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblCity_IsDeleted]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblCity] PRIMARY KEY CLUSTERED 
(
	[CityGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCountry]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCountry](
	[CountryDisplayName] [nvarchar](200) NOT NULL CONSTRAINT [DEFAULT_tblCountry_CountryDisplayName]  DEFAULT (''),
	[CountryName] [nvarchar](200) NOT NULL CONSTRAINT [DEFAULT_tblCountry_CountryName]  DEFAULT (''),
	[CountryGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DEFAULT_tblCountry_CountryGUID]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[DateModified] [datetime2](7) NOT NULL CONSTRAINT [DEFAULT_tblCountry_CountryLastModified]  DEFAULT ('11/14/2013 1:43:04 PM'),
	[PhoneCode] [varchar](10) NULL,
	[ISOCountryCode] [varchar](10) NULL,
	[Valid] [bit] NULL CONSTRAINT [DF_tblCountry_Valid]  DEFAULT ((1)),
	[CountryTwoLetterCode] [nvarchar](2) NULL,
	[CountryThreeLetterCode] [nvarchar](3) NULL,
 CONSTRAINT [PK_tblCountry_1] PRIMARY KEY CLUSTERED 
(
	[CountryGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblEmail]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmail](
	[EmailGuid] [uniqueidentifier] NOT NULL,
	[TemplateGuid] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](200) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[EmailFrom] [nvarchar](100) NOT NULL,
	[Sent] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblEmail_IsDeleted]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblEmail] PRIMARY KEY CLUSTERED 
(
	[EmailGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEmailTo]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmailTo](
	[EmailToGuid] [uniqueidentifier] NOT NULL,
	[EmailTo] [nvarchar](100) NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[EmailGuid] [uniqueidentifier] NOT NULL,
	[Read] [bit] NOT NULL,
 CONSTRAINT [PK_tblEmailTo] PRIMARY KEY CLUSTERED 
(
	[EmailToGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblFavourite]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFavourite](
	[FavouriteGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblFavourite_FavouriteGuid]  DEFAULT (newid()),
	[RestaurantGuid] [uniqueidentifier] NOT NULL,
	[CreatedByGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblFavourite_CreatedByUserGuid]  DEFAULT (newid()),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tblFavourite_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_tblFavourite] PRIMARY KEY CLUSTERED 
(
	[FavouriteGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblItem]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblItem](
	[ItemGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblItem_ItemGuid]  DEFAULT (newid()),
	[ItemName] [nvarchar](200) NOT NULL,
	[RestaurantGuid] [uniqueidentifier] NOT NULL,
	[CategoryGuid] [uniqueidentifier] NULL,
	[DeliveryCharge] [decimal](18, 2) NULL,
	[ItemImage] [nvarchar](100) NOT NULL,
	[ItemWeight] [nvarchar](50) NULL,
	[ItemSize] [nvarchar](50) NOT NULL,
	[ItemPrice] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tblItem_DateCreated]  DEFAULT (getdate()),
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_tblItem_IsActive]  DEFAULT ((1)),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblItem_IsDeleted]  DEFAULT ((0)),
	[DeletedByGuid] [uniqueidentifier] NULL,
	[DeletedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Modifiedby] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblItem] PRIMARY KEY CLUSTERED 
(
	[ItemGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblLookUpDomain]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLookUpDomain](
	[LookDomainGuid] [uniqueidentifier] NOT NULL,
	[LookUpDomainCode] [nvarchar](200) NOT NULL,
	[LookUpDomainDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblLookUpDomain] PRIMARY KEY CLUSTERED 
(
	[LookDomainGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblLookUpDomainValue]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLookUpDomainValue](
	[LookUpDomainValueGuid] [uniqueidentifier] NOT NULL,
	[FK_LookDomainValueGuid] [uniqueidentifier] NOT NULL,
	[LookupDomainValue] [nvarchar](200) NOT NULL,
	[LookupDomainText] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblLookUpDomainValue] PRIMARY KEY CLUSTERED 
(
	[LookUpDomainValueGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOrder]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOrder](
	[OrderGuid] [uniqueidentifier] NOT NULL,
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
	[OrderDiscount] [decimal](18, 2) NOT NULL,
	[OrderSubTotal] [decimal](18, 2) NOT NULL,
	[OrderTotal] [decimal](18, 2) NOT NULL,
	[OrderStatus] [nvarchar](50) NOT NULL CONSTRAINT [DF_tblOrder_OrderStatus]  DEFAULT ((0)),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblOrder_IsDeleted]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblOrder] PRIMARY KEY CLUSTERED 
(
	[OrderGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOrderAddress]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOrderAddress](
	[AddressGuid] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[ZipCode] [nvarchar](20) NOT NULL,
	[Phone] [nvarchar](100) NULL,
	[AddressPersonalName] [nvarchar](200) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_tblOrderAddress] PRIMARY KEY CLUSTERED 
(
	[AddressGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOrderItem]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOrderItem](
	[OrderItemGuid] [uniqueidentifier] NOT NULL,
	[OrderGuid] [uniqueidentifier] NOT NULL,
	[ItemGuid] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblOrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOrderStatus]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOrderStatus](
	[StatusName] [nvarchar](200) NOT NULL,
	[StatusDisplayName] [nvarchar](200) NOT NULL,
	[StatusOrder] [int] NULL,
	[StatusEnabled] [bit] NOT NULL,
	[StatusColor] [nvarchar](7) NULL,
	[StatusGUID] [uniqueidentifier] NOT NULL,
	[DateModified] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRestaurant]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRestaurant](
	[RestaurantGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblRestaurant_RestaurantId]  DEFAULT (newid()),
	[RestaurantName] [nvarchar](200) NOT NULL,
	[RestaurantType] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[CityGuid] [uniqueidentifier] NOT NULL,
	[PinCode] [nvarchar](50) NOT NULL,
	[RestaurantImage] [nvarchar](50) NULL,
	[CountryGuid] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tblRestaurant_DateCreated]  DEFAULT (getdate()),
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedByGuid] [uniqueidentifier] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblRestaurant] PRIMARY KEY CLUSTERED 
(
	[RestaurantGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblReview]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReview](
	[ReviewGuid] [uniqueidentifier] NOT NULL,
	[RestaurantGuid] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[Review] [nvarchar](500) NOT NULL,
	[OrderGuid] [uniqueidentifier] NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[IsSkipped] [bit] NOT NULL,
	[IsApproved] [bit] NULL,
	[IsProcessed] [bit] NULL,
	[ApproveDate] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedByGuid] [uniqueidentifier] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedByGuid] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedByGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblReview] PRIMARY KEY CLUSTERED 
(
	[ReviewGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblState]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblState](
	[StateDisplayName] [nvarchar](200) NOT NULL CONSTRAINT [DEFAULT_tblState_StateDisplayName]  DEFAULT (''),
	[StateName] [nvarchar](200) NOT NULL CONSTRAINT [DEFAULT_tblState_StateName]  DEFAULT (''),
	[StateCode] [nvarchar](100) NULL,
	[CountryID] [int] NOT NULL,
	[StateGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DEFAULT_tblState_StateGUID]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[DateModified] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_tblState_1] PRIMARY KEY CLUSTERED 
(
	[StateGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblTemplate]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTemplate](
	[TemplateGuid] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[EmailFrom] [nvarchar](100) NOT NULL,
	[TemplateType] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Template_IsDeleted]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[TemplateGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblUsers_UserGuid]  DEFAULT (newid()),
	[EmailAddress] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[MobileNumber] [nvarchar](50) NULL,
	[SocialMediaUniqueId] [nvarchar](256) NULL,
	[UserSocialMediaData] [nvarchar](256) NULL,
	[UserRegistrationType] [int] NOT NULL,
	[ImageName] [nvarchar](100) NULL,
	[ImageExtension] [nvarchar](50) NULL,
	[ImagePath] [nvarchar](256) NULL,
	[PreferedCity] [uniqueidentifier] NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Modifiedby] [uniqueidentifier] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblUser_IsDeleted]  DEFAULT ((0)),
	[DeletedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblUsers] PRIMARY KEY CLUSTERED 
(
	[UserGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_tblUser] UNIQUE NONCLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUserAddress]    Script Date: 3/12/2018 5:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblUserAddress](
	[UserAddressGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tblUserAddress_UserAddressGuid]  DEFAULT (newid()),
	[UserGuid] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](256) NOT NULL,
	[CityGuid] [uniqueidentifier] NOT NULL,
	[PostCode] [nvarchar](50) NULL,
	[AddressType] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tblUserAddress_DateCreated]  DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_tblUserAddress_IsDeleted]  DEFAULT ((0)),
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_tblUserAddress] PRIMARY KEY CLUSTERED 
(
	[UserAddressGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[tblLookUpDomainValue] ADD  CONSTRAINT [DF_tblLookUpDomainValue_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tblLookUpDomainValue] ADD  CONSTRAINT [DF_tblLookUpDomainValue_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tblLookUpDomainValue] ADD  CONSTRAINT [DF_tblLookUpDomainValue_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[tblReview] ADD  CONSTRAINT [DF_tblReview_ReviewGuid]  DEFAULT (newid()) FOR [ReviewGuid]
GO
ALTER TABLE [dbo].[tblReview] ADD  CONSTRAINT [DF_tblReview_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]
GO
ALTER TABLE [dbo].[tblReview] ADD  CONSTRAINT [DF_tblReview_IsSkipped]  DEFAULT ((0)) FOR [IsSkipped]
GO
ALTER TABLE [dbo].[tblReview] ADD  CONSTRAINT [DF_tblReview_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[tblEmail]  WITH CHECK ADD  CONSTRAINT [FK_tblEmail_tblTemplate] FOREIGN KEY([TemplateGuid])
REFERENCES [dbo].[tblTemplate] ([TemplateGuid])
GO
ALTER TABLE [dbo].[tblEmail] CHECK CONSTRAINT [FK_tblEmail_tblTemplate]
GO
ALTER TABLE [dbo].[tblEmail]  WITH CHECK ADD  CONSTRAINT [FK_tblEmail_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblEmail] CHECK CONSTRAINT [FK_tblEmail_tblUser]
GO
ALTER TABLE [dbo].[tblEmailTo]  WITH CHECK ADD  CONSTRAINT [FK_tblEmailTo_tblUser] FOREIGN KEY([UserGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblEmailTo] CHECK CONSTRAINT [FK_tblEmailTo_tblUser]
GO
ALTER TABLE [dbo].[tblFavourite]  WITH CHECK ADD  CONSTRAINT [FK_tblFavourite_tblRestaurant] FOREIGN KEY([RestaurantGuid])
REFERENCES [dbo].[tblRestaurant] ([RestaurantGuid])
GO
ALTER TABLE [dbo].[tblFavourite] CHECK CONSTRAINT [FK_tblFavourite_tblRestaurant]
GO
ALTER TABLE [dbo].[tblFavourite]  WITH CHECK ADD  CONSTRAINT [FK_tblFavourite_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblFavourite] CHECK CONSTRAINT [FK_tblFavourite_tblUser]
GO
ALTER TABLE [dbo].[tblItem]  WITH CHECK ADD  CONSTRAINT [FK_tblItem_tblLookUpDomainValue] FOREIGN KEY([CategoryGuid])
REFERENCES [dbo].[tblLookUpDomainValue] ([LookUpDomainValueGuid])
GO
ALTER TABLE [dbo].[tblItem] CHECK CONSTRAINT [FK_tblItem_tblLookUpDomainValue]
GO
ALTER TABLE [dbo].[tblItem]  WITH CHECK ADD  CONSTRAINT [FK_tblItem_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblItem] CHECK CONSTRAINT [FK_tblItem_tblUser]
GO
ALTER TABLE [dbo].[tblItem]  WITH CHECK ADD  CONSTRAINT [FK_tblItem_tblUser1] FOREIGN KEY([DeletedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblItem] CHECK CONSTRAINT [FK_tblItem_tblUser1]
GO
ALTER TABLE [dbo].[tblOrder]  WITH CHECK ADD  CONSTRAINT [FK_tblOrder_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblUser]
GO
ALTER TABLE [dbo].[tblOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderItem_tblItem] FOREIGN KEY([ItemGuid])
REFERENCES [dbo].[tblItem] ([ItemGuid])
GO
ALTER TABLE [dbo].[tblOrderItem] CHECK CONSTRAINT [FK_tblOrderItem_tblItem]
GO
ALTER TABLE [dbo].[tblOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderItem_tblOrder1] FOREIGN KEY([OrderGuid])
REFERENCES [dbo].[tblOrder] ([OrderGuid])
GO
ALTER TABLE [dbo].[tblOrderItem] CHECK CONSTRAINT [FK_tblOrderItem_tblOrder1]
GO
ALTER TABLE [dbo].[tblOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderItem_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblOrderItem] CHECK CONSTRAINT [FK_tblOrderItem_tblUser]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblCity] FOREIGN KEY([CityGuid])
REFERENCES [dbo].[tblCity] ([CityGuid])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblCity]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblCountry] FOREIGN KEY([CountryGuid])
REFERENCES [dbo].[tblCountry] ([CountryGUID])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblCountry]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblUser] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblUser]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblUser1] FOREIGN KEY([DeletedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblUser1]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblUser2] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblUser2]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_CreatedByGuid] FOREIGN KEY([CreatedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_CreatedByGuid]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_DeletedBy] FOREIGN KEY([DeletedByGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_DeletedBy]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_ModifiedBy] FOREIGN KEY([UserGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_ModifiedBy]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_tblOrder] FOREIGN KEY([OrderGuid])
REFERENCES [dbo].[tblOrder] ([OrderGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_tblOrder]
GO
ALTER TABLE [dbo].[tblReview]  WITH NOCHECK ADD  CONSTRAINT [FK_tblReview_tblRestaurant] FOREIGN KEY([RestaurantGuid])
REFERENCES [dbo].[tblRestaurant] ([RestaurantGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_tblRestaurant]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_tblUser] FOREIGN KEY([UserGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_tblUser]
GO
ALTER TABLE [dbo].[tblUser]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblCity] FOREIGN KEY([PreferedCity])
REFERENCES [dbo].[tblCity] ([CityGuid])
GO
ALTER TABLE [dbo].[tblUser] CHECK CONSTRAINT [FK_tblUser_tblCity]
GO
ALTER TABLE [dbo].[tblUserAddress]  WITH CHECK ADD  CONSTRAINT [FK_tblUserAddress_tblCity] FOREIGN KEY([CityGuid])
REFERENCES [dbo].[tblCity] ([CityGuid])
GO
ALTER TABLE [dbo].[tblUserAddress] CHECK CONSTRAINT [FK_tblUserAddress_tblCity]
GO
ALTER TABLE [dbo].[tblUserAddress]  WITH CHECK ADD  CONSTRAINT [FK_tblUserAddress_tblUser] FOREIGN KEY([UserGuid])
REFERENCES [dbo].[tblUser] ([UserGuid])
GO
ALTER TABLE [dbo].[tblUserAddress] CHECK CONSTRAINT [FK_tblUserAddress_tblUser]
GO
USE [master]
GO
ALTER DATABASE [DEV_Bringly] SET  READ_WRITE 
GO
