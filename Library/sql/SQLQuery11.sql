
IF (OBJECT_ID('System_Customer_Setting')) IS NULL
BEGIN
	CREATE TABLE [dbo].[System_Customer_Setting](
	[Key] [nvarchar](50) NOT NULL,
	[Value] [text] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[Type] [bit] NOT NULL,
	 CONSTRAINT [PK_System_Customer_Setting] PRIMARY KEY CLUSTERED 
	(
	[Key] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[System_Customer_Setting] ADD  CONSTRAINT [DF_System_Customer_Setting_IsEnable]  DEFAULT ((0)) FOR [IsEnable]
	ALTER TABLE [dbo].[System_Customer_Setting] ADD  CONSTRAINT [DF_System_Customer_Setting_Type]  DEFAULT ((0)) FOR [Type]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System_Customer_Setting', @level2type=N'COLUMN',@level2name=N'Key'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System_Customer_Setting', @level2type=N'COLUMN',@level2name=N'Value'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用（0:启用，1：禁用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System_Customer_Setting', @level2type=N'COLUMN',@level2name=N'IsEnable'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型（0：主表，1：扩展表）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System_Customer_Setting', @level2type=N'COLUMN',@level2name=N'Type'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System_Customer_Setting'
	
END
	IF @@ERROR>0 PRINT 'Error:Failed to create table System_Customer_Setting!'
GO

