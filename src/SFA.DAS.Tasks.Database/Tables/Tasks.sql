﻿CREATE TABLE [Tasks].[Tasks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Type] VARCHAR(100) NOT NULL, 
    [OwnerId] VARCHAR(50) NOT NULL, 
    [ItemsDueCount] INT NOT NULL
)