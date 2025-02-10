#!/usr/bin/env bash
set -e

host="$1"
shift
cmd="$@"

until nc -z "$host" 1433; do
  echo "Waiting for SQL Server..."
  sleep 5
done

>&2 echo "SQL Server is up - executing command"
exec $cmd