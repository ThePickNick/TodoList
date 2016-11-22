-- Original table schema
CREATE TABLE [AddTask] (
    [TaskID] integer PRIMARY KEY NOT NULL,
    [TaskOwner] varchar(50) NOT NULL,
    [TaskName] varchar(50) NOT NULL,
    [TaskDescription] varchar(500),
    [TaskCompleted] varchar(50) NOT NULL DEFAULT No,
    [TaskDate] varchar(50) NOT NULL,
    [TaskDays] varchar(50) NOT NULL
    Unique(TaskOwner, TaskName, TaskDate)
);
