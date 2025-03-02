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
helm template students-backend ./backend
helm install students-backend ./backend
helm upgrade students-backend ./backend
helm uninstall students-backend ./backend

//Frontend
helm template students-frontend ./frontend
helm install students-frontend ./frontend
helm upgrade students-frontend  ./frontend
helm uninstall students-frontend ./frontend


//Routes
helm template students-app-rules ./ingress-configuration-routes
helm install students-app-rules ./ingress-configuration-routes
helm upgrade students-app-rules ./ingress-configuration-routes
helm uninstallstudents-app-rules ./ingress-configuration-routes


//Sql Container
psql -h students-db-students-database-db-rset-0.students-db-students-database-db-svc -U studentApiAdmin -d studentsdb
