CREATE TABLE [dbo].[EmployeeVaccinations] (
    [IdentityCard]         NVARCHAR (20)  NOT NULL,
    [Vaccine1Date]         DATE           NULL,
    [Vaccine1Manufacturer] NVARCHAR (100) NULL,
    [Vaccine2Date]         DATE           NULL,
    [Vaccine2Manufacturer] NVARCHAR (100) NULL,
    [Vaccine3Date]         DATE           NULL,
    [Vaccine3Manufacturer] NVARCHAR (100) NULL,
    [Vaccine4Date]         DATE           NULL,
    [Vaccine4Manufacturer] NVARCHAR (100) NULL,
    [PositiveResultDate]   DATE           NULL,
    [RecoveryDate]         DATE           NULL,
    CONSTRAINT [FK_EmployeeVaccinations_EmployeePersonalDetails] FOREIGN KEY ([IdentityCard]) REFERENCES [dbo].[EmployeePersonalDetails] ([IdentityCard])
);

