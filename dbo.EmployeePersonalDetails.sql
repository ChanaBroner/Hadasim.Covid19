CREATE TABLE [dbo].[EmployeePersonalDetails] (
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [IdentityCard] NVARCHAR (20)  NOT NULL,
    [City]         NVARCHAR (100) NULL,
    [Street]       NVARCHAR (100) NULL,
    [HouseNumber]  NVARCHAR (10)  NULL,
    [DateOfBirth]  DATE           NULL,
    [Phone]        NVARCHAR (20)  NULL,
    [MobilePhone]  NVARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([IdentityCard] ASC)
);

