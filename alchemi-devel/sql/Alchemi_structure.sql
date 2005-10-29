use Alchemi

/****** Object:  Stored Procedure dbo.Admon_GetExecutors    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_GetExecutors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_GetExecutors]
GO

/****** Object:  Stored Procedure dbo.Admon_GetUserList    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_GetUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_GetUserList]
GO

/****** Object:  Stored Procedure dbo.Cleanup    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Cleanup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Cleanup]
GO

/****** Object:  Stored Procedure dbo.Executor_Heartbeat    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_Heartbeat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_Heartbeat]
GO

/****** Object:  Stored Procedure dbo.Executor_Insert    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_Insert]
GO

/****** Object:  Stored Procedure dbo.Executor_SelectAvailableDedicated    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_SelectAvailableDedicated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_SelectAvailableDedicated]
GO

/****** Object:  Stored Procedure dbo.Executor_SelectExists    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_SelectExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_SelectExists]
GO

/****** Object:  Stored Procedure dbo.Executors_DiscoverDisconnectedNDE    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executors_DiscoverDisconnectedNDE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executors_DiscoverDisconnectedNDE]
GO

/****** Object:  Stored Procedure dbo.Executors_SelectDedicatedRunningThreads    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executors_SelectDedicatedRunningThreads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executors_SelectDedicatedRunningThreads]
GO

/****** Object:  Stored Procedure dbo.Thread_Reset    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Reset]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Reset]
GO

/****** Object:  Stored Procedure dbo.Thread_Schedule    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Schedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Schedule]
GO

/****** Object:  Stored Procedure dbo.Threads_SelectLostNDE    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Threads_SelectLostNDE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Threads_SelectLostNDE]
GO

/****** Object:  Stored Procedure dbo.User_Authenticate    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_Authenticate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_Authenticate]
GO

/****** Object:  Stored Procedure dbo.User_VerifyPermission    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_VerifyPermission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_VerifyPermission]
GO

/****** Object:  Stored Procedure dbo.Admon_Applications    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_Applications]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_Applications]
GO

/****** Object:  Stored Procedure dbo.Admon_SystemSummary    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_SystemSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_SystemSummary]
GO

/****** Object:  Stored Procedure dbo.Admon_UserApplications    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_UserApplications]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_UserApplications]
GO

/****** Object:  Stored Procedure dbo.Application_Insert    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Application_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Application_Insert]
GO

/****** Object:  Stored Procedure dbo.Application_Stop    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Application_Stop]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Application_Stop]
GO

/****** Object:  Stored Procedure dbo.Application_UpdateState    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Application_UpdateState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Application_UpdateState]
GO

/****** Object:  Stored Procedure dbo.Thread_Insert    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Insert]
GO

/****** Object:  Stored Procedure dbo.Thread_InsertNonPrimary    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_InsertNonPrimary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_InsertNonPrimary]
GO

/****** Object:  Stored Procedure dbo.Thread_SelectReady    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SelectReady]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SelectReady]
GO

/****** Object:  Stored Procedure dbo.Thread_SelectState    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SelectState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SelectState]
GO

/****** Object:  Stored Procedure dbo.Thread_SetFailed    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SetFailed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SetFailed]
GO

/****** Object:  Stored Procedure dbo.Thread_UpdateState    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_UpdateState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_UpdateState]
GO

/****** Object:  Stored Procedure dbo.Threads_UpdateStateAndSelect    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Threads_UpdateStateAndSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Threads_UpdateStateAndSelect]
GO

/****** Object:  Stored Procedure dbo.User_VerifyApplicationCreator    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_VerifyApplicationCreator]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_VerifyApplicationCreator]
GO

/****** Object:  Stored Procedure dbo.VerifyConnection    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VerifyConnection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[VerifyConnection]
GO

/****** Object:  Table [dbo].[application]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[application]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[application]
GO

/****** Object:  Table [dbo].[executor]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[executor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[executor]
GO

/****** Object:  Table [dbo].[grp]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp]
GO

/****** Object:  Table [dbo].[grp_prm]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp_prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp_prm]
GO

/****** Object:  Table [dbo].[prm]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[prm]
GO

/****** Object:  Table [dbo].[thread]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[thread]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[thread]
GO

/****** Object:  Table [dbo].[usr]    Script Date: 29/09/2005 8:03:11 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[usr]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[usr]
GO

/****** Object:  Table [dbo].[application]    Script Date: 29/09/2005 8:03:12 PM ******/
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

/****** Object:  Table [dbo].[executor]    Script Date: 29/09/2005 8:03:14 PM ******/
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
	[arch] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[os] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[grp]    Script Date: 29/09/2005 8:03:14 PM ******/
CREATE TABLE [dbo].[grp] (
	[grp_id] [int] NOT NULL ,
	[grp_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[grp_prm]    Script Date: 29/09/2005 8:03:14 PM ******/
CREATE TABLE [dbo].[grp_prm] (
	[grp_id] [int] NOT NULL ,
	[prm_id] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[prm]    Script Date: 29/09/2005 8:03:14 PM ******/
CREATE TABLE [dbo].[prm] (
	[prm_id] [int] NOT NULL ,
	[prm_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[thread]    Script Date: 29/09/2005 8:03:14 PM ******/
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

/****** Object:  Table [dbo].[usr]    Script Date: 29/09/2005 8:03:14 PM ******/
CREATE TABLE [dbo].[usr] (
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[grp_id] [int] NOT NULL 
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Admon_Applications    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Admon_Applications    Script Date: 9/08/2005 3:33:03 PM ******/








CREATE PROCEDURE Admon_Applications

AS

create table #apps
(
  application_id uniqueidentifier,
  total_threads int,
  unfinished_threads int
)

insert #apps
select
thread.application_id, count(internal_thread_id) as total_threads, null from thread
group by thread.application_id

update #apps set unfinished_threads = (select count(internal_thread_id) from thread where state in (0, 1, 2) and thread.application_id = #apps.application_id)

select application.application_id, usr_name, state, time_created, /*CONVERT(VARCHAR(11), getdate() - time_created, 108) as uptime,*/ total_threads, unfinished_threads from #apps
inner join application on #apps.application_id = application.application_id order by time_created desc






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Admon_SystemSummary    Script Date: 29/09/2005 8:03:14 PM ******/


/****** Object:  Stored Procedure dbo.Admon_SystemSummary    Script Date: 9/08/2005 3:33:03 PM ******/








CREATE PROCEDURE Admon_SystemSummary

AS

create table #summary
(
  total_executors int,
  max_power varchar(100),
  power_usage int,
  power_avail int,
  power_totalusage varchar(100),
  unfinished_threads int,
  unfinished_apps int
)

-- executor info
insert into #summary
(
  total_executors,
  max_power,
  power_usage,
  power_avail,
  power_totalusage
)
select count(*) as total_executors, convert(varchar, cast(isnull(sum(cpu_max), 0) as float)/1000 ) + ' GHz', isnull(avg(cpu_usage), 0), isnull(avg(cpu_avail), 0), convert(varchar, isnull(sum(cpu_totalusage * cpu_max / (3600 * 1000)), 0)) + ' GHz*Hr' from executor where is_connected = 1

-- thread info
update #summary set unfinished_threads = (select count(*) from thread where state not in (3, 4))

-- app info
update #summary set unfinished_apps = 
(select count(*) from application,thread
where application.application_id=thread.application_id
and thread.state not in (3,4))


select * from #summary






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Admon_UserApplications    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Admon_UserApplications    Script Date: 9/08/2005 3:33:03 PM ******/
CREATE PROCEDURE Admon_UserApplications
	(
		@user_name uniqueidentifier
	)
AS

create table #apps
(
  application_id uniqueidentifier,
  total_threads int,
  unfinished_threads int
)

insert #apps
select
thread.application_id, count(internal_thread_id) as total_threads, null from thread, application
where thread.application_id = application.application_id and application.usr_name = @user_name
group by thread.application_id

update #apps set unfinished_threads = (select count(internal_thread_id) from thread where state in (0, 1, 2) and thread.application_id = #apps.application_id)

select application.application_id, state, time_created, time_completed, application_name, total_threads, unfinished_threads from #apps
inner join application on #apps.application_id = application.application_id order by time_created desc






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Application_Insert    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Application_Insert    Script Date: 9/08/2005 3:33:03 PM ******/












CREATE            PROCEDURE 

Application_Insert
(
  @usr_name varchar(50)
)

AS

set nocount on

declare @id uniqueidentifier

select @id = newid()

insert application(application_id, state, is_primary, time_created, usr_name)
  values(@id, 0, 1, getdate(), @usr_name)
select @id as id










GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Application_Stop    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Application_Stop    Script Date: 9/08/2005 3:33:03 PM ******/













CREATE            PROCEDURE 

Application_Stop
(
  @application_id uniqueidentifier
)

AS

set nocount on

create table #de_threads
(
  thread_id int,
  executor_id uniqueidentifier,
)

insert into #de_threads
select thread_id, executor_id from thread where state <> 4

update thread set state = 4 where application_id = @application_id

select * from #de_threads where executor_id <> null




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Application_UpdateState    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Application_UpdateState    Script Date: 9/08/2005 3:33:03 PM ******/





CREATE    PROCEDURE Application_UpdateState
(
  @application_id uniqueidentifier,
  @state int
)

AS

update application set state = @state where application_id = @application_id







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_Insert    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Thread_Insert    Script Date: 9/08/2005 3:33:03 PM ******/












CREATE          PROCEDURE Thread_Insert
(
  @application_id uniqueidentifier,
  @thread_id int
)

AS

insert thread(application_id, thread_id, state)
values (@application_id, @thread_id, 0)






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_InsertNonPrimary    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Thread_InsertNonPrimary    Script Date: 9/08/2005 3:33:03 PM ******/












CREATE          PROCEDURE Thread_InsertNonPrimary
(
  @application_id uniqueidentifier,
  @thread_id int,
  @priority int
)

AS

if not exists (select application_id from application where application_id = @application_id) begin
  insert application(application_id, state, is_primary)
  values (@application_id, 0, 0)
end

insert thread(application_id, thread_id, state, priority)
values (@application_id, @thread_id, 0, @priority)







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_SelectReady    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Thread_SelectReady    Script Date: 9/08/2005 3:33:03 PM ******/












CREATE               PROCEDURE Thread_SelectReady
AS


select * from thread where state = 0 order by newid()/*priority, internal_thread_id*/





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_SelectState    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Thread_SelectState    Script Date: 9/08/2005 3:33:03 PM ******/






CREATE      PROCEDURE Thread_SelectState
(
  @application_id uniqueidentifier,
  @thread_id int
)

AS

select state from thread where application_id = @application_id and thread_id = @thread_id




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_SetFailed    Script Date: 29/09/2005 8:03:14 PM ******/

/****** Object:  Stored Procedure dbo.Thread_SetFailed    Script Date: 9/08/2005 3:33:03 PM ******/






CREATE     PROCEDURE Thread_SetFailed
(
  @application_id uniqueidentifier,
  @thread_id int
)

AS

update thread set failed = 1 where application_id = @application_id and thread_id = @thread_id





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_UpdateState    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Thread_UpdateState    Script Date: 9/08/2005 3:33:03 PM ******/





CREATE     PROCEDURE Thread_UpdateState
(
  @application_id uniqueidentifier,
  @thread_id int,
  @state int
)

AS

update thread set state = @state where application_id = @application_id and thread_id = @thread_id








GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Threads_UpdateStateAndSelect    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Threads_UpdateStateAndSelect    Script Date: 9/08/2005 3:33:03 PM ******/











CREATE            PROCEDURE Threads_UpdateStateAndSelect
(
  @application_id uniqueidentifier,
  @old_state int,
  @new_state int
)

AS

create table #return_table
(
  internal_thread_id int
)

insert #return_table
  select internal_thread_id from thread where application_id = @application_id and state = @old_state

update thread
  set state = @new_state
  where internal_thread_id in (select internal_thread_id from #return_table)

select thread_id from thread inner join #return_table on thread.internal_thread_id = #return_table.internal_thread_id










GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.User_VerifyApplicationCreator    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.User_VerifyApplicationCreator    Script Date: 9/08/2005 3:33:03 PM ******/














CREATE         PROCEDURE User_VerifyApplicationCreator
(
  @usr_name varchar(50),
  @application_id uniqueidentifier
)

AS

select count(*) as verified from application usr where usr_name = @usr_name and application_id = @application_id





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.VerifyConnection    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.VerifyConnection    Script Date: 9/08/2005 3:33:03 PM ******/



CREATE      PROCEDURE VerifyConnection

AS

-- intentionally left blank





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Admon_GetExecutors    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Admon_GetExecutors    Script Date: 9/08/2005 3:33:03 PM ******/




















CREATE             PROCEDURE Admon_GetExecutors

AS

select executor_id, host, port, usr_name, is_connected, is_dedicated, cpu_max, convert(varchar, cpu_totalusage * cpu_max / (3600 * 1000)) as cpu_totalusage from executor order by is_connected desc









GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Admon_GetUserList    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Admon_GetUserList    Script Date: 9/08/2005 3:33:03 PM ******/
















CREATE         PROCEDURE Admon_GetUserList

AS

select usr_name, password, grp.grp_id from usr inner join grp on grp.grp_id = usr.grp_id






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Cleanup    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Cleanup    Script Date: 9/08/2005 3:33:03 PM ******/








CREATE       PROCEDURE Cleanup
AS

delete thread
delete executor
delete application










GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Executor_Heartbeat    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executor_Heartbeat    Script Date: 9/08/2005 3:33:03 PM ******/


















CREATE            PROCEDURE Executor_Heartbeat
(
  @executor_id uniqueidentifier,
  @interval int,
  @cpu_usage int,
  @cpu_avail int
)

AS

update executor set ping_time = getdate(), is_connected = 1, cpu_usage = @cpu_usage, cpu_avail = @cpu_avail, cpu_totalusage = cpu_totalusage + @interval * cast(@cpu_usage as float) / 100 where executor_id = @executor_id






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Executor_Insert    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executor_Insert    Script Date: 9/08/2005 3:33:03 PM ******/

















CREATE PROCEDURE 

Executor_Insert
(
  @is_dedicated bit,
  @usr_name varchar(50),
  @hostname varchar(30),
  @cpu_max int,
  @mem_max float,
  @disk_max float,
  @num_cpus int,
  @os varchar(15),
  @arch varchar(15)
)
--to do: set limits and costs, for this.
AS

set nocount on

declare @id uniqueidentifier

select @id = newid()

insert executor(executor_id, is_connected, is_dedicated, usr_name, host, cpu_max, mem_max, disk_max, num_cpus, os, arch) values(@id, 0, @is_dedicated, @usr_name, @hostname, @cpu_max, @mem_max, @disk_max, @num_cpus, @os, @arch)
select @id as id










GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Executor_SelectAvailableDedicated    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executor_SelectAvailableDedicated    Script Date: 9/08/2005 3:33:03 PM ******/











create              PROCEDURE Executor_SelectAvailableDedicated
AS

select * from executor
where executor_id not in(
  select executor_id from thread where state in (1, 2)
)
and is_dedicated = 1 and is_connected = 1




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Executor_SelectExists    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executor_SelectExists    Script Date: 9/08/2005 3:33:03 PM ******/











CREATE PROCEDURE 

Executor_SelectExists
(
  @executor_id uniqueidentifier,
  @hostname varchar(50)
)

AS

set nocount on
declare @executor_exists varchar(5)
declare @host varchar(30)

if exists(select executor_id from executor where executor_id = @executor_id) begin
	if (@hostname=(select host from executor where executor_id = @executor_id) ) begin
		set @executor_exists = 'true'
	end else begin
		set @executor_exists = 'false'
	end
end else begin
	set @executor_exists = 'false'
end

select @executor_exists as executor_exists






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Executors_DiscoverDisconnectedNDE    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executors_DiscoverDisconnectedNDE    Script Date: 9/08/2005 3:33:03 PM ******/















CREATE               PROCEDURE Executors_DiscoverDisconnectedNDE
(
  @timeout int
)

AS

update executor set is_connected = 0 where datediff(s, ping_time, getdate()) > @timeout







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Executors_SelectDedicatedRunningThreads    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Executors_SelectDedicatedRunningThreads    Script Date: 9/08/2005 3:33:03 PM ******/












CREATE          PROCEDURE Executors_SelectDedicatedRunningThreads
AS

select thread.executor_id, application_id, thread_id from thread inner join executor on executor.executor_id = thread.executor_id where executor.is_dedicated = 1 and state in (1,2)







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Thread_Reset    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Thread_Reset    Script Date: 9/08/2005 3:33:03 PM ******/







CREATE PROCEDURE Thread_Reset
(
  @application_id uniqueidentifier,
  @thread_id int
)

AS

update thread set state = 0, executor_id = null, time_started = null where application_id = @application_id and thread_id = @thread_id and state <> 4








GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Thread_Schedule    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Thread_Schedule    Script Date: 9/08/2005 3:33:03 PM ******/





















CREATE                                PROCEDURE Thread_Schedule
(
  @executor_id uniqueidentifier -- optional (only supplied for non-dedicated scheduling)
)

AS

--
-- get executor if no executor supplied (dedicated scheduling)
--
if @executor_id = null begin
  set @executor_id = 
  (
    select top 1 executor_id from executor
    where executor_id not in(
      select executor_id from thread where state in (1, 2)
    )
    and is_dedicated = 1 and is_connected = 1
  )
end

if @executor_id = null begin
  -- no dedicated executor to schedule thread for, so return
  return -- select null as application_id, -1 as thread_id, null as executor_id, 0 as priority
end else begin
  --
  -- schedule thread
  --
  declare @internal_thread_id int
  -- get next ready thread
  set @internal_thread_id = (select top 1 internal_thread_id from thread where state = 0 order by priority, internal_thread_id /*newid()*/)
  if (@internal_thread_id = null) begin
    -- no threads to execute, so return
    return -- select null as application_id, -1 as thread_id, null as executor_id, 0 as priority
  end else begin
    -- return details of scheduled thread
    select application_id, thread_id, priority, @executor_id as executor_id from thread where internal_thread_id = @internal_thread_id
  end
end


























GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.Threads_SelectLostNDE    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.Threads_SelectLostNDE    Script Date: 9/08/2005 3:33:03 PM ******/

















CREATE                 PROCEDURE Threads_SelectLostNDE

AS

select executor.executor_id, thread.application_id, thread.thread_id from executor
inner join thread on thread.executor_id = executor.executor_id
where is_dedicated = 0
and is_connected = 0
and thread.state not in (0)











GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.User_Authenticate    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.User_Authenticate    Script Date: 9/08/2005 3:33:03 PM ******/













CREATE          PROCEDURE User_Authenticate
(
  @usr_name varchar(50),
  @password varchar(50)
)

AS

select count(*) as authenticated from usr where usr_name = @usr_name and password = @password





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.User_VerifyPermission    Script Date: 29/09/2005 8:03:15 PM ******/

/****** Object:  Stored Procedure dbo.User_VerifyPermission    Script Date: 9/08/2005 3:33:03 PM ******/















CREATE PROCEDURE User_VerifyPermission
(
  @usr_name varchar(50),
  @prm_id int
)

AS

select count(*) as permitted from usr
inner join grp on grp.grp_id = usr.grp_id
inner join grp_prm on grp_prm.grp_id = grp.grp_id
inner join prm on prm.prm_id = grp_prm.prm_id
where usr.usr_name = @usr_name and prm.prm_id >= @prm_id






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


exec sp_addextendedproperty N'MS_Description', N'Default priority=5 on a scale of 1 to 10', N'user', N'dbo', N'table', N'thread', N'column', N'priority'


GO

