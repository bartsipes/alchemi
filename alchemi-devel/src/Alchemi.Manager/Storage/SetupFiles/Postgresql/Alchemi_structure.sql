drop table if exists application;

drop table if exists executor;

drop table if exists grp;

drop table if exists grp_prm;

drop table if exists prm;

drop table if exists thread;

drop table if exists usr;

CREATE TABLE application (
	application_id varchar(50) NOT NULL ,
	state int NOT NULL ,
	time_created timestamp NULL ,
	is_primary boolean NOT NULL ,
	usr_name varchar (50) NOT NULL ,
	application_name varchar (50) NULL ,
	time_completed timestamp NULL 
);

CREATE TABLE executor (
	executor_id varchar(50) NOT NULL ,
	is_dedicated boolean NOT NULL ,
	is_connected boolean NOT NULL ,
	ping_time timestamp NULL ,
	host varchar (100) NULL ,
	port int NULL ,
	usr_name varchar (50) NULL ,
	cpu_max int NULL ,
	cpu_usage int NULL ,
	cpu_avail int NULL ,
	cpu_totalusage float NULL ,
	mem_max float NULL ,
	disk_max float NULL ,
	num_cpus int NULL ,
	cpuLimit float NULL ,
	memLimit float NULL ,
	diskLimit float NULL ,
	costPerCPUSec float NULL ,
	costPerThread float NULL ,
	costPerDiskMB float NULL ,
	arch varchar (50) NULL ,
	os varchar (50) NULL 
);

CREATE TABLE grp (
	grp_id int NOT NULL ,
	grp_name varchar (50) NOT NULL ,
	description varchar (255) NULL ,
	is_system boolean NOT NULL 
);

CREATE TABLE grp_prm (
	grp_id int NOT NULL ,
	prm_id int NOT NULL 
);

CREATE TABLE prm (
	prm_id int NOT NULL ,
	prm_name varchar (50) NOT NULL ,
	description varchar (255) NULL 
);

CREATE TABLE thread (
	internal_thread_id serial NOT NULL,
	application_id varchar(50) NOT NULL ,
	executor_id varchar(50) NULL ,
	thread_id int NOT NULL ,
	state int NOT NULL ,
	time_started timestamp NULL ,
	time_finished timestamp NULL ,
	priority int NULL ,
	failed boolean NULL,
	PRIMARY KEY (internal_thread_id)
);

CREATE TABLE usr (
	usr_name varchar (50) NOT NULL ,
	password varchar (50) NOT NULL ,
	grp_id int NOT NULL ,
	description varchar (255) NULL ,
	is_system boolean NOT NULL 
);

--Force Postgres to accept 1 and 0 instead of proper Postgres boolean values

UPDATE pg_cast set castcontext = 'i'
WHERE (castsource = (SELECT oid FROM pg_type WHERE typname = 'bool')
       AND casttarget = (SELECT oid FROM pg_type WHERE typname = 'int4'));

UPDATE pg_cast set castcontext = 'a'
WHERE (castsource = (SELECT oid FROM pg_type WHERE typname = 'int4')
       AND casttarget = (SELECT oid FROM pg_type WHERE typname = 'bool'));