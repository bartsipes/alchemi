set nocount on

-- grp
insert grp values(1, 'Administrators')
insert grp values(2, 'Executors')
insert grp values(3, 'Users')

-- prm
insert prm values(1, 'ExecuteThread')
insert prm values(2, 'ManageOwnApp')
insert prm values(3, 'ManageAllApps')
insert prm values(4, 'ManageUsers')

-- grp_prm
insert grp_prm values(1, 1)
insert grp_prm values(1, 2)
insert grp_prm values(1, 3)
insert grp_prm values(1, 4)
insert grp_prm values(2, 1)
insert grp_prm values(3, 2)

-- usr
insert usr values('admin', 'admin', 1)
insert usr values('executor', 'executor', 2)
insert usr values('user', 'user', 3)





