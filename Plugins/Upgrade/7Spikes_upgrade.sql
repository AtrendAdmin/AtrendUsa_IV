DECLARE @attachmentEntityTypeId INT
SET @attachmentEntityTypeId = 5
DECLARE @htmlWidgetEntityTypeId INT
SET @htmlWidgetEntityTypeId = 25

/* =============== Remove leading/trailing newlines + spaces in SevenSpikes resource values =============== */
UPDATE [LocaleStringResource]
SET [ResourceValue] = LTRIM(STUFF([ResourceValue], 1, 1, ''))
WHERE LEFT(LTRIM([ResourceValue]), 1) = CHAR(10) AND ResourceName LIKE 'SevenSpikes%'
  
UPDATE [LocaleStringResource]
SET [ResourceValue] = REVERSE(STUFF(REVERSE(RTRIM([ResourceValue])), 1, 1, ''))
WHERE RIGHT(RTRIM([ResourceValue]), 1) = CHAR(10) AND ResourceName LIKE 'SevenSpikes%'

/* =============== Create the scheduling table =============== */
IF(NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_S_Schedule]') AND type in (N'U')))
BEGIN
	CREATE TABLE [dbo].[SS_S_Schedule] (
		[Id]                      INT      IDENTITY (1, 1) NOT NULL,
		[EntityType]              INT      NOT NULL,
		[EntityId]                INT      NOT NULL,
		[EntityFromDate]          DATETIME NULL,
		[EntityToDate]            DATETIME NULL,
		[SchedulePatternType]     INT      NOT NULL,
		[SchedulePatternFromTime] TIME (7) NULL,
		[SchedulePatternToTime]   TIME (7) NULL,
		[ExactDayValue]           INT      NULL,
		[EveryMonthFromDayValue]  INT      NULL,
		[EveryMonthToDayValue]    INT      NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);
END

/* =============== Create/Update the entity mapping table =============== */
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_MAP_EntityMapping]') AND type in (N'U'))
	AND NOT EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id(N'[dbo].[SS_MAP_EntityMapping]') and NAME='EntityType'))
BEGIN
	
	BEGIN TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;

		CREATE TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] (
			[Id]             INT IDENTITY (1, 1) NOT NULL,
			[EntityType]     INT NULL,
			[EntityId]       INT NOT NULL,
			[MappedEntityId] INT NOT NULL,
			[DisplayOrder]   INT NOT NULL,
			[MappingType]    INT NOT NULL,
			[EntityName]     NVARCHAR (MAX) NOT NULL,
			PRIMARY KEY CLUSTERED ([Id] ASC)
		)
		
		create table #dummy_entityMappings
		(i int)
		
		IF EXISTS (SELECT NULL FROM   [dbo].[SS_MAP_EntityMapping])
		BEGIN
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] ON;
			INSERT INTO [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] ([Id], [EntityId], [MappedEntityId], [DisplayOrder], [MappingType], [EntityName])
			SELECT   [Id],
					 [EntityId],
					 [MappedEntityId],
					 [DisplayOrder],
					 [MappingTypeId],
					 [EntityName]
			FROM     [dbo].[SS_MAP_EntityMapping]
			WHERE NOT EXISTS(SELECT * FROM #dummy_entityMappings)
			ORDER BY [Id] ASC;
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] OFF;
		
			UPDATE [tmp_ms_xx_SS_MAP_EntityMapping] SET [EntityType] = @attachmentEntityTypeId WHERE [EntityName] = 'Attachment'
			UPDATE [tmp_ms_xx_SS_MAP_EntityMapping] SET [EntityType] = @htmlWidgetEntityTypeId WHERE [EntityName] = 'HtmlWidgets'
		END
		
		DROP TABLE #dummy_entityMappings
		
		ALTER TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] ALTER COLUMN [EntityType] INT NOT NULL
		ALTER TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityMapping] DROP COLUMN [EntityName];

		DROP TABLE [dbo].[SS_MAP_EntityMapping];
		
		EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SS_MAP_EntityMapping]', N'SS_MAP_EntityMapping';
	
	COMMIT TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END
	
IF(NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_MAP_EntityMapping]') AND type in (N'U')))
BEGIN
	CREATE TABLE [dbo].[SS_MAP_EntityMapping] (
		[Id]             INT IDENTITY (1, 1) NOT NULL,
		[EntityType]     INT NOT NULL,
		[EntityId]       INT NOT NULL,
		[MappedEntityId] INT NOT NULL,
		[DisplayOrder]   INT NOT NULL,
		[MappingType]    INT NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);
END
   
/* =============== Create/Update the entity widget mapping table =============== */  
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_MAP_EntityWidgetMapping]') AND type in (N'U')) 
	AND NOT EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id(N'[dbo].[SS_MAP_EntityWidgetMapping]') and NAME='EntityType'))	
BEGIN
	
	BEGIN TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;

		CREATE TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] (
			[Id]           INT            IDENTITY (1, 1) NOT NULL,
			[EntityType]   INT            NULL,
			[EntityId]     INT            NOT NULL,
			[WidgetZone]   NVARCHAR (MAX) NULL,
			[DisplayOrder] INT            NOT NULL,
			[EntityName]   NVARCHAR (MAX) NOT NULL,
			PRIMARY KEY CLUSTERED ([Id] ASC)
		);
		
		create table #dummy_entityWidgetMappings
		(i int)
		
		IF EXISTS (SELECT NULL FROM   [dbo].[SS_MAP_EntityWidgetMapping])
		BEGIN
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] ON;
			INSERT INTO [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] ([Id], [EntityId], [WidgetZone], [DisplayOrder], [EntityName])
			SELECT   [Id],
					 [EntityId],
					 [WidgetZone],
					 [DisplayOrder],
					 [EntityName]
			FROM     [dbo].[SS_MAP_EntityWidgetMapping]
			WHERE NOT EXISTS(SELECT * FROM #dummy_entityWidgetMappings)
			ORDER BY [Id] ASC;
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] OFF;
		
			UPDATE [tmp_ms_xx_SS_MAP_EntityWidgetMapping] SET [EntityType] = @attachmentEntityTypeId WHERE [EntityName] = 'Attachment'
			UPDATE [tmp_ms_xx_SS_MAP_EntityWidgetMapping] SET [EntityType] = @htmlWidgetEntityTypeId WHERE [EntityName] = 'HtmlWidgets'
		END
		
		DROP TABLE #dummy_entityWidgetMappings
		
		ALTER TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] ALTER COLUMN [EntityType] INT NOT NULL
		ALTER TABLE [dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping] DROP COLUMN [EntityName];

		DROP TABLE [dbo].[SS_MAP_EntityWidgetMapping];
		
		EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SS_MAP_EntityWidgetMapping]', N'SS_MAP_EntityWidgetMapping';
	
	COMMIT TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
	
END
	
IF(NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_MAP_EntityWidgetMapping]') AND type in (N'U')))
BEGIN
	CREATE TABLE [dbo].[SS_MAP_EntityWidgetMapping] (
		[Id]           INT            IDENTITY (1, 1) NOT NULL,
		[EntityType]   INT            NOT NULL,
		[EntityId]     INT            NOT NULL,
		[WidgetZone]   NVARCHAR (MAX) NULL,
		[DisplayOrder] INT            NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);
END
   
/* =============== Create the EntityCondition table =============== */
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_C_EntityCondition]') AND type in (N'U')) 
	AND NOT EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id(N'[dbo].[SS_C_EntityCondition]') and NAME='EntityType'))
		
BEGIN
	
	BEGIN TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;

		CREATE TABLE [dbo].[tmp_ms_xx_SS_C_EntityCondition] (
			[Id]              INT IDENTITY (1, 1) NOT NULL,
			[ConditionId]     INT NOT NULL,
			[EntityType]      INT NULL,
			[EntityId]        INT NOT NULL,
			[LimitedToStores] BIT NOT NULL,
			[EntityName]   NVARCHAR (MAX) NOT NULL,
			PRIMARY KEY CLUSTERED ([Id] ASC)
		);
		
		create table #dummy_entityCondition
		(i int)
		
		IF EXISTS (SELECT NULL FROM   [dbo].[SS_C_EntityCondition])
		BEGIN
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_C_EntityCondition] ON;
			INSERT INTO [dbo].[tmp_ms_xx_SS_C_EntityCondition] ([Id], [ConditionId], [EntityId], [LimitedToStores], [EntityName])
			SELECT   [Id],
					 [ConditionId],
					 [EntityId],
					 [LimitedToStores],
					 [EntityName]
			FROM     [dbo].[SS_C_EntityCondition]
			WHERE NOT EXISTS(SELECT * FROM #dummy_entityCondition)
			ORDER BY [Id] ASC;
			SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SS_C_EntityCondition] OFF;
		
			UPDATE [tmp_ms_xx_SS_C_EntityCondition] SET [EntityType] = @attachmentEntityTypeId WHERE [EntityName] = 'Attachment'

			UPDATE [tmp_ms_xx_SS_C_EntityCondition] SET [EntityType] = @htmlWidgetEntityTypeId WHERE [EntityName] = 'HtmlWidgets'
		END
		
		DROP TABLE #dummy_entityCondition
		
		ALTER TABLE [dbo].[tmp_ms_xx_SS_C_EntityCondition] ALTER COLUMN [EntityType] INT NOT NULL
		ALTER TABLE [dbo].[tmp_ms_xx_SS_C_EntityCondition] DROP COLUMN [EntityName]

		DROP TABLE [dbo].[SS_C_EntityCondition];
		
		EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SS_C_EntityCondition]', N'SS_C_EntityCondition';
		
		ALTER TABLE [dbo].[SS_C_EntityCondition]  WITH CHECK ADD CONSTRAINT [EntityCondition_ConditionEntity] FOREIGN KEY([ConditionId])
		REFERENCES [dbo].[SS_C_Condition] ([Id])
		ON DELETE CASCADE

		ALTER TABLE [dbo].[SS_C_EntityCondition] CHECK CONSTRAINT [EntityCondition_ConditionEntity]
	
	COMMIT TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END
	
IF(NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_C_EntityCondition]') AND type in (N'U')))
BEGIN
	CREATE TABLE [dbo].[SS_C_Condition] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NULL,
		[Active] [bit] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
	)

	CREATE TABLE [dbo].[SS_C_ConditionGroup] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ConditionId] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
	) 

	ALTER TABLE [dbo].[SS_C_ConditionGroup] WITH CHECK ADD CONSTRAINT [ConditionGroup_ConditionEntity] FOREIGN KEY([ConditionId])
	REFERENCES [dbo].[SS_C_Condition] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SS_C_ConditionGroup] CHECK CONSTRAINT [ConditionGroup_ConditionEntity]
	
	CREATE TABLE [dbo].[SS_C_ConditionStatement](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ConditionType] [int] NOT NULL,
		[ConditionProperty] [int] NOT NULL,
		[OperatorType] [int] NOT NULL,
		[Value] [nvarchar](max) NULL,
		[ConditionGroupId] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
	)

	ALTER TABLE [dbo].[SS_C_ConditionStatement]  WITH CHECK ADD  CONSTRAINT [ConditionStatement_ConditionGroupEntity] FOREIGN KEY([ConditionGroupId])
	REFERENCES [dbo].[SS_C_ConditionGroup] ([Id])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[SS_C_ConditionStatement] CHECK CONSTRAINT [ConditionStatement_ConditionGroupEntity]
	
	CREATE TABLE [dbo].[SS_C_CustomerOverride](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ConditionId] [int] NOT NULL,
		[CustomerId] [int] NOT NULL,
		[OverrideState] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
	)

	ALTER TABLE [dbo].[SS_C_CustomerOverride]  WITH CHECK ADD  CONSTRAINT [CustomerOverride_ConditionEntity] FOREIGN KEY([ConditionId])
	REFERENCES [dbo].[SS_C_Condition] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SS_C_CustomerOverride] CHECK CONSTRAINT [CustomerOverride_ConditionEntity]
	
	CREATE TABLE [dbo].[SS_C_ProductOverride](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ConditionId] [int] NOT NULL,
		[ProductId] [int] NOT NULL,
		[ProductState] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
	)

	ALTER TABLE [dbo].[SS_C_ProductOverride]  WITH CHECK ADD  CONSTRAINT [ProductOverride_ConditionEntity] FOREIGN KEY([ConditionId])
	REFERENCES [dbo].[SS_C_Condition] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SS_C_ProductOverride] CHECK CONSTRAINT [ProductOverride_ConditionEntity]

	CREATE TABLE [dbo].[SS_C_EntityCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ConditionId] [int] NOT NULL,
		[EntityType] [int] NOT NULL,
		[EntityId] [int] NOT NULL,
		[LimitedToStores] [bit] NOT NULL,
		PRIMARY KEY CLUSTERED 
		([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
	)

	ALTER TABLE [dbo].[SS_C_EntityCondition]  WITH CHECK ADD  CONSTRAINT [EntityCondition_ConditionEntity] FOREIGN KEY([ConditionId])
	REFERENCES [dbo].[SS_C_Condition] ([Id])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[SS_C_EntityCondition] CHECK CONSTRAINT [EntityCondition_ConditionEntity]
			
END

IF(NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_PR_ProductRibbon]') AND type in (N'U')) 
	OR NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_PR_ProductOverride]') AND type in (N'U')) 
	OR NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_PR_Condition]') AND type in (N'U')) 
	OR NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_PR_ConditionGroup]') AND type in (N'U')) 
	OR NOT EXISTS (SELECT NULL FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SS_PR_ConditionStatement]') AND type in (N'U')))
BEGIN
	PRINT 'Product Ribbons'
END
ELSE 
BEGIN

	BEGIN TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;

		DECLARE @productRibbonEntityTypeId INT
		SET @productRibbonEntityTypeId = 30

		DECLARE @productConditionType INT
		SET @productConditionType = 3

		DECLARE @productRibbonsDefaultConditionType INT
		SET @productRibbonsDefaultConditionType = 0

		DECLARE @CurrentOldConditionId INT
		DECLARE @CurrentNewConditionId INT
		DECLARE @CurrentConditionRow INT
		DECLARE @TotalConditionRecords INT

		DECLARE @CurrentProductRibbonId INT

		DECLARE @CurrentOldGroupId INT
		DECLARE @CurrentNewGroupId INT
		DECLARE @CurrentGroupRow INT
		DECLARE @TotalGroupRecords INT

		SET @CurrentConditionRow = 1
		
		EXEC sp_executesql N'SELECT @TotalConditionRecords = COUNT(*) FROM SS_PR_Condition
		WHERE EXISTS (SELECT NULL FROM SS_PR_ProductRibbon WHERE ConditionId = SS_PR_Condition.Id)',
		N'@TotalConditionRecords INT OUTPUT', @TotalConditionRecords OUTPUT


		CREATE TABLE #ConditionIteratorTable
		(
			RowNum INT NOT NULL,
			Id INT NOT NULL
		)

		EXEC ('INSERT INTO #ConditionIteratorTable
		SELECT ROW_NUMBER() OVER(ORDER BY Id), Id
		FROM SS_PR_Condition
		WHERE EXISTS (SELECT NULL FROM SS_PR_ProductRibbon WHERE ConditionId = SS_PR_Condition.Id)')

		WHILE @CurrentConditionRow <= @TotalConditionRecords
		BEGIN

			SELECT Top 1 @CurrentOldConditionId = Id FROM #ConditionIteratorTable

			EXEC sp_executesql 
			N'SELECT @CurrentProductRibbonId = Id FROM SS_PR_ProductRibbon WHERE ConditionId = @CurrentOldConditionId',
			N'@CurrentProductRibbonId INT OUTPUT, @CurrentOldConditionId INT', @CurrentProductRibbonId OUTPUT, @CurrentOldConditionId			

			--Insert the new condition
			INSERT INTO SS_C_Condition
			SELECT '', 1

			SET @CurrentNewConditionId = SCOPE_IDENTITY()

			--Insert the new entity condition mapping
			INSERT INTO SS_C_EntityCondition
			SELECT @CurrentNewConditionId, @productRibbonEntityTypeId, @CurrentProductRibbonId, 0

			--Move all the product overrides for the current condition
			INSERT INTO SS_C_ProductOverride
			SELECT @CurrentNewConditionId, [pr].ProductId, [pr].ProductState 
			FROM SS_PR_ProductOverride [pr]
			WHERE [pr].ConditionId = @CurrentOldConditionId AND
			NOT EXISTS (SELECT NULL FROM SS_C_ProductOverride [po]
						WHERE [po].ConditionId = @CurrentOldConditionId AND
							  [po].ProductId = [pr].ProductId)

			--Move all the groups for the current condition

			SET @CurrentGroupRow = 1
			SELECT @TotalGroupRecords = COUNT(*) FROM SS_PR_ConditionGroup WHERE ConditionId = @CurrentOldConditionId

			
			CREATE TABLE #GroupIteratorTable(
				Id int,
				RowNum int,
				ConditionId int
			)

			INSERT INTO #GroupIteratorTable
			SELECT RowNum = ROW_NUMBER() OVER(ORDER BY Id), *
			FROM SS_PR_ConditionGroup WHERE ConditionId = @CurrentOldConditionId
		
			WHILE @CurrentGroupRow <= @TotalGroupRecords
			BEGIN
			
				SELECT Top 1 @CurrentOldGroupId = Id FROM #GroupIteratorTable

				--Insert the new group
				INSERT INTO SS_C_ConditionGroup
				SELECT @CurrentNewConditionId

				SET @CurrentNewGroupId = SCOPE_IDENTITY()

				--Insert the conditions statements
				INSERT INTO SS_C_ConditionStatement
				SELECT CASE WHEN [Type] != @productRibbonsDefaultConditionType THEN @productConditionType ELSE @productRibbonsDefaultConditionType END AS ConditionType,
				[Type], 
				CASE WHEN OperatorType> 1 THEN OperatorType + 2 ELSE OperatorType END AS OperatorType, Value, @CurrentNewGroupId FROM
				SS_PR_ConditionStatement WHERE ConditionGroupId = @CurrentOldGroupId

				DELETE FROM #GroupIteratorTable WHERE RowNum = @CurrentGroupRow
				SET @CurrentGroupRow = @CurrentGroupRow + 1
			END

			DROP TABLE #GroupIteratorTable

			DELETE FROM #ConditionIteratorTable WHERE RowNum = @CurrentConditionRow
			SET @CurrentConditionRow = @CurrentConditionRow + 1
		END

		DROP TABLE #ConditionIteratorTable

		ALTER TABLE [SS_PR_ProductRibbon] DROP CONSTRAINT [ProductRibbon_Condition]
		ALTER TABLE SS_PR_ProductRibbon DROP COLUMN ConditionId

		DROP TABLE SS_PR_ConditionStatement
		DROP TABLE SS_PR_ConditionGroup
		DROP TABLE SS_PR_ProductOverride
		DROP TABLE SS_PR_Condition
			
	COMMIT TRANSACTION;	
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
END

BEGIN TRY
    DROP FUNCTION dbo.GetMappingsSQL;
END TRY
BEGIN CATCH
    --PRINT 'Function did not exist.';
END CATCH
GO

CREATE FUNCTION dbo.GetMappingsSQL(@Type NVARCHAR(MAX), @TypeName NVARCHAR(MAX), @EntityName NVARCHAR(MAX), @MappingTable NVARCHAR(MAX), @EntityTable NVARCHAR(MAX))
RETURNS nvarchar(MAX)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = 'SELECT [t].[Id] AS EntityId, [cm].' + @TypeName + ' AS MappedEntityId,' + @Type + ' AS MappedEntityTypeId FROM ' + @EntityTable + ' [t]'
	SET @sql = @sql + ' JOIN ' + @MappingTable + ' [cm] ON [cm].' + @EntityName + ' = [t].Id'
	RETURN(@sql)
END
GO

--DELETE FROM SS_C_Condition
--DELETE FROM SS_C_ConditionGroup
--DELETE FROM SS_C_ConditionStatement
--DELETE FROM SS_C_EntityCondition
--drop table #TabsWithMappings
--EXEC('ALTER TABLE SS_QT_Tab DROP COLUMN TabMode')

-- Store procedure
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CreateConditionGroup')
	DROP PROCEDURE sp_CreateConditionGroup
GO

GO
CREATE PROCEDURE sp_CreateConditionGroup
	@entityId int,  -- tab id, slider id etc.
	@entityType int,  -- tab, slider etc.
	@conditionType int, -- Product condition, Customer condition etc.
	@conditionPropertyType int,  -- category or manufacturer
	@conditionValue nvarchar(10), -- actual category id or manufacturer id
	@hasProductMappingTable bit = 0 -- if has SS_QT_Product_Tab_Mapping etc.
AS
BEGIN

	DECLARE @currentConditionId INT
	SET @currentConditionId = (SELECT [ConditionId] FROM SS_C_EntityCondition WHERE (EntityId = @entityId AND EntityType = @entityType))

	IF @currentConditionId IS NULL
	BEGIN
	    -- 1. Create the condition
	    INSERT INTO [dbo].[SS_C_Condition] (Active)
	    VALUES (1)
	    SET @currentConditionId = @@IDENTITY

	   -- 2. Create the entity-condition mapping
		INSERT INTO [dbo].[SS_C_EntityCondition] (ConditionId, EntityId, EntityType, LimitedToStores)
		VALUES (@currentConditionId, @entityId, @entityType, 0)

		--3.Create the default condition group
		INSERT INTO [dbo].[SS_C_ConditionGroup] (ConditionId)
		VALUES (@currentConditionId)

		DECLARE @currentDefaultConditionGroupId INT
		SET @currentDefaultConditionGroupId = @@IDENTITY

		--4.Create the default condition statement
		INSERT INTO [dbo].[SS_C_ConditionStatement] (ConditionGroupId, ConditionProperty, ConditionType, OperatorType, Value)
		VALUES (@currentDefaultConditionGroupId, 0, 0, 0, 'Fail')
	END

	-- create new condition group
	INSERT INTO [dbo].[SS_C_ConditionGroup] (ConditionId)
	VALUES (@currentConditionId)

	DECLARE @currentConditionGroupId INT
	SET @currentConditionGroupId = @@IDENTITY

	--4.Create the specific condition statement
	INSERT INTO [dbo].[SS_C_ConditionStatement] (ConditionGroupId, ConditionProperty, ConditionType, OperatorType, Value)
	VALUES (@currentConditionGroupId, @conditionPropertyType, @conditionType, 0, @conditionValue)

	-- Create Product Overrides if entity has product mappings
	IF @hasProductMappingTable = 1 AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_QT_Product_Tab_Mapping]'))
	BEGIN
		IF EXISTS (SELECT NULL FROM SS_QT_Product_Tab_Mapping [ptm] WHERE [ptm].TabId = @entityId)
		BEGIN
			DECLARE @productMappingsCounter INT
			SET @productMappingsCounter = 1

			DECLARE @productOverridesCount INT
			SET @productOverridesCount = (SELECT COUNT(Id) FROM SS_QT_Product_Tab_Mapping [ptm] WHERE [ptm].TabId = @entityId)

			DECLARE @ProductOverridesForTab TABLE 
			(
				Id int IDENTITY(1,1) PRIMARY KEY,
				ProductId int
			)

			INSERT INTO @ProductOverridesForTab (ProductId)
			SELECT [ptm].ProductId FROM SS_QT_Product_Tab_Mapping [ptm]
			WHERE [ptm].TabId = @entityId

			WHILE @productMappingsCounter <= @productOverridesCount
			BEGIN
				DECLARE @productIdForOverride INT
				SET @productIdForOverride = (SELECT ProductId FROM @ProductOverridesForTab WHERE Id = @productMappingsCounter)

				INSERT INTO SS_C_ProductOverride (ConditionId, ProductId, ProductState)
				SELECT @currentConditionId, @productIdForOverride, 0
				WHERE NOT EXISTS (SELECT NULL FROM SS_C_ProductOverride [po]
								  WHERE [po].ConditionId = @currentConditionId AND
								        [po].ProductId = @productIdForOverride)

				SET @productMappingsCounter = @productMappingsCounter + 1
			END
		END
	END
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CreatePassOnlyConditionGroup')
	DROP PROCEDURE sp_CreatePassOnlyConditionGroup
GO

GO
CREATE PROCEDURE  sp_CreatePassOnlyConditionGroup
	@entityId int,  -- tab id, slider id etc.
	@entityType int  -- tab, slider etc.	
AS
BEGIN
	DECLARE @currentConditionId INT

	-- 1. Create the condition
	INSERT INTO [dbo].[SS_C_Condition] (Active)
	VALUES (1)
	SET @currentConditionId = @@IDENTITY

	-- 2. Create the entity-condition mapping
	INSERT INTO [dbo].[SS_C_EntityCondition] (ConditionId, EntityId, EntityType, LimitedToStores)
	VALUES (@currentConditionId, @entityId, @entityType, 0)

	--3.Create the default condition group
	INSERT INTO [dbo].[SS_C_ConditionGroup] (ConditionId)
	VALUES (@currentConditionId)

	DECLARE @currentDefaultConditionGroupId INT
	SET @currentDefaultConditionGroupId = @@IDENTITY

	--4.Create the default condition statement
	INSERT INTO [dbo].[SS_C_ConditionStatement] (ConditionGroupId, ConditionProperty, ConditionType, OperatorType, Value)
	VALUES (@currentDefaultConditionGroupId, 0, 0, 0, 'Pass')
END
	
GO

-- Get the data
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_TransferMappingsToConditions')
	DROP PROCEDURE sp_TransferMappingsToConditions
GO

CREATE PROCEDURE sp_TransferMappingsToConditions
	@entityType int,  -- tab, slider etc.
	@conditionType int, -- Product condition, Customer condition etc.
	@type nvarchar(MAX), -- Number coresponding to the MappedEntityTypeId - category, manufacturer etc.
	@mappedEntityColumnName nvarchar(MAX), -- CategoryId, ManufacturerId etc.
	@entityColumnName nvarchar(MAX), -- TabId, SliderId etc.
	@mappingTableName nvarchar(MAX), -- SS_QT_Category_Tab_Mapping, SS_QT_Manufacturer_Tab_Mapping etc.
	@entityTableName nvarchar(MAX) -- SS_QT_Tab, SS_AS_AnywhereSlider etc.
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = (SELECT dbo.GetMappingsSQL(@type, @mappedEntityColumnName, @entityColumnName, @mappingTableName, @entityTableName))
	--(SELECT dbo.GetSQL('1', 'Category', 'Tab', 'SS_QT_Category_Tab_Mapping', 'SS_QT_Tab'))

	-- Put the data into table variable
	DECLARE @Mappings TABLE 
	(
		Id int IDENTITY(1,1) PRIMARY KEY,
		EntityId int,
		MappedEntityId int,
		MappedEntityTypeId int
	)

	INSERT INTO @Mappings
	EXEC sp_executesql @sql	

	-- Loop

	-- get the number of mappings
	DECLARE @MaxRownum INT
	SET @MaxRownum = (SELECT COUNT(*) FROM @Mappings)

	DECLARE @Iter INT
	SET @Iter = 1

	WHILE @Iter <= @MaxRownum
	BEGIN
		-- call store procedure
		DECLARE @entityId INT
		SET @entityId = (SELECT [EntityId] FROM @Mappings WHERE Id = @Iter)

		DECLARE @conditionPropertyType INT
		SET @conditionPropertyType = (SELECT [MappedEntityTypeId] FROM @Mappings WHERE Id = @Iter) -- Category

		DECLARE @conditionValue INT
		SET @conditionValue = (SELECT [MappedEntityId] FROM @Mappings WHERE Id = @Iter)

		EXEC sp_CreateConditionGroup @entityId, @entityType, @conditionType, @conditionPropertyType, @conditionValue, 1

		SET @Iter = @Iter + 1
	END
END
GO

--** Update Product Tabs **--
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_QT_Tab]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_QT_Category_Tab_Mapping]') AND type in (N'U')))
BEGIN

	BEGIN TRANSACTION;		
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;		
	SET XACT_ABORT ON;
		
		EXEC('ALTER TABLE SS_QT_Tab ADD TabMode int NULL')
		EXEC('UPDATE SS_QT_Tab SET TabMode = 5')

		-- get all tabs with mappings that are not global
		create table #TabsWithMappings
		(
			 Id int
		)

		INSERT INTO #TabsWithMappings (Id)
		SELECT DISTINCT [t].Id FROM SS_QT_Tab [t]
		LEFT JOIN SS_QT_Manufacturer_Tab_Mapping [tmm] ON [t].Id = [tmm].TabId
		LEFT JOIN SS_QT_Category_Tab_Mapping [tcm] ON [t].Id = [tcm].TabId
		WHERE ([t].Id = [tmm].TabId OR [t].Id = [tcm].TabId) AND [t].IsGlobal = 0

		-- insert the mappings for tabs with only product mappings
		DECLARE @tabEntityTypeId INT
		SET @tabEntityTypeId = 10

		DECLARE @productMappingTypeId INT
		SET @productMappingTypeId = 2

		CREATE TABLE #dummy_T
		(i int)

		INSERT INTO SS_MAP_EntityMapping
		SELECT @tabEntityTypeId, [pm].[TabId], [pm].[ProductId], 0, @productMappingTypeId FROM SS_QT_Product_Tab_Mapping [pm]
		WHERE NOT EXISTS(SELECT * FROM #dummy_T) AND EXISTS 
		(SELECT NULL FROM 
			(SELECT * FROM SS_QT_Tab [t]
			 WHERE NOT EXISTS (SELECT * FROM #TabsWithMappings [twm] 
							   WHERE [t].Id = [twm].Id))[qt]
		WHERE [pm].TabId = [qt].Id)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @tabEntityTypeId AND 
		      EntityId = [pm].[TabId] AND 
			  MappedEntityId = [pm].[ProductId] AND 
			  MappingType = @productMappingTypeId)

		-- global tabs
		-- set TabMode to conditions
		EXEC('UPDATE qt 
		SET TabMode = 10
		FROM SS_QT_Tab As qt
		JOIN #TabsWithMappings [twm] ON qt.Id = [twm].Id')

		EXEC('UPDATE SS_QT_Tab
		SET TabMode = 10
		Where IsGlobal = 1')

		DROP TABLE #dummy_T

		-- Create the pass condition for all global tabs
		
		DECLARE @globalTabsCount INT
		EXEC sp_executesql N'SET @globalTabsCount = (SELECT COUNT(Id) FROM SS_QT_Tab WHERE IsGlobal = 1)', 
					N'@globalTabsCount INT OUTPUT', @globalTabsCount OUTPUT

		DECLARE @globalTabsCounter INT
		SET @globalTabsCounter = 1

		CREATE TABLE #GlobalTabs
		(
			Id int IDENTITY(1,1) PRIMARY KEY,
			EntityId int
		)

		EXEC('INSERT INTO #GlobalTabs (EntityId)	SELECT Id FROM SS_QT_Tab [qt] WHERE [qt].IsGlobal = 1')
		--EXEC sp_executesql N'INSERT INTO @GlobalTabs (EntityId)	SELECT Id FROM SS_QT_Tab [qt] WHERE [qt].IsGlobal = 1', 
		--		   N'@GlobalTabs TABLE OUTPUT', #GlobalTabs OUTPUT

		WHILE @globalTabsCounter <= @globalTabsCount
		BEGIN
			DECLARE @entityId INT
			SET @entityId = (SELECT Id FROM #GlobalTabs WHERE Id = @globalTabsCounter)

			EXEC sp_CreatePassOnlyConditionGroup @entityId, 10

			SET @globalTabsCounter = @globalTabsCounter + 1
		END

		-- not global		
		EXEC sp_TransferMappingsToConditions 
			10, -- tab
			3, -- product condition
			'1', -- category property
			'CategoryId', 
			'TabId', 
			'SS_QT_Category_Tab_Mapping',
			'#TabsWithMappings'

		EXEC sp_TransferMappingsToConditions 
			10, -- tab
			3, -- product condition
			'2', -- manufacturer property
			'ManufacturerId', 
			'TabId', 
			'SS_QT_Manufacturer_Tab_Mapping',
			'#TabsWithMappings'

		DROP TABLE SS_QT_Product_Tab_Mapping;
		DROP TABLE SS_QT_Category_Tab_Mapping;
		DROP TABLE SS_QT_Manufacturer_Tab_Mapping;

		ALTER TABLE SS_QT_Tab ALTER COLUMN TabMode INT NOT NULL

		-- drop IsGlobal	
		EXEC('ALTER TABLE SS_QT_Tab DROP COLUMN IsGlobal')
		DROP TABLE #TabsWithMappings
		DROP TABLE #GlobalTabs

		PRINT('Product Tabs updated')

	COMMIT TRANSACTION;			
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
END


------------------------ Sliders -------------------
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_AS_AnywhereSlider]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id(N'[dbo].[SS_AS_AnywhereSlider]') AND NAME='FromDate') 
	AND EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id(N'[dbo].[SS_AS_AnywhereSlider]') AND NAME='ToDate') 
	AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_AS_SliderCategory]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_AS_SliderManufacturer]') AND type in (N'U')))
BEGIN
		
	DECLARE @anywhereSlidersEntityTypeId INT
	SET @anywhereSlidersEntityTypeId = 15

	BEGIN TRANSACTION;		
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;		
	SET XACT_ABORT ON;

		ALTER TABLE SS_AS_SliderImage
		ALTER COLUMN Url nvarchar(MAX) NULL

		/* SCHEDULING */

		DECLARE @FromDate_S DATETIME
		DECLARE @ToDate_S DATETIME
		DECLARE @FromTime_S TIME
		DECLARE @ToTime_S TIME

		DECLARE @Id_S INT
		DECLARE @TempFromDate_S DATETIME
		DECLARE @TempToDate_S DATETIME
		DECLARE @Iterator_S INT
		DECLARE @TotalRecords_S INT
		
		SET @Iterator_S = 1
		SET @TotalRecords_S = (SELECT COUNT(*) FROM SS_AS_AnywhereSlider)

		CREATE TABLE #TempTable_S(
			RowNum int,
			Id int,
			SystemName nvarchar(400), 
			SliderType int,
			FromDate DateTime,
			ToDate DateTime,
			LanguageId int,
			LimitedToStores bit
		)

		INSERT INTO #TempTable_S
		SELECT RowNum = ROW_NUMBER() OVER(ORDER BY Id), *
		FROM SS_AS_AnywhereSlider

		WHILE (@Iterator_S <= @TotalRecords_S)
		BEGIN
			SELECT TOP 1 @Id_S = Id, @TempFromDate_S = FromDate, @TempToDate_S = ToDate FROM #TempTable_S
			
			IF( @TempFromDate_S IS NOT NULL )
			BEGIN
				SET @FromDate_S = cast(@TempFromDate_S AS DATE)
				SET @FromTime_S = cast(@TempFromDate_S AS TIME)					
			END
			ELSE
			BEGIN
				SET @FromDate_S = NULL
				SET @FromTime_S = '00:00:00'
			END
				
			IF( @TempToDate_S IS NOT NULL )
			BEGIN				
				SET @ToDate_S = cast(@TempToDate_S AS DATE)
				SET @ToTime_S = cast(@TempToDate_S AS TIME)					
			END
			ELSE
			BEGIN
				SET @ToDate_S = NULL
				SET @ToTime_S = '00:00:00'
			END
				
			INSERT INTO SS_S_Schedule(EntityType, EntityId, EntityFromDate, EntityToDate, SchedulePatternType, SchedulePatternFromTime, SchedulePatternToTime )
			VALUES(@anywhereSlidersEntityTypeId, @Id_S, @FromDate_S, @ToDate_S, 5, @FromTime_S, @ToTime_S)

			DELETE FROM #TempTable_S WHERE RowNum = @Iterator_S
			
			SET @Iterator_S = @Iterator_S + 1
		END

		DROP TABLE #TempTable_S;
	
		/* CONDITIONS */
		
		CREATE TABLE #SlidersWithoutMappings
		(
			Id int IDENTITY(1,1) PRIMARY KEY,
			EntityId int
		)

		INSERT INTO #SlidersWithoutMappings(EntityId)
		SELECT DISTINCT [t].Id FROM SS_AS_AnywhereSlider [t]
		WHERE NOT EXISTS (SELECT NULL FROM SS_AS_SliderManufacturer WHERE SS_AS_SliderManufacturer.SliderId = [t].Id)
		AND NOT EXISTS (SELECT NULL FROM SS_AS_SliderCategory WHERE SS_AS_SliderCategory.SliderId = [t].Id)
		
		DECLARE @globalSlidersCount INT
		SET @globalSlidersCount = (SELECT COUNT(Id) FROM #SlidersWithoutMappings)

		DECLARE @globalSlidersCounter INT
		SET @globalSlidersCounter = 1

		WHILE @globalSlidersCounter <= @globalSlidersCount
		BEGIN
			DECLARE @sliderId INT
			SET @sliderId = (SELECT EntityId FROM #SlidersWithoutMappings WHERE Id = @globalSlidersCounter)

			EXEC sp_CreatePassOnlyConditionGroup @sliderId, 15

			SET @globalSlidersCounter = @globalSlidersCounter + 1
		END

		DROP TABLE #SlidersWithoutMappings

		EXEC sp_TransferMappingsToConditions
			15, -- entity type
			0, -- condition type
			'1', -- name property
			'CategoryId',
			'Id',
			'SS_AS_SliderCategory',
			'SS_AS_AnywhereSlider'

		EXEC sp_TransferMappingsToConditions
			15, -- entity type
			1, -- manufacturer type
			'2', -- name property
			'ManufacturerId',
			'Id',
			'SS_AS_SliderManufacturer',
			'SS_AS_AnywhereSlider'

		/* WIDGET MAPPINGS */

		INSERT INTO SS_MAP_EntityWidgetMapping
		SELECT @anywhereSlidersEntityTypeId, SliderId, WidgetZone, DisplayOrder FROM SS_AS_SliderWidget
		WHERE EXISTS (SELECT NULL FROM SS_AS_AnywhereSlider WHERE SS_AS_AnywhereSlider.Id = SS_AS_SliderWidget.SliderId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityWidgetMapping entitywidgetmapping
		WHERE entitywidgetmapping.EntityType = @anywhereSlidersEntityTypeId AND entitywidgetmapping.EntityId = SliderId 
		AND entitywidgetmapping.WidgetZone = WidgetZone AND entitywidgetmapping.DisplayOrder = DisplayOrder)
		
		ALTER TABLE SS_AS_AnywhereSlider DROP COLUMN [FromDate], COLUMN [ToDate];
		DROP TABLE SS_AS_SliderCategory;
		DROP TABLE SS_AS_SliderManufacturer;
		DROP TABLE SS_AS_SliderWidget;
			
	COMMIT TRANSACTION;			
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END

-------------- Carousel --------------------------------------------
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_JC_JCarousel]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_JC_JCarouselCategory]') AND type in (N'U')))
BEGIN

	BEGIN TRANSACTION;		
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;		
	SET XACT_ABORT ON;

		DECLARE @jCarouselEntityTypeId INT
	    SET @jCarouselEntityTypeId = 35

		DECLARE @categoyMappingTypeId_JC INT
		SET @categoyMappingTypeId_JC = 0

		DECLARE @manufacturerMappingTypeId_JC INT
		SET @manufacturerMappingTypeId_JC = 1

		DECLARE @productMappingTypeId_JC INT
		SET @productMappingTypeId_JC = 2

		INSERT INTO SS_MAP_EntityMapping
		SELECT @jCarouselEntityTypeId, CarouselId, CategoryId, DisplayOrder, @categoyMappingTypeId_JC FROM SS_JC_JCarouselCategory
		WHERE EXISTS (SELECT NULL FROM SS_JC_JCarousel WHERE SS_JC_JCarousel.Id = SS_JC_JCarouselCategory.CarouselId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @jCarouselEntityTypeId AND EntityId = CarouselId AND MappedEntityId = CategoryId AND MappingType = @categoyMappingTypeId_JC)

		INSERT INTO SS_MAP_EntityMapping
		SELECT @jCarouselEntityTypeId, CarouselId, ManufacturerId, DisplayOrder, @manufacturerMappingTypeId_JC FROM SS_JC_JCarouselManufacturer
		WHERE EXISTS (SELECT NULL FROM SS_JC_JCarousel WHERE SS_JC_JCarousel.Id = SS_JC_JCarouselManufacturer.CarouselId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @jCarouselEntityTypeId AND EntityId = CarouselId AND MappedEntityId = ManufacturerId AND MappingType = @manufacturerMappingTypeId_JC)

		INSERT INTO SS_MAP_EntityMapping
		SELECT @jCarouselEntityTypeId, CarouselId, ProductId, DisplayOrder, @productMappingTypeId_JC FROM SS_JC_JCarouselProduct
		WHERE EXISTS (SELECT NULL FROM SS_JC_JCarousel WHERE SS_JC_JCarousel.Id = SS_JC_JCarouselProduct.CarouselId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @jCarouselEntityTypeId AND EntityId = CarouselId AND MappedEntityId = ProductId AND MappingType = @productMappingTypeId_JC)

		INSERT INTO SS_MAP_EntityWidgetMapping
		SELECT @jCarouselEntityTypeId, CarouselId, WidgetZone, DisplayOrder FROM SS_JC_JCarouselWidget
		WHERE EXISTS (SELECT NULL FROM SS_JC_JCarousel WHERE SS_JC_JCarousel.Id = SS_JC_JCarouselWidget.CarouselId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityWidgetMapping entitywidgetmapping
		WHERE entitywidgetmapping.EntityType = @jCarouselEntityTypeId AND entitywidgetmapping.EntityId = CarouselId AND entitywidgetmapping.WidgetZone = WidgetZone)

		DROP TABLE SS_JC_JCarouselCategory;
		DROP TABLE SS_JC_JCarouselManufacturer;
		DROP TABLE SS_JC_JCarouselProduct;
		DROP TABLE SS_JC_JCarouselWidget;

	COMMIT TRANSACTION;			
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END

/* =============== SMART SEO UPGRADE =============== */
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_SS_SeoTemplate]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_SS_SEOCategory]') AND type in (N'U')))
BEGIN

	BEGIN TRANSACTION;		
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;		
	SET XACT_ABORT ON;

		DECLARE @smartSEOEntityTypeId INT
		SET @smartSEOEntityTypeId = 20	

		DECLARE @categoyMappingTypeId_SS INT
		SET @categoyMappingTypeId_SS = 0

		DECLARE @manufacturerMappingTypeId_SS INT
		SET @manufacturerMappingTypeId_SS = 1

		DECLARE @productMappingTypeId_SS INT
		SET @productMappingTypeId_SS = 2

		INSERT INTO SS_MAP_EntityMapping
		SELECT @smartSEOEntityTypeId, SeoTemplateId, CategoryId, 0, @categoyMappingTypeId_SS FROM SS_SS_SEOCategory
		WHERE EXISTS (SELECT NULL FROM SS_SS_SeoTemplate WHERE SS_SS_SeoTemplate.Id = SS_SS_SEOCategory.SeoTemplateId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @smartSEOEntityTypeId AND EntityId = SeoTemplateId AND MappedEntityId = CategoryId AND MappingType = @categoyMappingTypeId_SS)

		INSERT INTO SS_MAP_EntityMapping
		SELECT @smartSEOEntityTypeId, SeoTemplateId, ManufacturerId, 0, @manufacturerMappingTypeId_SS FROM SS_SS_SEOManufacturer
		WHERE EXISTS (SELECT NULL FROM SS_SS_SeoTemplate WHERE SS_SS_SeoTemplate.Id = SS_SS_SEOManufacturer.SeoTemplateId)
		AND NOT EXISTS (SELECT NULL FROM SS_MAP_EntityMapping 
		WHERE EntityType = @smartSEOEntityTypeId AND EntityId = SeoTemplateId AND MappedEntityId = ManufacturerId AND MappingType = @manufacturerMappingTypeId_SS)

		DROP TABLE SS_SS_SEOCategory;
		DROP TABLE SS_SS_SEOManufacturer;

	COMMIT TRANSACTION;			
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END

	/* =============== HTML WIDGETS UPGRADE =============== */
IF(EXISTS (SELECT NULL FROM sys.objects WHERE object_id = object_id(N'[dbo].[SS_HW_HtmlWidget]') AND type in (N'U')) 
	AND EXISTS (SELECT NULL FROM sys.columns WHERE object_id = object_id('[SS_HW_HtmlWidget]') and NAME='FromDate'))
BEGIN

	DECLARE @htmlWidgetsEntityTypeId INT
	SET @htmlWidgetsEntityTypeId = 25
	
	BEGIN TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;

		DECLARE @FromDate DATETIME
		DECLARE @ToDate DATETIME
		DECLARE @FromTime TIME
		DECLARE @ToTime TIME

		DECLARE @Id INT
		DECLARE @TempFromDate DATETIME
		DECLARE @TempToDate DATETIME
		DECLARE @Iterator INT
		DECLARE @TotalRecords INT
		
		SET @Iterator = 1
		SET @TotalRecords = (SELECT COUNT(*) FROM SS_HW_HtmlWidget)

		CREATE TABLE #TempTable(
			RowNum int,
			Id int,
			Name nvarchar(MAX), 
			Visible bit,
			HtmlContent nvarchar(MAX),
			FromDate DateTime,
			ToDate DateTime,
			LimitedToStores bit
		)

		INSERT INTO #TempTable
		SELECT RowNum = ROW_NUMBER() OVER(ORDER BY Id), *
		FROM SS_HW_HtmlWidget

		WHILE (@Iterator <= @TotalRecords)
		BEGIN
			SELECT TOP 1 @Id = Id, @TempFromDate = FromDate, @TempToDate = ToDate FROM #TempTable

			IF( @TempFromDate IS NOT NULL )
			BEGIN
				SET @FromDate = cast(@TempFromDate AS DATE)
				SET @FromTime = cast(@TempFromDate AS TIME)     
			END
			ELSE
			BEGIN
				SET @FromDate = NULL
				SET @FromTime = '00:00:00'
			END

			IF( @TempToDate IS NOT NULL )
			BEGIN    
				SET @ToDate = cast(@TempToDate AS DATE)
				SET @ToTime = cast(@TempToDate AS TIME)     
			END
			ELSE
			BEGIN
				SET @ToDate = NULL
				SET @ToTime = '00:00:00'
			END

			INSERT INTO SS_S_Schedule(EntityType, EntityId, EntityFromDate, EntityToDate, SchedulePatternType, SchedulePatternFromTime, SchedulePatternToTime )
			VALUES(@htmlWidgetsEntityTypeId, @Id, @FromDate, @ToDate, 5, @FromTime, @ToTime)

			DELETE FROM #TempTable WHERE RowNum = @Iterator

			SET @Iterator = @Iterator + 1
		END
		
		DROP TABLE #TempTable
		ALTER TABLE SS_HW_HtmlWidget DROP COLUMN [FromDate], COLUMN [ToDate];
	
	COMMIT TRANSACTION;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
END

-- DROP PROCEDURES
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CreateConditionGroup')
	DROP PROCEDURE sp_CreateConditionGroup
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CreatePassOnlyConditionGroup')
	DROP PROCEDURE sp_CreatePassOnlyConditionGroup
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_TransferMappingsToConditions')
	DROP PROCEDURE sp_TransferMappingsToConditions
GO

BEGIN TRY
	DROP FUNCTION dbo.GetMappingsSQL;
END TRY
BEGIN CATCH
    --PRINT 'Function did not exist.';
END CATCH
GO