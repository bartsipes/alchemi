set quoted_identifier  OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Cleanup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Cleanup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_SelectAvailableDedicated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_SelectAvailableDedicated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executors_SelectDedicatedRunningThreads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executors_SelectDedicatedRunningThreads]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Insert]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_InsertNonPrimary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_InsertNonPrimary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Reset]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Reset]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_Schedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_Schedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SelectReady]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SelectReady]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SelectState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SelectState]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_SetFailed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_SetFailed]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Thread_UpdateState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Thread_UpdateState]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Threads_SelectLostNDE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Threads_SelectLostNDE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_GetExecutors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_GetExecutors]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_Heartbeat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_Heartbeat]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_VerifyApplicationCreator]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_VerifyApplicationCreator]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_VerifyPermission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_VerifyPermission]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_GetUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_GetUserList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_Applications]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_Applications]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Admon_SystemSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Admon_SystemSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Application_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Application_Insert]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Application_UpdateState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Application_UpdateState]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_Insert]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executor_SelectExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executor_SelectExists]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Executors_DiscoverDisconnectedNDE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Executors_DiscoverDisconnectedNDE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Threads_UpdateStateAndSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Threads_UpdateStateAndSelect]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_Authenticate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[User_Authenticate]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VerifyConnection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[VerifyConnection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[thread]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[thread]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[application]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[application]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[executor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[executor]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp_prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp_prm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[grp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[prm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[usr]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[usr]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[grp] (
	[grp_id] [int] NOT NULL ,
	[grp_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_grp] PRIMARY KEY  CLUSTERED 
	(
		[grp_id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[prm] (
	[prm_id] [int] NOT NULL ,
	[prm_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_prm] PRIMARY KEY  CLUSTERED 
	(
		[prm_id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[usr]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[usr] (
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[grp_id] [int] NOT NULL ,
	CONSTRAINT [PK_usr] PRIMARY KEY  CLUSTERED 
	(
		[usr_name]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[application]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[application] (
	[application_id] [uniqueidentifier] NOT NULL ,
	[state] [int] NOT NULL ,
	[time_created] [datetime] NULL ,
	[is_primary] [bit] NOT NULL ,
	[usr_name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_application_1] PRIMARY KEY  CLUSTERED 
	(
		[application_id]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_application_usr1] FOREIGN KEY 
	(
		[usr_name]
	) REFERENCES [dbo].[usr] (
		[usr_name]
	)
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[executor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
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
	[cpu_totalusage] [float] NULL CONSTRAINT [DF_executor_cpu_totalusage] DEFAULT (0),
	CONSTRAINT [PK_executor] PRIMARY KEY  CLUSTERED 
	(
		[executor_id]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_executor_usr] FOREIGN KEY 
	(
		[usr_name]
	) REFERENCES [dbo].[usr] (
		[usr_name]
	)
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[grp_prm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[grp_prm] (
	[grp_id] [int] NOT NULL ,
	[prm_id] [int] NOT NULL ,
	CONSTRAINT [PK_grp_prm] PRIMARY KEY  CLUSTERED 
	(
		[grp_id],
		[prm_id]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_grp_prm_grp] FOREIGN KEY 
	(
		[grp_id]
	) REFERENCES [dbo].[grp] (
		[grp_id]
	),
	CONSTRAINT [FK_grp_prm_prm] FOREIGN KEY 
	(
		[prm_id]
	) REFERENCES [dbo].[prm] (
		[prm_id]
	)
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[thread]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[thread] (
	[internal_thread_id] [int] IDENTITY (1, 1) NOT NULL ,
	[application_id] [uniqueidentifier] NOT NULL ,
	[executor_id] [uniqueidentifier] NULL ,
	[thread_id] [int] NOT NULL ,
	[state] [int] NOT NULL ,
	[time_started] [datetime] NULL ,
	[time_finished] [datetime] NULL ,
	[priority] [int] NOT NULL CONSTRAINT [DF_thread_priority] DEFAULT (5),
	[failed] [bit] NOT NULL CONSTRAINT [DF_thread_failed] DEFAULT (0),
	CONSTRAINT [PK_thread_1] PRIMARY KEY  CLUSTERED 
	(
		[internal_thread_id]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_thread_application] FOREIGN KEY 
	(
		[application_id]
	) REFERENCES [dbo].[application] (
		[application_id]
	),
	CONSTRAINT [FK_thread_executor] FOREIGN KEY 
	(
		[executor_id]
	) REFERENCES [dbo].[executor] (
		[executor_id]
	)
) ON [PRIMARY]
END

GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO








CREATE     PROCEDURE Admon_Applications

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
SET ANSI_NULLS OFF 
GO








CREATE      PROCEDURE Admon_SystemSummary

AS

create table #summary
(
  total_executors int,
  max_power varchar(100),
  power_usage int,
  power_avail int,
  power_totalusage varchar(100),
  unfinished_threads int
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
select count(*) as total_executors, convert(varchar, cast(isnull(sum(cpu_max), 0) as float)/1000 ) + ' GHz', isnull(avg(cpu_usage), 0), isnull(avg(cpu_avail), 0), convert(varchar, sum(cpu_totalusage * cpu_max / (3600 * 1000))) + ' GHz*Hr' from executor where is_connected = 1

-- thread info
update #summary set unfinished_threads = (select count(*) from thread where state not in (3, 4))

select * from #summary



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO












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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO





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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

















CREATE               PROCEDURE 

Executor_Insert
(
  @is_dedicated bit,
  @usr_name varchar(50),
  @cpu_max int
)

AS

set nocount on

declare @id uniqueidentifier

select @id = newid()

insert executor(executor_id, is_dedicated, is_connected, cpu_max, usr_name) values(@id, @is_dedicated, 0, @cpu_max, @usr_name)
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











CREATE         PROCEDURE 

Executor_SelectExists
(
  @executor_id uniqueidentifier
)

AS

set nocount on

if exists(select executor_id from executor where executor_id = @executor_id) begin
  select 'true' as executor_exists
end else begin
  select 'false' as executor_exists
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
SET ANSI_NULLS OFF 
GO



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
SET ANSI_NULLS OFF 
GO














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















CREATE          PROCEDURE User_VerifyPermission
(
  @usr_name varchar(50),
  @prm_id int
)

AS

select count(*) as permitted from usr
inner join grp on grp.grp_id = usr.grp_id
inner join grp_prm on grp_prm.grp_id = grp.grp_id
inner join prm on prm.prm_id = grp_prm.prm_id
where usr.usr_name = @usr_name and prm.prm_id = @prm_id




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO








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
SET ANSI_NULLS OFF 
GO












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
SET ANSI_NULLS OFF 
GO












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







CREATE      PROCEDURE Thread_Reset
(
  @application_id uniqueidentifier,
  @thread_id int
)

AS

update thread set state = 0, executor_id = null, time_started = null where application_id = @application_id and thread_id = @thread_id






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


















CREATE                             PROCEDURE Thread_Schedule
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

