DECLARE @COUNT integer = 100, @@Line varchar(30) = 'Commercial', @Line varchar(30) = 'Line'

SELECT TOP (@COUNT) * FROM [TestDB].[dbo].[FL Insurance Sample] WHERE QUOTENAME(@Line) = @@Line--WHERE (@Line) = @@Line