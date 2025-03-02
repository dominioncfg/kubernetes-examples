pgSqlConnectionString: Host=students-db-students-database-db-svc0;Port=5432;Database=studentsdb;Username=studentApiAdmin;Password=PataDeCabra@2020;Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;Connection Lifetime=0;




//Database
helm template students-db ./database
helm install students-db ./database
helm upgrade students-db ./database
helm uninstall students-db ./database

//Database Migrator

helm template students-db-migrator ./database-migrator
helm install students-db-migrator ./database-migrator
helm upgrade students-db-migrator ./database-migrator

//Backend
students-database-migrator-job


//Sql Container
psql -h students-db-students-database-db-rset-0.students-db-students-database-db-svc -U studentApiAdmin -d studentsdb
