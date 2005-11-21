use Alchemi

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
insert usr values('admin', '19a2854144b63a8f7617a6f225019b12', 1)
insert usr values('executor', '63c2867ae3b5ea1dccf158f6b084a9ec', 2)
insert usr values('user', '9ce4b5879f3fcb5a9842547bebe191e1', 3)





