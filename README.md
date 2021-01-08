# EfProblemSample.WebApi

This project demonstrates a weird behavior of EFCore 5.0.1 when encounting an exception from the database.
The framework appears to try to load all data from all tables into memory after the exception occurs.

## What does this sample do?
The sample is a simple WebApi with two entities Entity1 and Entity2. 
- Entity1 has a Property with a unique constraint
- Entity2 is only here to demonstrate that that all tables (not only the one experiencing a violation) are loaded into memory

The controller for Entity1 has a hardcoded way of forcing simultanious inserts that will violate the Unique index constraint in the database.
Of course this is not a real world sample, but we have experienced this behaviour when multiple requests from various clients causes db exceptions, resulting in gigabytes of data being loaded into memory, and effectively taking a node in our cluster down.

## To reproduce the behaviour
1. Build the database using migrations
2. Start the webapplication
3. Use swagger to envoke the PUT endpoint on Entity1
4. Consult the logs. You will find the following:

>[13:25:54 ERR] Failed executing DbCommand (271ms) [Parameters=[@p0='?' (Size = 19), @p1='?' (Size = 9)], CommandType='Text', CommandTimeout='30']
INSERT INTO "Entity1s" ("AUniqueProperty", "SomeProperty")
VALUES (@p0, @p1);
SELECT "Id"
FROM "Entity1s"
WHERE changes() = 1 AND "rowid" = last_insert_rowid();
[13:25:55 INF] Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT "e"."Id", "e"."SomeOtherProperty"
FROM "Entity2s" AS "e"
[13:25:55 INF] Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT "e"."Id", "e"."AUniqueProperty", "e"."SomeProperty"
FROM "Entity1s" AS "e"

**Notice the last two selects!**

## What have we tried to do
- Downgrading to EFCore 3.1.10 seems to remove the problem
- We have not been able to reproduce the behaviour from a simple console application like the [GetStarted sample](https://github.com/dotnet/EntityFramework.Docs/tree/master/samples/core/GetStarted) (of course still using EFCore 5.0.1)
- Using SqlServer rather than SqlLite results in the same behaviour 

