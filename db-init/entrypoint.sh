#!/bin/bash
set -e

echo "Waiting for SQL Server to be ready..."
sleep 10  # Ensuring MSSQL is up before running the script

/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "YourStrong!Passw0rd" -d master -i /db-init/init.sql

echo "Database initialized!"