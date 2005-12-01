/****** Object:  Table [dbo].[application]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[application]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[application]
GO

/****** Object:  Table [dbo].[executor]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[executor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[executor]
GO

/****** Object:  Table [dbo].[grp]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp]
GO

/****** Object:  Table [dbo].[grp_prm]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp_prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp_prm]
GO

/****** Object:  Table [dbo].[prm]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[prm]
GO

/****** Object:  Table [dbo].[thread]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[thread]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[thread]
GO

/****** Object:  Table [dbo].[usr]    Script Date: 29/11/2005 11:16:25 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[usr]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[usr]
GO

/****** Object:  Table [dbo].[application]    Script Date: 29/11/2005 11:16:25 PM ******/
CREATE TABLE [dbo].[application] (
	[application_id] [uniqueidentifier] NOT NULL ,
	[state] [int] NOT NULL ,
	[time_created] [datetime] NULL ,
	[is_primary] [bit] NOT NULL ,
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[application_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[time_completed] [datetime] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[executor]    Script Date: 29/11/2005 11:16:25 PM ******/
CREATE TABLE [dbo].[executor] (
	[executor_id] [uniqueidentifier] NOT NULL ,
	[is_dedicated] [bit] NOT NULL ,
	[is_connected] [bit] NOT NULL ,
	[ping_time] [datetime] NULL ,
	[host] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[port] [int] NULL ,
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cpu_max] [int] NULL ,
	[cpu_usage] [int] NULL ,
	[cpu_avail] [int] NULL ,
	[cpu_totalusage] [float] NULL ,
	[mem_max] [float] NULL ,
	[disk_max] [float] NULL ,
	[num_cpus] [int] NULL ,
	[cpuLimit] [float] NULL ,
	[memLimit] [float] NULL ,
	[diskLimit] [float] NULL ,
	[costPerCPUSec] [float] NULL ,
	[costPerThread] [float] NULL ,
	[costPerDiskMB] [float] NULL ,
	[arch] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[os] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[grp]    Script Date: 29/11/2005 11:16:25 PM ******/
CREATE TABLE [dbo].[grp] (
	[grp_id] [int] NOT NULL ,
	[grp_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[grp_prm]    Script Date: 29/11/2005 11:16:26 PM ******/
CREATE TABLE [dbo].[grp_prm] (
	[grp_id] [int] NOT NULL ,
	[prm_id] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[prm]    Script Date: 29/11/2005 11:16:26 PM ******/
CREATE TABLE [dbo].[prm] (
	[prm_id] [int] NOT NULL ,
	[prm_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[thread]    Script Date: 29/11/2005 11:16:26 PM ******/
CREATE TABLE [dbo].[thread] (
	[internal_thread_id] [int] IDENTITY (1, 1) NOT NULL ,
	[application_id] [uniqueidentifier] NOT NULL ,
	[executor_id] [uniqueidentifier] NULL ,
	[thread_id] [int] NOT NULL ,
	[state] [int] NOT NULL ,
	[time_started] [datetime] NULL ,
	[time_finished] [datetime] NULL ,
	[priority] [int] NULL ,
	[failed] [bit] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[usr]    Script Date: 29/11/2005 11:16:26 PM ******/
CREATE TABLE [dbo].[usr] (
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[grp_id] [int] NOT NULL ,
	[description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO


exec sp_addextendedproperty N'MS_Description', N'Default priority=5 on a scale of 1 to 10', N'user', N'dbo', N'table', N'thread', N'column', N'priority'


GO

